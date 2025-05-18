using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class CartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Checkout()
    {
        return View();
    }
}