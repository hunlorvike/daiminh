using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }
}