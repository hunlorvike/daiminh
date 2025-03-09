using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public partial class HomeController(
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration);

public partial class HomeController
{
    public IActionResult Index()
    {
        return View();
    }
}