using System.Security.Claims;
using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
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
    private readonly IMapper _mapper;
    private readonly IValidator<LoginViewModel> _validator;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<LoginViewModel> validator,
        IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _passwordHasher = passwordHasher;
    }


    // GET: Admin/Account/Login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["PageTitle"] = "Đăng nhập";

        var viewModel = new LoginViewModel
        {
            ReturnUrl = returnUrl
        };

        return View(viewModel);
    }

    // POST: Admin/Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        ViewData["PageTitle"] = "Đăng nhập";

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        // Find user by username or email
        var user = await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == viewModel.Username || u.Email == viewModel.Username);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
            return View(viewModel);
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, viewModel.Password);
        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
            return View(viewModel);
        }

        // Create claims for the authenticated user
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "DaiMinhCookies");
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = viewModel.RememberMe,
            RedirectUri = viewModel.ReturnUrl
        };

        await HttpContext.SignInAsync(
            "DaiMinhCookies",
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        // Redirect to returnUrl or default page
        if (!string.IsNullOrEmpty(viewModel.ReturnUrl) && Url.IsLocalUrl(viewModel.ReturnUrl))
        {
            return Redirect(viewModel.ReturnUrl);
        }

        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
    }

    // GET: Admin/Account/Logout
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("DaiMinhCookies");
        return RedirectToAction("Login");
    }
}