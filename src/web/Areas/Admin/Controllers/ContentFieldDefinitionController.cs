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
using web.Areas.Admin.Models.ContentFieldDefinition;
using web.Areas.Admin.Requests.ContentFieldDefinition;
using web.Attributes;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ContentFieldDefinitionController(
    IContentFieldDefinitionService contentFieldDefinitionService,
    IContentTypeService contentTypeService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ContentFieldDefinitionController
{
    public async Task<IActionResult> Index()
    {
        var contentFieldDefinitions = await contentFieldDefinitionService.GetAllAsync();
        List<ContentFieldDefinitionViewModel> models =
            _mapper.Map<List<ContentFieldDefinitionViewModel>>(contentFieldDefinitions);
        return View(models);
    }


    [AjaxOnly]
    public async Task<IActionResult> Create(int? contentTypeId = null)
    {
        var model = new ContentFieldDefinitionCreateRequest();
        if (contentTypeId.HasValue) model.ContentTypeId = contentTypeId.Value;

        await PopulateContentTypeDropdown();
        await PopulateFieldTypeDropdown();
        return PartialView("_Create.Modal", model);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await contentFieldDefinitionService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<ContentFieldDefinitionUpdateRequest>(response);

        await PopulateContentTypeDropdown();
        await PopulateFieldTypeDropdown();
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await contentFieldDefinitionService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<ContentFieldDefinitionDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }

    private async Task PopulateContentTypeDropdown()
    {
        var contentTypes = await contentTypeService.GetAllAsync();
        ViewBag.ContentTypes = new SelectList(contentTypes, "Id", "Name");
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

public partial class ContentFieldDefinitionController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContentFieldDefinitionCreateRequest model)
    {
        var validator = GetValidator<ContentFieldDefinitionCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateContentTypeDropdown();
            await PopulateFieldTypeDropdown();
            return result;
        }

        try
        {
            var newContentFieldDefinition = _mapper.Map<ContentFieldDefinition>(model);
            var response = await contentFieldDefinitionService.AddAsync(newContentFieldDefinition);

            switch (response)
            {
                case SuccessResponse<ContentFieldDefinition> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ContentFieldDefinition", new { area = "Admin" })
                    });
                case SuccessResponse<ContentFieldDefinition> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ContentFieldDefinition", new { area = "Admin" });
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
    public async Task<IActionResult> Edit(ContentFieldDefinitionUpdateRequest model)
    {
        var validator = GetValidator<ContentFieldDefinitionUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateContentTypeDropdown();
            await PopulateFieldTypeDropdown();
            return result;
        }

        try
        {
            var contentFieldDefinition = await contentFieldDefinitionService.GetByIdAsync(model.Id);
            if (contentFieldDefinition == null) return NotFound();

            _mapper.Map(model, contentFieldDefinition);

            var response = await contentFieldDefinitionService.UpdateAsync(model.Id, contentFieldDefinition);

            switch (response)
            {
                case SuccessResponse<ContentFieldDefinition> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ContentFieldDefinition", new { area = "Admin" })
                    });
                case SuccessResponse<ContentFieldDefinition> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ContentFieldDefinition", new { area = "Admin" });
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
    public async Task<IActionResult> Delete(ContentFieldDefinitionDeleteRequest model)
    {
        try
        {
            var response = await contentFieldDefinitionService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<ContentFieldDefinition> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ContentFieldDefinition", new { area = "Admin" })
                    });
                case SuccessResponse<ContentType> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ContentFieldDefinition", new { area = "Admin" });
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