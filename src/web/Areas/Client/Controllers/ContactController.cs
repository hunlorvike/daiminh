using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("lien-he")]
public class ContactController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}