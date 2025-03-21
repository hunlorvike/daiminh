using AutoMapper;

using domain.Entities;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;

using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.ProductFieldDefinition;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class ProductFieldDefinitionController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache)
{
    public async Task<IActionResult> Index()
    {
        var productFieldDefinitions = await dbContext.ProductFieldDefinitions
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Include(x => x.ProductType)
            .ToListAsync();

        return View(productFieldDefinitions);
    }

    [AjaxOnly]
    public async Task<IActionResult> Create(int? productTypeId = null)
    {
        var model = new ProductFieldDefinitionCreateRequest();
        if (productTypeId.HasValue) model.ProductTypeId = productTypeId.Value;

        await PopulateProductTypeDropdown();
        return PartialView("_Create.Modal", model);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var productFieldDefinition = await dbContext.ProductFieldDefinitions
            .AsNoTracking()
            .FirstOrDefaultAsync(pfd => pfd.Id == id && pfd.DeletedAt == null) ?? throw new NotFoundException("Product field definition not found.");
        var request = _mapper.Map<ProductFieldDefinitionUpdateRequest>(productFieldDefinition);
        await PopulateProductTypeDropdown();
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var productFieldDefinition = await dbContext.ProductFieldDefinitions
            .AsNoTracking()
            .FirstOrDefaultAsync(pfd => pfd.Id == id && pfd.DeletedAt == null) ?? throw new NotFoundException("Product field definition not found.");
        var request = _mapper.Map<ProductFieldDefinitionDeleteRequest>(productFieldDefinition);
        return PartialView("_Delete.Modal", request);
    }

    private async Task PopulateProductTypeDropdown()
    {
        var productTypes = await dbContext.ProductTypes
            .AsNoTracking()
            .Where(pt => pt.DeletedAt == null)
            .ToListAsync();
        ViewBag.ProductTypes = new SelectList(productTypes, "Id", "Name");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductFieldDefinitionCreateRequest model)
    {
        var validator = GetValidator<ProductFieldDefinitionCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var newProductFieldDefinition = _mapper.Map<ProductFieldDefinition>(model);
            await dbContext.ProductFieldDefinitions.AddAsync(newProductFieldDefinition);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ProductFieldDefinition>(newProductFieldDefinition, "Thêm định nghĩa trường sản phẩm mới thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ProductFieldDefinition", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ProductFieldDefinition", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error creating product field definition.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductFieldDefinitionUpdateRequest model)
    {
        var validator = GetValidator<ProductFieldDefinitionUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var productFieldDefinition = await dbContext.ProductFieldDefinitions
                .FirstOrDefaultAsync(pfd => pfd.Id == model.Id && pfd.DeletedAt == null);

            if (productFieldDefinition == null) return NotFound();

            _mapper.Map(model, productFieldDefinition);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ProductFieldDefinition>(productFieldDefinition, "Cập nhật định nghĩa trường sản phẩm thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ProductFieldDefinition", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ProductFieldDefinition", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error updating product field definition.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ProductFieldDefinitionDeleteRequest model)
    {
        try
        {
            var productFieldDefinition = await dbContext.ProductFieldDefinitions
                .FirstOrDefaultAsync(pfd => pfd.Id == model.Id && pfd.DeletedAt == null);

            dbContext.ProductFieldDefinitions.Remove(productFieldDefinition!);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ProductFieldDefinition>(productFieldDefinition!, "Xóa định nghĩa trường sản phẩm thành công (đã ẩn).");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ProductFieldDefinition", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ProductFieldDefinition", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error deleting product field definition.", ex);
        }
    }
}