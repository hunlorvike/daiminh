using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ProductController : Controller
{
    [HttpGet("san-pham")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("san-pham/{slug}")]  
    public IActionResult Detail(string slug)
    {
        return View();
    }
}