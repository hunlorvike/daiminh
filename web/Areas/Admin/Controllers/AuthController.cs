using core.Common.Constants;
using Core.Common.Models;
using core.Entities;
using core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Models;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

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
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            User user = new()
            {
                Username = model.Username,
                PasswordHash = model.Password
            };

            var response = await _authService.SignInAsync(user, CookiesConstants.AdminCookieSchema);

            switch (response)
            {
                case SuccessResponse<User> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;

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
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            User newUser = new()
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = model.Password
            };

            var response = await _authService.SignUpAsync(newUser);

            switch (response)
            {
                case SuccessResponse<User> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;

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
        await _authService.SignOutAsync(CookiesConstants.AdminCookieSchema);

        return RedirectToAction("Login", "Auth", new { area = "Admin" });
    }
}