using System.Security.Claims;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
            return View(model);
        }

        if (!user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa.");
            return View(model);
        }


        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Success || passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var claimsIdentity = new ClaimsIdentity(claims, AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            };

            await HttpContext.SignInAsync(
                AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                try
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to rehash password for user '{Username}' after login.", user.Username);
                }
            }


            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
            return View(model);
        }
    }

    // POST: /Admin/Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var userName = User.Identity?.Name ?? "Unknown";
        await HttpContext.SignOutAsync(AuthenticationScheme);
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