using AutoMapper;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Attributes;
using shared.Constants;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Product;

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

        var viewModels = products.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Slug = p.Slug,
            BasePrice = p.BasePrice,
            Sku = p.Sku,
            Status = p.Status,
            ProductTypeId = p.ProductTypeId,
            ProductTypeName = p.ProductType?.Name ?? "N/A",
            PrimaryImage = p.Images?.FirstOrDefault(i => i.IsPrimary)?.ImageUrl ??
                              p.Images?.OrderBy(i => i.DisplayOrder).FirstOrDefault()?.ImageUrl,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();

        return View(viewModels);
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