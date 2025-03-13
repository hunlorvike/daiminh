using application.Interfaces;
using AutoMapper;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Content;
using web.Areas.Admin.Requests.Content;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}", AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ContentController(
    IContentService contentService,
    IContentTypeService contentTypeService,
    IUserService userService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration)
{
    public async Task<IActionResult> Index()
    {
        var contents = await contentService.GetAllAsync();
        List<ContentViewModel> models = _mapper.Map<List<ContentViewModel>>(contents);
        return View(models);
    }

    [AjaxOnly]
    public async Task<IActionResult> Create()
    {
        var contentTypes = await contentTypeService.GetAllAsync();
        ViewBag.ContentTypes = new SelectList(contentTypes, "Id", "Name");

        var authors = await userService.GetAllAsync();
        ViewBag.Authors = new SelectList(authors, "Id", "Username");

        return PartialView("_Create.Modal", new ContentCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await contentService.GetByIdAsync(id);
        if (response == null) return NotFound();

        var contentTypes = await contentTypeService.GetAllAsync();
        ViewBag.ContentTypes = new SelectList(contentTypes, "Id", "Name", response.ContentTypeId);

        var authors = await userService.GetAllAsync();
        ViewBag.Authors = new SelectList(authors, "Id", "Username", response.AuthorId);
        var request = _mapper.Map<ContentUpdateRequest>(response);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await contentService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<ContentDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContentCreateRequest model)
    {
        var validator = GetValidator<ContentCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var newContent = _mapper.Map<Content>(model);
            var response = await contentService.AddAsync(newContent);

            switch (response)
            {
                case SuccessResponse<Content> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Content", new { area = "Admin" })
                    });
                case SuccessResponse<Content> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Content", new { area = "Admin" });
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
            return BadRequest(new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            }));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ContentUpdateRequest model)
    {
        var validator = GetValidator<ContentUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;
        try
        {
            var content = await contentService.GetByIdAsync(model.Id);
            if (content == null) return NotFound();

            _mapper.Map(model, content);

            var response = await contentService.UpdateAsync(model.Id, content);

            switch (response)
            {
                case SuccessResponse<Content> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Content", new { area = "Admin" })
                    });
                case SuccessResponse<Content> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Content", new { area = "Admin" });
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
            return BadRequest(new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            }));
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ContentDeleteRequest model)
    {

        try
        {
            var response = await contentService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<Content> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Content", new { area = "Admin" })
                    });
                case SuccessResponse<Content> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Content", new { area = "Admin" });
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
            return BadRequest(new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            }));
        }
    }
}