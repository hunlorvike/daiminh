using Microsoft.AspNetCore.Mvc;

namespace web.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}