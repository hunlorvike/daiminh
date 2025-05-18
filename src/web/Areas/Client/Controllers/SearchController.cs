using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class SearchController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Search(string keyword)
    {
        return View("Index", keyword);
    }
}