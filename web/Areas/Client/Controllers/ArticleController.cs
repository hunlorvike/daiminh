using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ArticleController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Detail() => View();
}