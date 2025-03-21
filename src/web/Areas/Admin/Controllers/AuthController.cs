using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using shared.Constants;
using shared.Extensions;
using System.Security.Claims;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.Auth;
using BC = BCrypt.Net.BCrypt;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class AuthController(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache)
{
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity is { IsAuthenticated: true })
            return RedirectToAction("Index", "Home", new { area = "Admin" });

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [AllowAnonymous]
    public IActionResult Register(string? returnUrl = null)
    {
        if (User.Identity is { IsAuthenticated: true })
            return RedirectToAction("Login", "Auth", new { area = "Admin" });

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest model, string? returnUrl = null)
    {
        var validator = GetValidator<LoginRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            User user = _mapper.Map<User>(model);

            User? existingUser = await context.Users
                .Include(u => u.Role)
                .Where(u => u.Username == user.Username)
                .FirstOrDefaultAsync();

            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, existingUser!.Id.ToString()),
                new(ClaimTypes.Email, existingUser.Email),
                new(ClaimTypes.Role, existingUser.Role?.Name ?? string.Empty)
            ];

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookiesConstants.AdminCookieSchema);
            AuthenticationProperties authProperties = new()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            };

            await httpContextAccessor.HttpContext!.SignInAsync(
                CookiesConstants.AdminCookieSchema,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Json(new
            {
                success = true,
                message = "Đăng nhập thành công.",
                redirectUrl = returnUrl ?? Url.Action("Index", "Home", new { area = "Admin" })
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Errors = ex.Message
            });
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequest model, string? returnUrl = null)
    {
        var validator = GetValidator<RegisterRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var user = _mapper.Map<User>(model);

            Role? defaultRole = await context.Roles
                .Where(r => r.Name == RoleConstants.User)
                .FirstOrDefaultAsync() ??
                throw new Exception("Không tìm thấy role mặc định. Vui lòng kiểm tra lại cấu hình hệ thống.");

            user.PasswordHash = BC.HashPassword(user.PasswordHash);
            user.RoleId = defaultRole.Id;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            if (Request.IsAjaxRequest())
                return Json(new
                {
                    success = true,
                    message = "Đăng ký tài khoản thành công.",
                    redirectUrl = Url.Action("Login", "Auth", new { area = "Admin" })
                });

            TempData["SuccessMessage"] = "Đăng ký tài khoản thành công.";
            return RedirectToAction("Login", "Auth", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Errors = ex.Message
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await httpContextAccessor.HttpContext!.SignOutAsync(CookiesConstants.AdminCookieSchema);
            return RedirectToAction("Login", "Auth", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Errors = ex.Message
            });
        }
    }
}