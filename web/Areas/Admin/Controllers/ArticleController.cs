using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public class ArticleController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}