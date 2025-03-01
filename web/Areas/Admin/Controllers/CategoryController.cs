using AutoMapper;
using core.Attributes;
using core.Common.Constants;
using core.Common.Extensions;
using Core.Common.Models;
using core.Entities;
using core.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Category;
using web.Areas.Admin.Requests.Category;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class CategoryController(
    ICategoryService categoryService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class CategoryController
{
    public async Task<IActionResult> Index()
    {
        var categories = await categoryService.GetAllAsync();
        List<CategoryViewModel> models = _mapper.Map<List<CategoryViewModel>>(categories);
        return View(models);
    }

    [AjaxOnly]
    public async Task<IActionResult> Create()
    {
        var categories = await categoryService.GetAllAsync();
        ViewBag.CategoryList = new List<SelectListItem>
        {
            new() { Value = "", Text = "-- Chọn danh mục cha --" }
        }.Concat(categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        })).ToList();

        return PartialView("_Create.Modal", new CategoryCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await categoryService.GetByIdAsync(id);
        if (response == null)
            return NotFound();

        // Fetch all categories to populate the dropdown list
        var categories = await categoryService.GetAllAsync();
        ViewBag.CategoryList = new List<SelectListItem>
        {
            new() { Value = "", Text = "-- Chọn danh mục cha --" }
        }.Concat(categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        })).ToList();

        // Map the retrieved category to a CategoryUpdateRequest
        var request = _mapper.Map<CategoryUpdateRequest>(response);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await categoryService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<CategoryDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }
}

public partial class CategoryController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateRequest model)
    {
        var validator = GetValidator<CategoryCreateRequest>();
        if (await this.ValidateAndReturnView(validator, model)) return PartialView("_Create.Modal", model);

        try
        {
            var newCategory = _mapper.Map<Category>(model);

            var response = await categoryService.AddAsync(newCategory);

            switch (response)
            {
                case SuccessResponse<Category> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;

                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "Category", new { area = "Admin" }) });

                    return RedirectToAction("Index", "Category", new { area = "Admin" });
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
    public async Task<IActionResult> Edit(CategoryUpdateRequest model)
    {
        var validator = GetValidator<CategoryUpdateRequest>();
        if (await this.ValidateAndReturnView(validator, model)) return PartialView("_Edit.Modal", model);

        try
        {
            var category = await categoryService.GetByIdAsync(model.Id);
            if (category == null) return NotFound();

            _mapper.Map(model, category);

            var response = await categoryService.UpdateAsync(model.Id, category);

            switch (response)
            {
                case SuccessResponse<Category> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "Category", new { area = "Admin" }) });

                    return RedirectToAction("Index", "Category", new { area = "Admin" });
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
    public async Task<IActionResult> Delete(CategoryDeleteRequest model)
    {
        try
        {
            var response = await categoryService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<Category> successResponse:
                    ViewData["SuccessMessage"] = successResponse.Message;
                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "Category", new { area = "Admin" }) });

                    return RedirectToAction("Index", "Category", new { area = "Admin" });
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