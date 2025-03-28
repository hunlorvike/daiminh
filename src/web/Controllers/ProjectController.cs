using Microsoft.AspNetCore.Mvc;

namespace web.Controllers;

public class ProjectController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
