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
using web.Areas.Admin.Models.ContentType;
using web.Areas.Admin.Requests.ContentType;

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
        if (await this.ValidateAndReturnView(validator, model)) return PartialView("_Create.Modal", model);

        try
        {
            var newContentType = _mapper.Map<ContentType>(model);

            var response = await contentTypeService.AddAsync(newContentType);

            switch (response)
            {
                case SuccessResponse<ContentType> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;

                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "ContentType", new { area = "Admin" }) });

                    return RedirectToAction("Index", "ContentType", new { area = "Admin" });
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
    public async Task<IActionResult> Edit(ContentTypeUpdateRequest model)
    {
        var validator = GetValidator<ContentTypeUpdateRequest>();
        if (await this.ValidateAndReturnView(validator, model)) return PartialView("_Edit.Modal", model);

        try
        {
            var contentType = await contentTypeService.GetByIdAsync(model.Id);
            if (contentType == null) return NotFound();

            _mapper.Map(model, contentType);

            var response = await contentTypeService.UpdateAsync(model.Id, contentType);

            switch (response)
            {
                case SuccessResponse<ContentType> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "ContentType", new { area = "Admin" }) });

                    return RedirectToAction("Index", "ContentType", new { area = "Admin" });
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
    public async Task<IActionResult> Delete(ContentTypeDeleteRequest model)
    {
        try
        {
            var response = await contentTypeService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<ContentType> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "ContentType", new { area = "Admin" }) });

                    return RedirectToAction("Index", "ContentType", new { area = "Admin" });
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