using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("danh-muc")]
public class CategoryController(
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache)
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