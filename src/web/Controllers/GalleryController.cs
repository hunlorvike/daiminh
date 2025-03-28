using Microsoft.AspNetCore.Mvc;

namespace web.Controllers;

public class GalleryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detail()
    {
        return View();
    }
}
