using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ContactController : Controller
{
    [HttpGet("lien-he")]
    public IActionResult Index()
    {
        return View();
    }
}