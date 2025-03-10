using application.Interfaces;
using AutoMapper;
using domain.Constants;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Category;
using web.Areas.Admin.Requests.Category;
using web.Attributes;

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
        var category = await categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        var categoryList = (await categoryService.GetAllAsync())
            .Where(c => c.Id != id)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToList();

        categoryList.Insert(0, new SelectListItem { Value = "", Text = "-- Chọn danh mục cha --" });
        ViewBag.CategoryList = categoryList;

        var request = _mapper.Map<CategoryUpdateRequest>(category);
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
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var newCategory = _mapper.Map<Category>(model);

            var response = await categoryService.AddAsync(newCategory);

            switch (response)
            {
                case SuccessResponse<Category> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Category", new { area = "Admin" })
                    });
                case SuccessResponse<Category> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Category", new { area = "Admin" });
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
    public async Task<IActionResult> Edit(CategoryUpdateRequest model)
    {
        var validator = GetValidator<CategoryUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var category = await categoryService.GetByIdAsync(model.Id);
            if (category == null) return NotFound();

            _mapper.Map(model, category);

            var response = await categoryService.UpdateAsync(model.Id, category);

            switch (response)
            {
                case SuccessResponse<Category> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Category", new { area = "Admin" })
                    });
                case SuccessResponse<Category> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Category", new { area = "Admin" });
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
    public async Task<IActionResult> Delete(CategoryDeleteRequest model)
    {
        try
        {
            var response = await categoryService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<Category> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Category", new { area = "Admin" })
                    });
                case SuccessResponse<Category> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Category", new { area = "Admin" });
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