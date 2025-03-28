using Microsoft.AspNetCore.Mvc;

namespace web.Controllers;

public class PricingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
