using core.Common.Constants;
using core.Common.Extensions;
using Core.Common.Models;
using core.Entities;
using core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Models.ContentType;
using web.Areas.Admin.Requests.ContentType;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ContentTypeController(
    ContentTypeService contentTypeService,
    IServiceProvider serviceProvider) : Controller
{
    private readonly ContentTypeService _contentTypeService = contentTypeService;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private IValidator<T> GetValidator<T>() where T : class
        => _serviceProvider.GetRequiredService<IValidator<T>>();
}

public partial class ContentTypeController
{
    public async Task<IActionResult> Index()
    {
        List<ContentType> response = await _contentTypeService.GetAllAsync();
        List<ContentTypeViewModel> viewModels = response.Select(r => new ContentTypeViewModel
        {
            Id = r.Id,
            Name = r.Name,
            Slug = r.Slug,
            CreatedAt = r.CreatedAt,
        }).ToList();
        return View(viewModels);
    }

    public IActionResult Create()
    {
        return PartialView("_Create.Modal", new ContentTypeCreateRequest());
    }

    public async Task<IActionResult> Edit(int id)
    {
        ContentType? response = await _contentTypeService.GetByIdAsync(id);
        if (response == null) return NotFound();
        ContentTypeUpdateRequest request = new()
        {
            Id = response.Id,
            Name = response.Name,
            Slug = response.Slug,
        };
        return PartialView("_Edit.Modal", request);
    }


    public async Task<IActionResult> Delete(int id)
    {
        ContentType? response = await _contentTypeService.GetByIdAsync(id);
        if (response == null) return NotFound();
        ContentTypeDeleteRequest request = new()
        {
            Id = response.Id,
        };
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
        if (await this.ValidateAndReturnView(validator, model))
        {
            return PartialView("_Create.Modal", model);
        }

        try
        {
            ContentType newContentType = new ContentType()
            {
                Name = model.Name ?? string.Empty,
                Slug = model.Slug ?? string.Empty,
            };

            var response = await _contentTypeService.AddAsync(newContentType);

            switch (response)
            {
                case SuccessResponse<ContentType> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;

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
        if (await this.ValidateAndReturnView(validator, model))
        {
            return PartialView("_Edit.Modal", model);
        }

        try
        {
            ContentType updateContentType = new ContentType()
            {
                Name = model.Name ?? string.Empty,
                Slug = model.Slug ?? string.Empty,
            };

            var response = await _contentTypeService.UpdateAsync(model.Id, updateContentType);

            switch (response)
            {
                case SuccessResponse<ContentType> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ContentType", new { area = "Admin" });
                case ErrorResponse errorResponse:
                    foreach (var error in errorResponse.Errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }

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
            var response = await _contentTypeService.DeleteAsync(model.Id);
            Console.WriteLine(response);
            switch (response)
            {
                case SuccessResponse<ContentType> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "ContentType", new { area = "Admin" });
                case ErrorResponse errorResponse:
                    foreach (var error in errorResponse.Errors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }

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