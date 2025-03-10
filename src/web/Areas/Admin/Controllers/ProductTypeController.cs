using application.Interfaces;
using AutoMapper;
using domain.Constants;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.ProductType;
using web.Areas.Admin.Requests.ProductType;
using web.Attributes;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ProductTypeController(
    IProductTypeService productTypeService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ProductTypeController
{
    public async Task<IActionResult> Index()
    {
        var productTypes = await productTypeService.GetAllAsync();
        List<ProductTypeViewModel> models = _mapper.Map<List<ProductTypeViewModel>>(productTypes);
        return View(models);
    }

    [AjaxOnly]
    public IActionResult Create()
    {
        return PartialView("_Create.Modal", new ProductTypeCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await productTypeService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<ProductTypeUpdateRequest>(response);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await productTypeService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<ProductTypeDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }
}

public partial class ProductTypeController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductTypeCreateRequest model)
    {
        var validator = GetValidator<ProductTypeCreateRequest>();
        if (await this.ValidateAndReturnView(validator, model)) return PartialView("_Create.Modal", model);

        try
        {
            var newProductType = _mapper.Map<ProductType>(model);

            var response = await productTypeService.AddAsync(newProductType);

            switch (response)
            {
                case SuccessResponse<ProductType> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ProductType", new { area = "Admin" })
                    });
                case SuccessResponse<ProductType> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ProductType", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return PartialView("_Create.Modal", model);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Errors = ex.Message
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductTypeUpdateRequest model)
    {
        var validator = GetValidator<ProductTypeUpdateRequest>();
        if (await this.ValidateAndReturnView(validator, model)) return PartialView("_Edit.Modal", model);

        try
        {
            var productType = await productTypeService.GetByIdAsync(model.Id);
            if (productType == null) return NotFound();

            _mapper.Map(model, productType);

            var response = await productTypeService.UpdateAsync(model.Id, productType);

            switch (response)
            {
                case SuccessResponse<ProductType> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ProductType", new { area = "Admin" })
                    });
                case SuccessResponse<ProductType> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ProductType", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return PartialView("_Edit.Modal", model);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Errors = ex.Message
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ProductTypeDeleteRequest model)
    {
        try
        {
            var response = await productTypeService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<ProductType> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ProductType", new { area = "Admin" })
                    });
                case SuccessResponse<ProductType> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ProductType", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return PartialView("_Delete.Modal", model);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Errors = ex.Message
            });
        }
    }
}