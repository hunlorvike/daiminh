using Microsoft.AspNetCore.Mvc;
using web.Areas.Client.Services.Interfaces; // Thêm using

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class HomeController : Controller
{
    private readonly IHomeService _homeService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IHomeService homeService, ILogger<HomeController> logger)
    {
        _homeService = homeService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = await _homeService.GetHomeViewModelAsync();
        return View(viewModel);
    }
}