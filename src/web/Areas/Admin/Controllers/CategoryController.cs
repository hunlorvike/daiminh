using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = "AdminAccess")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;
    private readonly IValidator<CategoryViewModel> _categoryViewModelValidator;

    public CategoryController(
        ICategoryService categoryService,
        ILogger<CategoryController> logger,
        IValidator<CategoryViewModel> categoryViewModelValidator)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _categoryViewModelValidator = categoryViewModelValidator ?? throw new ArgumentNullException(nameof(categoryViewModelValidator));
    }

    // GET: Admin/Category
    public async Task<IActionResult> Index(CategoryFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new CategoryFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IPagedList<CategoryListItemViewModel> categoriesPaged = await _categoryService.GetPagedCategoriesAsync(filter, pageNumber, currentPageSize);

        filter.TypeOptions = GetCategoryTypesSelectList(filter.Type);
        filter.StatusOptions = GetStatusSelectList(filter.IsActive);

        CategoryIndexViewModel viewModel = new()
        {
            Categories = categoriesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Category/Create
    public async Task<IActionResult> Create(CategoryType type = CategoryType.Product)
    {
        CategoryViewModel viewModel = new()
        {
            IsActive = true,
            Type = type,
            OrderIndex = 0,
            ItemCount = 0,
            HasChildren = false,
            ParentCategories = await _categoryService.GetParentCategorySelectListAsync(type),
            CategoryTypes = GetCategoryTypesSelectList(type)
        };
        return View(viewModel);
    }

    // POST: Admin/Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel viewModel)
    {
        var result = await _categoryViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.ParentCategories = await _categoryService.GetParentCategorySelectListAsync(viewModel.Type);
            viewModel.CategoryTypes = GetCategoryTypesSelectList(viewModel.Type);
            return View(viewModel);
        }

        var createResult = await _categoryService.CreateCategoryAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm danh mục thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { type = (int)viewModel.Type });
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                if (error.Contains("Slug", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), error);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            if (!createResult.Errors.Any() && !string.IsNullOrEmpty(createResult.Message))
            {
                ModelState.AddModelError(string.Empty, createResult.Message);
            }


            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm danh mục '{viewModel.Name}'.", ToastType.Error)
            );

            viewModel.ParentCategories = await _categoryService.GetParentCategorySelectListAsync(viewModel.Type);
            viewModel.CategoryTypes = GetCategoryTypesSelectList(viewModel.Type);
            return View(viewModel);
        }
    }

    // GET: Admin/Category/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        CategoryViewModel? viewModel = await _categoryService.GetCategoryByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Category not found for editing: ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy danh mục để chỉnh sửa.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }

        viewModel.ParentCategories = await _categoryService.GetParentCategorySelectListAsync(viewModel.Type, viewModel.ParentId, viewModel.Id);
        viewModel.CategoryTypes = GetCategoryTypesSelectList(viewModel.Type);


        return View(viewModel);
    }

    // POST: Admin/Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _categoryViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.ParentCategories = await _categoryService.GetParentCategorySelectListAsync(viewModel.Type, viewModel.ParentId, viewModel.Id);
            viewModel.CategoryTypes = GetCategoryTypesSelectList(viewModel.Type);
            return View(viewModel);
        }

        var updateResult = await _categoryService.UpdateCategoryAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật danh mục thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { type = (int)viewModel.Type });
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                if (error.Contains("Slug", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), error);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật danh mục '{viewModel.Name}'.", ToastType.Error)
            );

            viewModel.ParentCategories = await _categoryService.GetParentCategorySelectListAsync(viewModel.Type, viewModel.ParentId, viewModel.Id);
            viewModel.CategoryTypes = GetCategoryTypesSelectList(viewModel.Type);
            return View(viewModel);
        }
    }

    // POST: Admin/Category/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _categoryService.DeleteCategoryAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa danh mục thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa danh mục.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Admin/Category/GetParentCategories
    [HttpGet]
    public async Task<IActionResult> GetParentCategories(CategoryType type)
    {
        try
        {
            var parentCategories = await _categoryService.GetParentCategorySelectListAsync(type);
            var result = parentCategories.Skip(1).Select(c => new { value = c.Value, text = c.Text }).ToList();
            return Json(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading parent categories for type {Type}", type);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi khi tải danh mục cha.", ToastType.Error)
            );
            return StatusCode(500, "Đã xảy ra lỗi khi tải danh mục cha");
        }
    }

    private List<SelectListItem> GetCategoryTypesSelectList(CategoryType? selectedType)
    {
        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Tất cả các loại --", Selected = !selectedType.HasValue }
        };

        items.AddRange(Enum.GetValues(typeof(CategoryType))
            .Cast<CategoryType>()
            .Select(t => new SelectListItem
            {
                Value = ((int)t).ToString(),
                Text = t.GetDisplayName(),
                Selected = selectedType.HasValue && t == selectedType.Value
            })
            .OrderBy(t => t.Text));

        return items;
    }

    private List<SelectListItem> GetStatusSelectList(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Đang kích hoạt", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Đã hủy kích hoạt", Selected = selectedValue == false }
        };
    }
}

