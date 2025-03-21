using AutoMapper;

using domain.Entities;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;

using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.Category;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class CategoryController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache)
{
    public async Task<IActionResult> Index()
    {
        var categories = await dbContext.Categories
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Include(x => x.ParentCategory)
            .ToListAsync();

        return View(categories);
    }

    [AjaxOnly]
    public async Task<IActionResult> Create()
    {
        await PopulateCategoryDropdown();
        return PartialView("_Create.Modal", new CategoryCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (category == null) return NotFound();

        var categoryList = await dbContext.Categories
            .AsNoTracking()
            .Where(c => c.Id != id && c.DeletedAt == null)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();

        categoryList.Insert(0, new SelectListItem { Value = "", Text = "-- Chọn danh mục cha --" });
        ViewBag.CategoryList = categoryList;

        var request = _mapper.Map<CategoryUpdateRequest>(category);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (category == null) return NotFound();
        var request = _mapper.Map<CategoryDeleteRequest>(category);
        return PartialView("_Delete.Modal", request);
    }

    private async Task PopulateCategoryDropdown()
    {
        var categories = await dbContext.Categories
            .AsNoTracking()
            .Where(c => c.DeletedAt == null)
            .ToListAsync();

        ViewBag.CategoryList = new List<SelectListItem>
        {
            new() { Value = "", Text = "-- Chọn danh mục cha --" }
        }.Concat(categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        })).ToList();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateRequest model)
    {
        var validator = GetValidator<CategoryCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateCategoryDropdown();
            return result;
        }

        try
        {
            var newCategory = _mapper.Map<Category>(model);
            await dbContext.Categories.AddAsync(newCategory);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Category>(newCategory, "Thêm danh mục mới thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Category", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Category", new { area = "Admin" });
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
        if (result != null)
        {
            await PopulateCategoryDropdownForEdit(model.Id);
            return result;
        }

        try
        {
            var category = await dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.DeletedAt == null);

            if (category == null) return NotFound();

            _mapper.Map(model, category);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Category>(category, "Cập nhật danh mục thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Category", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Category", new { area = "Admin" });
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
            var category = await dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.DeletedAt == null);

            if (category == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Danh mục không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Category>(category, "Xóa danh mục thành công (đã ẩn).");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Category", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Category", new { area = "Admin" });
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

    private async Task PopulateCategoryDropdownForEdit(int currentCategoryId)
    {
        var categoryList = await dbContext.Categories
            .AsNoTracking()
            .Where(c => c.Id != currentCategoryId && c.DeletedAt == null)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();

        categoryList.Insert(0, new SelectListItem { Value = "", Text = "-- Chọn danh mục cha --" });
        ViewBag.CategoryList = categoryList;
    }

    private async Task<bool> IsDescendant(int parentId, int childId)
    {
        var currentId = childId;
        while (currentId != 0)
        {
            var category = await dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == currentId && c.DeletedAt == null);
            if (category == null) return false;
            if (category.ParentCategoryId == parentId) return true;
            currentId = category.ParentCategoryId ?? 0;
        }
        return false;
    }
}