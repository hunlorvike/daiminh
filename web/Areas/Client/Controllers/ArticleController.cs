using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ArticleController : Controller
{
    public IActionResult Index() => View();

    [Route("/about-us")]
    public IActionResult AboutUs() => View();

    public IActionResult Detail() => View();
}