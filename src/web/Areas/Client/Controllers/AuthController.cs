using System.Security.Claims;
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
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Requests.Auth;
using BC = BCrypt.Net.BCrypt;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public partial class AuthController(
    IMapper mapper,
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class AuthController
{
    [HttpGet("dang-nhap")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity is { IsAuthenticated: true })
            return RedirectToAction("Index", "Home", new { area = "Client" });

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpGet("dang-ky")]
    public IActionResult Register(string? returnUrl = null)
    {
        if (User.Identity is { IsAuthenticated: true })
            return RedirectToAction("Login", "Auth", new { area = "Client" });

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
}

public partial class AuthController
{
    [HttpPost("dang-nhap")]
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

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookiesConstants.UserCookieSchema);
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
                redirectUrl = returnUrl ?? Url.Action("Index", "Home", new { area = "Client" })
            });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error during login.", ex);
        }
    }

    [HttpPost("dang-ky")]
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
                            throw new BusinessLogicException("Default role not found. Please check system configuration.");

            user.PasswordHash = BC.HashPassword(user.PasswordHash);
            user.RoleId = defaultRole.Id;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            if (Request.IsAjaxRequest())
                return Json(new
                {
                    success = true,
                    message = "Đăng ký tài khoản thành công.",
                    redirectUrl = Url.Action("Login", "Auth", new { area = "Client" })
                });

            TempData["SuccessMessage"] = "Đăng ký tài khoản thành công.";
            return RedirectToAction("Login", "Auth", new { area = "Client" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error during registration.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(AuthenticationSchemes = CookiesConstants.UserCookieSchema)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await httpContextAccessor.HttpContext!.SignOutAsync(CookiesConstants.UserCookieSchema);
            return RedirectToAction("Login", "Auth", new { area = "Client" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error during logout.", ex);
        }
    }
}