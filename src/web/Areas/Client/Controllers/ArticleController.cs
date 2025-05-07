using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ArticleController : Controller
{
    [HttpGet("bai-viet")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("bai-viet/{slug}")]
    public IActionResult Detail(string slug)
    {
        return View();
    }
}