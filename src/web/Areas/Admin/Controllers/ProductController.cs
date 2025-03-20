using AutoMapper;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
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
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

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
    public Task<IActionResult> Create()
    {
        return Task.FromResult<IActionResult>(PartialView("_Create.Modal"));
    }

    [AjaxOnly]
    public Task<IActionResult> Edit(int id)
    {
        return Task.FromResult<IActionResult>(PartialView("_Edit.Modal"));
    }


    [AjaxOnly]
    public Task<IActionResult> Delete()
    {
        return Task.FromResult<IActionResult>(PartialView("_Delete.Modal"));
    }
}