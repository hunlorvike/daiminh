using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("bai-viet")]
public partial class ArticleController(
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ArticleController : DaiminhController
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("ve-chung-toi")]
    public IActionResult AboutUs()
    {
        return View();
    }

    [HttpGet("{id}")]
    public IActionResult Detail(string id)
    {
        return View();
    }
}