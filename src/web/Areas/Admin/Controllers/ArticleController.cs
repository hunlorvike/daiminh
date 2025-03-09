using AutoMapper;
using domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;
using web.Attributes;

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

    [AjaxOnly]
    public IActionResult Edit(int id)
    {
        return PartialView("_Edit.Modal");
    }

    [AjaxOnly]
    public IActionResult Details(int id)
    {
        return PartialView("_Detail.Modal");
    }

    [AjaxOnly]
    public IActionResult Delete(int id)
    {
        return PartialView("_Delete.Modal");
    }
}

public partial class ArticleController
{
}