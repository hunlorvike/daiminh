using application.Interfaces;
using AutoMapper;
using domain.Constants;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.ProductFieldDefinition;
using web.Areas.Admin.Requests.ProductFieldDefinition;
using web.Attributes;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ProductFieldDefinitionController(
    IProductFieldDefinitionService productFieldDefinitionService,
    IProductTypeService productTypeService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ProductFieldDefinitionController
{
    public async Task<IActionResult> Index()
    {
        var productFieldDefinitions = await productFieldDefinitionService.GetAllAsync();
        List<ProductFieldDefinitionViewModel> models =
            _mapper.Map<List<ProductFieldDefinitionViewModel>>(productFieldDefinitions);
        return View(models);
    }


    [AjaxOnly]
    public async Task<IActionResult> Create(int? productTypeId = null)
    {
        var model = new ProductFieldDefinitionCreateRequest();
        if (productTypeId.HasValue) model.ProductTypeId = productTypeId.Value;

        await PopulateProductTypeDropdown();
        await PopulateFieldTypeDropdown();
        return PartialView("_Create.Modal", model);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await productFieldDefinitionService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<ProductFieldDefinitionUpdateRequest>(response);

        await PopulateProductTypeDropdown();
        await PopulateFieldTypeDropdown();
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await productFieldDefinitionService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<ProductFieldDefinitionDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }

    private async Task PopulateProductTypeDropdown()
    {
        var productTypes = await productTypeService.GetAllAsync();
        ViewBag.ProductTypes = new SelectList(productTypes, "Id", "Name");
    }

    private async Task PopulateFieldTypeDropdown()
    {
        var fieldTypes = Enum.GetValues(typeof(FieldType))
            .Cast<FieldType>()
            .Select(ft => new SelectListItem
            {
                Value = ft.ToString(),
                Text = ft.ToString()
            });
        ViewBag.FieldTypes = new SelectList(fieldTypes, "Value", "Text");
    }
}

public partial class ProductFieldDefinitionController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductFieldDefinitionCreateRequest model)
    {
        var validator = GetValidator<ProductFieldDefinitionCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateProductTypeDropdown();
            await PopulateFieldTypeDropdown();
            return result;
        }

        try
        {
            var newProductFieldDefinition = _mapper.Map<ProductFieldDefinition>(model);
            var response = await productFieldDefinitionService.AddAsync(newProductFieldDefinition);

            switch (response)
            {
                case SuccessResponse<ProductFieldDefinition> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ProductFieldDefinition", new { area = "Admin" })
                    });
                case SuccessResponse<ProductFieldDefinition> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ProductFieldDefinition", new { area = "Admin" });
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
    public async Task<IActionResult> Edit(ProductFieldDefinitionUpdateRequest model)
    {
        var validator = GetValidator<ProductFieldDefinitionUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateProductTypeDropdown();
            await PopulateFieldTypeDropdown();
            return result;
        }

        try
        {
            var productFieldDefinition = await productFieldDefinitionService.GetByIdAsync(model.Id);
            if (productFieldDefinition == null) return NotFound();

            _mapper.Map(model, productFieldDefinition);

            var response = await productFieldDefinitionService.UpdateAsync(model.Id, productFieldDefinition);

            switch (response)
            {
                case SuccessResponse<ProductFieldDefinition> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ProductFieldDefinition", new { area = "Admin" })
                    });
                case SuccessResponse<ProductFieldDefinition> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ProductFieldDefinition", new { area = "Admin" });
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
            if (Request.IsAjaxRequest())
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });

            ModelState.AddModelError("", ex.Message);
            return PartialView("_Edit.Modal", model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ProductFieldDefinitionDeleteRequest model)
    {
        try
        {
            var response = await productFieldDefinitionService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<ProductFieldDefinition> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ProductFieldDefinition", new { area = "Admin" })
                    });
                case SuccessResponse<ProductType> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ProductFieldDefinition", new { area = "Admin" });
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