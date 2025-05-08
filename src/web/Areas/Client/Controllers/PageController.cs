using Microsoft.AspNetCore.Mvc;

[Area("Client")]
public class PageController : Controller
{
    // GET: /{pageSlug}
    [HttpGet("{pageSlug}")]
    public IActionResult Index(string pageSlug)
    {
        return View();
    }
}