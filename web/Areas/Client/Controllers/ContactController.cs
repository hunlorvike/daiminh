using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ContactController : Controller
{
    
    [Route("/lien-he")]
    public IActionResult Index()
    {
        return View();
    }
}