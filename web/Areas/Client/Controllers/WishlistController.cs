using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("/yeu-thich")]
public class WishlistController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}