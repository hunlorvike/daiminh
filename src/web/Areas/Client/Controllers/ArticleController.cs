using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ArticleController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detail(string slug)
    {
        return View();
    }

    public IActionResult ListByCategory(string categorySlug)
    {
        return View();
    }

    public IActionResult ListByTag(string tagSlug)
    {
        return View();
    }
}