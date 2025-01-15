using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class SearchController : Controller
{
    public IActionResult Index() => View();
}