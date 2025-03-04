using AutoMapper;
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
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration);

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
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var user = _mapper.Map<User>(model);

            var response = await authService.SignInAsync(user, CookiesConstants.AdminCookieSchema);

            switch (response)
            {
                case SuccessResponse<User> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = returnUrl ?? Url.Action("Index", "Home", new { area = "Admin" })
                    });
                case SuccessResponse<User> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                {
                    return BadRequest(errorResponse);
                }
            }

            return View(model);
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

            var response = await authService.SignUpAsync(user);

            switch (response)
            {
                case SuccessResponse<User> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Login", "Auth", new { area = "Admin" })
                    });
                case SuccessResponse<User> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Login", "Auth", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                {
                    return BadRequest(errorResponse);
                }
            }

            return View(model);
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

    [Authorize(AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
    public async Task<IActionResult> Logout()
    {
        await authService.SignOutAsync(CookiesConstants.AdminCookieSchema);
        return RedirectToAction("Login", "Auth", new { area = "Admin" });
    }
}