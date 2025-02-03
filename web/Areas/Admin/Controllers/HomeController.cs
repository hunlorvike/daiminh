using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "RequireAdminAuth")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}