using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Security.Claims;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")]
public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AccountController> _logger;
    private const string AuthenticationScheme = "AdminScheme";

    public AccountController(
        IAuthService authService,
        ILogger<AccountController> logger)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: /Admin/Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true && User.Identity.AuthenticationType == AuthenticationScheme)
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

        if (User.Identity?.IsAuthenticated == true && User.Identity.AuthenticationType == AuthenticationScheme)
        {
            return LocalRedirect(returnUrl ?? Url.Action("Index", "Dashboard", new { Area = "Admin" }) ?? "/Admin");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Login failed - ViewModel validation failed for {Username}", model.Username);
            return View(model);
        }

        var loginResult = await _authService.AuthenticateAdminUserAsync(model.Username, model.Password);

        if (!loginResult.Success)
        {
            _logger.LogWarning("Login failed for user {Username}. Service message: {Message}", model.Username, loginResult.Message);
            ModelState.AddModelError(string.Empty, loginResult.Message ?? "Đăng nhập thất bại.");
            foreach (var error in loginResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View(model);
        }

        var claimsIdentity = new ClaimsIdentity(loginResult.Claims, AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            IsPersistent = model.RememberMe,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
        };

        await HttpContext.SignInAsync(AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        _logger.LogInformation("User {Username} signed in successfully via {AuthenticationScheme}", model.Username, AuthenticationScheme);

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Thành công", loginResult.Message ?? "Đăng nhập thành công!", ToastType.Success)
        );

        return LocalRedirect(returnUrl ?? Url.Action("Index", "Dashboard", new { Area = "Admin" }) ?? "/Admin");
    }

    // GET: /Admin/Account/Logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        var userName = User.Identity?.Name ?? "Unknown";
        await HttpContext.SignOutAsync(AuthenticationScheme);

        _logger.LogInformation("User {Username} signed out successfully via {AuthenticationScheme}", userName, AuthenticationScheme);

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