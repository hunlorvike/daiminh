using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class MediaController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}