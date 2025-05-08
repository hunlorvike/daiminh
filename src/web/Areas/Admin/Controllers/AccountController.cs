using System.Security.Claims;
using System.Text.Json;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Constants;
using shared.Enums;
using shared.Models;
using web.Areas.Admin.ViewModels.Account;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<AccountController> _logger;
    private const string AuthenticationScheme = "DaiMinhCookies";

    public AccountController(
        ApplicationDbContext context,
        IPasswordHasher<User> passwordHasher,
        ILogger<AccountController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: /Admin/Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return LocalRedirect(returnUrl ?? Url.Action("Index", "Dashboard", new { Area = "Admin" }) ?? "/Admin");
        }
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: /Admin/Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (User.Identity?.IsAuthenticated == true)
        {
            return LocalRedirect(returnUrl ?? Url.Action("Index", "Dashboard", new { Area = "Admin" }) ?? "/Admin");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _context.Set<User>()
                                 .FirstOrDefaultAsync(u => u.Username.ToLower() == model.Username.ToLower());

        if (user == null)
        {
            _logger.LogWarning("Đăng nhập thất bại - sai tên đăng nhập: {Username}", model.Username);
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
            return View(model);
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Đăng nhập thất bại - tài khoản bị khóa: {Username}", user.Username);
            ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa.");
            return View(model);
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Đăng nhập thất bại - sai mật khẩu: {Username}", user.Username);
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        };

        var claimsIdentity = new ClaimsIdentity(claims, AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            IsPersistent = model.RememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
        };

        await HttpContext.SignInAsync(AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        _logger.LogInformation("Đăng nhập thành công: {Username}", user.Username);
        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Thành công", "Đăng nhập thành công!", ToastType.Success)
        );

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            try
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                _context.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Rehash mật khẩu sau khi đăng nhập cho: {Username}", user.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Không thể rehash mật khẩu cho user '{Username}'", user.Username);
            }
        }

        return LocalRedirect(returnUrl ?? Url.Action("Index", "Dashboard", new { Area = "Admin" }) ?? "/Admin");
    }

    // GET: /Admin/Account/Logout
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var userName = User.Identity?.Name ?? "Unknown";
        await HttpContext.SignOutAsync(AuthenticationScheme);
        _logger.LogInformation("Đăng xuất: {Username}", userName);
        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Thành công", "Đăng xuất thành công!", ToastType.Success)
        );

        return RedirectToAction(nameof(Login));
    }


    // GET: /Admin/Account/AccessDenied
    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

}