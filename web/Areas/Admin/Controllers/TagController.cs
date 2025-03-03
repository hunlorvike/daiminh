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
using web.Areas.Admin.Models.Tag;
using web.Areas.Admin.Requests.Tag;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class TagController(
    ITagService tagService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class TagController
{
    public async Task<IActionResult> Index()
    {
        List<Tag> tags = await tagService.GetAllAsync();
        List<TagViewModel> models = _mapper.Map<List<TagViewModel>>(tags);
        return View(models);
    }

    [AjaxOnly]
    public IActionResult Create()
    {
        return PartialView("_Create.Modal", new TagCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await tagService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<TagUpdateRequest>(response);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await tagService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<TagDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }
}

public partial class TagController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TagCreateRequest model)
    {
        var validator = GetValidator<TagCreateRequest>();
        if (await this.ValidateAndReturnView(validator, model))
        {
            return PartialView("_Create.Modal", model);
        }

        try
        {
            var newTag = _mapper.Map<Tag>(model);

            var response = await tagService.AddAsync(newTag);

            switch (response)
            {
                case SuccessResponse<Tag> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;

                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "Tag", new { area = "Admin" }) });

                    return RedirectToAction("Index", "Tag", new { area = "Admin" });
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
    public async Task<IActionResult> Edit(TagUpdateRequest model)
    {
        var validator = GetValidator<TagUpdateRequest>();
        if (await this.ValidateAndReturnView(validator, model)) return PartialView("_Edit.Modal", model);

        try
        {
            var contentType = await tagService.GetByIdAsync(model.Id);
            if (contentType == null) return NotFound();

            _mapper.Map(model, contentType);

            var response = await tagService.UpdateAsync(model.Id, contentType);

            switch (response)
            {
                case SuccessResponse<Tag> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "Tag", new { area = "Admin" }) });

                    return RedirectToAction("Index", "Tag", new { area = "Admin" });
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
    public async Task<IActionResult> Delete(TagDeleteRequest model)
    {
        try
        {
            var response = await tagService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<Tag> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "Tag", new { area = "Admin" }) });

                    return RedirectToAction("Index", "Tag", new { area = "Admin" });
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