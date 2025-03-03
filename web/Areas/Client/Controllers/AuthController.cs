using AutoMapper;
using core.Common.Constants;
using core.Common.Extensions;
using Core.Common.Models;
using core.Entities;
using core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Requests.Auth;
using core.Interfaces.Service;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("Auth")]
public partial class AuthController(
    IMapper mapper,
    IAuthService authService,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class AuthController
{
    /*  
     
      [Route("/quen-mat-khau")]
      public IActionResult ForgotPassword()
      {
          return View();
      }*/
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

        if (await this.ValidateAndReturnView(validator, model)) return View(model);

        try
        {
            var user = _mapper.Map<User>(model);

            var response = await authService.SignInAsync(user, CookiesConstants.UserCookieSchema);

            switch (response)
            {
                case SuccessResponse<User> successResponse:
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home", new { area = "Client" });
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

    [HttpPost("dang-ky")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterRequest model, string? returnUrl = null)
    {
        var validator = GetValidator<RegisterRequest>();

        if (await this.ValidateAndReturnView(validator, model)) return View(model);

        try
        {
            var user = _mapper.Map<User>(model);

            var response = await authService.SignUpAsync(user);

            switch (response)
            {
                case SuccessResponse<User> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Login", "Auth", new { area = "Client" });
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

    [Authorize(AuthenticationSchemes = CookiesConstants.UserCookieSchema)]
    public async Task<IActionResult> Logout()
    {
        await authService.SignOutAsync(CookiesConstants.UserCookieSchema);
        return RedirectToAction("Login", "Auth", new { area = "Client" });
    }
}