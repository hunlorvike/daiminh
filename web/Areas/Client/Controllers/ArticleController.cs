using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("bai-viet")]
public class ArticleController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("ve-chung-toi")]
    public IActionResult AboutUs()
    {
        return View();
    }

    [HttpGet("{id}")]
    public IActionResult Detail(string id)
    {
        return View();
    }
}