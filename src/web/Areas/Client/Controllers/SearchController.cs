using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("tim-kiem")]
public class SearchController : Controller
{
    // GET: /tim-kiem
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    // GET: /tim-kiem/{keyword}
    [HttpGet("{keyword}")]
    public IActionResult Search(string keyword)
    {
        return View("Index", keyword);
    }
}