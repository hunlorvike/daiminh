using AutoMapper;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using shared.Attributes;
using shared.Constants;

using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}", AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ProductController(
    ApplicationDbContext context,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ProductController
{
    public async Task<IActionResult> Index()
    {
        var products = await context.Products
            .Include(p => p.ProductType)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return View(products);
    }

    [AjaxOnly]
    public async Task<IActionResult> Create()
    {
        return PartialView("_Create.Modal");
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        return PartialView("_Edit.Modal");
    }


    [AjaxOnly]
    public async Task<IActionResult> Delete()
    {
        return PartialView("_Delete.Modal");
    }
}