using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class AuthController : Controller
{
    [Route("/dang-nhap")]
    public IActionResult Login()
    {
        return View();
    }

    [Route("/dang-ky")]
    public IActionResult Register()
    {
        return View();
    }

    [Route("/quen-mat-khau")]
    public IActionResult ForgotPassword()
    {
        return View();
    }
}