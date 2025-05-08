using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("san-pham")]
public class ProductController : Controller
{
    // GET: /san-pham
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    // GET: /san-pham/{slug}
    [HttpGet("{slug}")]
    public IActionResult Detail(string slug)
    {
        return View();
    }

    // GET: /san-pham/danh-muc/{categorySlug}
    [HttpGet("danh-muc/{categorySlug}")]
    public IActionResult ListByCategory(string categorySlug)
    {
        return View();
    }

    // GET: /san-pham/the/{tagSlug}
    [HttpGet("the/{tagSlug}")]
    public IActionResult ListByTag(string tagSlug)
    {
        return View();
    }

    // GET: /san-pham/thuong-hieu/{brandSlug}
    [HttpGet("thuong-hieu/{brandSlug}")]
    public IActionResult ListByBrand(string brandSlug)
    {
        return View();
    }
}