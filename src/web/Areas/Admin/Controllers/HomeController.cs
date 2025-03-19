using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using shared.Constants;

using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}, {RoleConstants.Manager}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class HomeController(
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class HomeController
{
    public IActionResult Index()
    {
        return View();
    }
}