using core.Common.Constants;
using core.Common.Extensions;
using core.Entities;
using Core.Common.Models;
using core.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.Auth;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class AuthController(
    IAuthService authService,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(serviceProvider, configuration);

public partial class AuthController
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
}

public partial class AuthController

{
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginRequest model, string? returnUrl = null)
    {
        var validator = GetValidator<LoginRequest>();

        if (await this.ValidateAndReturnView(validator, model)) return View(model);

        try
        {
            User user = new()
            {
                Username = model.Username ?? string.Empty,
                PasswordHash = model.Password ?? string.Empty
            };

            var response = await authService.SignInAsync(user, CookiesConstants.AdminCookieSchema);

            switch (response)
            {
                case SuccessResponse<User> successResponse:
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                case ErrorResponse errorResponse:
                {
                    foreach (var error in errorResponse.Errors) ModelState.AddModelError(error.Key, error.Value);

                    return View(model);
                }
                default:
                    return View(model);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequest model, string? returnUrl = null)
    {
        var validator = GetValidator<RegisterRequest>();

        if (await this.ValidateAndReturnView(validator, model)) return View(model);

        try
        {
            User newUser = new()
            {
                Username = model.Username ?? string.Empty,
                Email = model.Email ?? string.Empty,
                PasswordHash = model.Password ?? string.Empty
            };

            var response = await authService.SignUpAsync(newUser);

            switch (response)
            {
                case SuccessResponse<User> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Login", "Auth", new { area = "Admin" });
                case ErrorResponse errorResponse:
                {
                    foreach (var error in errorResponse.Errors) ModelState.AddModelError(error.Key, error.Value);

                    return View(model);
                }
                default:
                    return View(model);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await authService.SignOutAsync(CookiesConstants.AdminCookieSchema);

        return RedirectToAction("Login", "Auth", new { area = "Admin" });
    }
}