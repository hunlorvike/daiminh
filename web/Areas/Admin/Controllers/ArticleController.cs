using AutoMapper;
using core.Attributes;
using core.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ArticleController(
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ArticleController
{
    public IActionResult Index()
    {
        return View();
    }

    [AjaxOnly]
    public IActionResult Create()
    {
        return PartialView("_Create.Modal");
    }
}