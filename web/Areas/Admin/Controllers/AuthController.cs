using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public class AuthController : Controller
{
    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult SignUp()
    {
        return View();
    }
}