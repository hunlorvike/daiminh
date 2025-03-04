using AutoMapper;
using core.Common.Constants;
using core.Common.Extensions;
using Core.Common.Models;
using core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Requests.Auth;
using core.Interfaces.Service;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public partial class AuthController(
    IMapper mapper,
    IAuthService authService,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class AuthController
{
    [HttpGet("dang-nhap")]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity is { IsAuthenticated: true })
            return RedirectToAction("Index", "Home", new { area = "Client" });

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpGet("dang-ky")]
    [AllowAnonymous]
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
            var user = _mapper.Map<User>(model);

            var response = await authService.SignInAsync(user, CookiesConstants.UserCookieSchema);

            switch (response)
            {
                case SuccessResponse<User> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = returnUrl ?? Url.Action("Index", "Home", new { area = "Client" })
                    });
                case SuccessResponse<User> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Home", new { area = "Client" });
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

            var response = await authService.SignUpAsync(user);

            switch (response)
            {
                case SuccessResponse<User> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Login", "Auth", new { area = "Client" })
                    });
                case SuccessResponse<User> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Login", "Auth", new { area = "Client" });
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

    [Authorize(AuthenticationSchemes = CookiesConstants.UserCookieSchema)]
    public async Task<IActionResult> Logout()
    {
        await authService.SignOutAsync(CookiesConstants.UserCookieSchema);
        return RedirectToAction("Login", "Auth", new { area = "Client" });
    }
}