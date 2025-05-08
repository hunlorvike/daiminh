using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("bai-viet")]
public class ArticleController : Controller
{
    // GET: /bai-viet
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    // GET: /bai-viet/{slug}
    [HttpGet("{slug}")]
    public IActionResult Detail(string slug)
    {
        return View();
    }

    // GET: /bai-viet/danh-muc/{categorySlug}
    [HttpGet("danh-muc/{categorySlug}")]
    public IActionResult ListByCategory(string categorySlug)
    {
        return View();
    }

    // GET: /bai-viet/the/{tagSlug}
    [HttpGet("the/{tagSlug}")]
    public IActionResult ListByTag(string tagSlug)
    {
        return View();
    }
}