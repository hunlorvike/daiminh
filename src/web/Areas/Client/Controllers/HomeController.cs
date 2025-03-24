using AutoMapper;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public partial class HomeController(
    ApplicationDbContext context,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class HomeController
{
    public IActionResult Index()
    {
        return View();

    }
}