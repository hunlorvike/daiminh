using AutoMapper;
using core.Attributes;
using core.Common.Constants;
using core.Common.Extensions;
using Core.Common.Models;
using core.Entities;
using core.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.ProductType;
using web.Areas.Admin.Requests.ProductType;

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
                case SuccessResponse<ProductType> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;

                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "ProductType", new { area = "Admin" }) });

                    return RedirectToAction("Index", "ProductType", new { area = "Admin" });
                case ErrorResponse errorResponse:
                {
                    foreach (var error in errorResponse.Errors) ModelState.AddModelError(error.Key, error.Value);

                    return PartialView("_Create.Modal", model);
                }
                default:
                    return PartialView("_Create.Modal", model);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return PartialView("_Create.Modal", model);
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
                case SuccessResponse<ProductType> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "ProductType", new { area = "Admin" }) });

                    return RedirectToAction("Index", "ProductType", new { area = "Admin" });
                case ErrorResponse errorResponse:
                    foreach (var error in errorResponse.Errors) ModelState.AddModelError(error.Key, error.Value);

                    return PartialView("_Edit.Modal", model);
                default:
                    return PartialView("_Edit.Modal", model);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return PartialView("_Edit.Modal", model);
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
                case SuccessResponse<ProductType> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "ProductType", new { area = "Admin" }) });

                    return RedirectToAction("Index", "ProductType", new { area = "Admin" });
                case ErrorResponse errorResponse:
                    foreach (var error in errorResponse.Errors) ModelState.AddModelError(error.Key, error.Value);

                    return PartialView("_Delete.Modal", model);
                default:
                    return PartialView("_Delete.Modal", model);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return PartialView("_Delete.Modal", model);
        }
    }
}