using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("/tim-kiem")]
public partial class SearchController(
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration);

public partial class SearchController : DaiminhController
{
    public IActionResult Index()
    {
        return View();
    }
}