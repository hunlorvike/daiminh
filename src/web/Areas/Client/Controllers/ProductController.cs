using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("san-pham")]
public partial class ProductController(
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class ProductController
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("{id}")]
    public IActionResult Detail(string id)
    {
        return View();
    }
}