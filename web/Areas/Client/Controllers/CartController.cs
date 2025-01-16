using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class CartController : Controller
{
    [Route("/gio-hang")]
    public IActionResult Index()
    {
        return View();
    }
}