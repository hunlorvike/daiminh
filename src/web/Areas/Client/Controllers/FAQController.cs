using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("cau-hoi-thuong-gap")]
public class FAQController : Controller
{
    // GET: /cau-hoi-thuong-gap
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}