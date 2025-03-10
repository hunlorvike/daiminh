using application.Interfaces;
using AutoMapper;
using domain.Constants;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.ContentType;
using web.Areas.Admin.Requests.ContentType;
using web.Attributes;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ContentTypeController(
    IContentTypeService contentTypeService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class ContentTypeController
{
    public async Task<IActionResult> Index()
    {
        var contentTypes = await contentTypeService.GetAllAsync();
        List<ContentTypeViewModel> models = _mapper.Map<List<ContentTypeViewModel>>(contentTypes);
        return View(models);
    }


    [AjaxOnly]
    public IActionResult Create()
    {
        return PartialView("_Create.Modal", new ContentTypeCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await contentTypeService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<ContentTypeUpdateRequest>(response);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await contentTypeService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<ContentTypeDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }
}

public partial class ContentTypeController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContentTypeCreateRequest model)
    {
        var validator = GetValidator<ContentTypeCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var newContentType = _mapper.Map<ContentType>(model);
            var response = await contentTypeService.AddAsync(newContentType);

            switch (response)
            {
                case SuccessResponse<ContentType> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ContentType", new { area = "Admin" })
                    });
                case SuccessResponse<ContentType> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ContentType", new { area = "Admin" });
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
    public async Task<IActionResult> Edit(ContentTypeUpdateRequest model)
    {
        var validator = GetValidator<ContentTypeUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var contentType = await contentTypeService.GetByIdAsync(model.Id);
            if (contentType == null) return NotFound();

            _mapper.Map(model, contentType);

            var response = await contentTypeService.UpdateAsync(model.Id, contentType);

            switch (response)
            {
                case SuccessResponse<ContentType> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ContentType", new { area = "Admin" })
                    });
                case SuccessResponse<ContentType> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ContentType", new { area = "Admin" });
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
    public async Task<IActionResult> Delete(ContentTypeDeleteRequest model)
    {
        try
        {
            var response = await contentTypeService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<ContentType> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "ContentType", new { area = "Admin" })
                    });
                case SuccessResponse<ContentType> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ContentType", new { area = "Admin" });
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