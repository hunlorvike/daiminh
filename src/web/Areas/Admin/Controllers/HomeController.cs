using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using shared.Constants;

using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}, {RoleConstants.Manager}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class HomeController(
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