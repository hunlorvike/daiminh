using Microsoft.AspNetCore.Mvc;

namespace web.Controllers;

public class ArticleController : Controller
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