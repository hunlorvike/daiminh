using AutoMapper;

using domain.Entities;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;

using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.ProductType;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class ProductTypeController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache)
{
    public async Task<IActionResult> Index()
    {
        var productTypes = await dbContext.ProductTypes
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .ToListAsync();

        return View(productTypes);
    }

    [AjaxOnly]
    public IActionResult Create()
    {
        return PartialView("_Create.Modal", new ProductTypeCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var productType = await dbContext.ProductTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.Id == id && pt.DeletedAt == null) ?? throw new NotFoundException("Product type not found.");
        var request = _mapper.Map<ProductTypeUpdateRequest>(productType);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var productType = await dbContext.ProductTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.Id == id && pt.DeletedAt == null) ?? throw new NotFoundException("Product type not found.");
        var request = _mapper.Map<ProductTypeDeleteRequest>(productType);
        return PartialView("_Delete.Modal", request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductTypeCreateRequest model)
    {
        var validator = GetValidator<ProductTypeCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var newProductType = _mapper.Map<ProductType>(model);
            await dbContext.ProductTypes.AddAsync(newProductType);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ProductType>(newProductType, "Thêm loại sản phẩm mới thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ProductType", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ProductType", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error creating product type.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductTypeUpdateRequest model)
    {
        var validator = GetValidator<ProductTypeUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var productType = await dbContext.ProductTypes
                .FirstOrDefaultAsync(pt => pt.Id == model.Id && pt.DeletedAt == null);

            if (productType == null) return NotFound();

            _mapper.Map(model, productType);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ProductType>(productType, "Cập nhật loại sản phẩm thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ProductType", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ProductType", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error updating product type.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ProductTypeDeleteRequest model)
    {
        try
        {
            var productType = await dbContext.ProductTypes
                .FirstOrDefaultAsync(pt => pt.Id == model.Id && pt.DeletedAt == null);

            dbContext.ProductTypes.Remove(productType!);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ProductType>(productType!, "Xóa loại sản phẩm thành công (đã ẩn).");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ProductType", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ProductType", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error deleting product type.", ex);
        }
    }
}