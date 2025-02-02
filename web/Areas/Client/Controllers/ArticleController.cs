using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ArticleController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("/about-us")]
    public IActionResult AboutUs()
    {
        return View();
    }

    public IActionResult Detail()
    {
        return View();
    }
}