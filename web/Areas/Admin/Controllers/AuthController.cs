using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Route("auth")]
public class AuthController : Controller
{
    [HttpGet("sign-in")]
    public IActionResult SignIn() => View();

    [HttpGet("sign-up")]
    public IActionResult SignUp() => View();
}