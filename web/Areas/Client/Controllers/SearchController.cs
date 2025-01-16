using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("/tim-kiem")]
public class SearchController : Controller
{
    public IActionResult Index() => View();
}