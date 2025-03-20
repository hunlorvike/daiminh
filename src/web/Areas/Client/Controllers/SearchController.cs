using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("/tim-kiem")]
public partial class SearchController(
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class SearchController : DaiminhController
{
    public IActionResult Index()
    {
        return View();
    }
}