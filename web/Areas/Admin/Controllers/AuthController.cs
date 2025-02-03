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
    public IActionResult Login(string returnUrl = null)
    {
        // TODO: Nếu người dùng đã đăng nhập, chuyển hướng về trang chủ
        if (User.Identity is { IsAuthenticated: true }) return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [AllowAnonymous]
    public IActionResult Register(string? returnUrl = null)
    {
        // TODO: Nếu người dùng đã đăng nhập, chuyển hướng về trang chủ
        if (User.Identity is { IsAuthenticated: true }) return RedirectToAction("Index", "Home");

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

            await _authService.SignInAsync(user);

            // Hiển thị thông báo thành công
            TempData["SuccessMessage"] = "Đăng nhập thành công!";

            return RedirectToLocal(returnUrl);
        }
        catch (Exception ex)
        {
            // Hiển thị thông báo lỗi
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

            await _authService.SignUpAsync(newUser);

            // Hiển thị thông báo thành công
            TempData["SuccessMessage"] = "Đăng ký thành công!";

            return RedirectToAction("Login", "Auth", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            // Hiển thị thông báo lỗi
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _authService.SignOutAsync();

        // Hiển thị thông báo thành công
        TempData["SuccessMessage"] = "Đăng xuất thành công!";

        return RedirectToAction("Index", "Home");
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

        return RedirectToAction("Index", "Home", new { Area = "Admin" });
    }
}