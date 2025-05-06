using AutoMapper;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.Validators.Category;
using web.Areas.Admin.ViewModels.Category;
using X.PagedList;
using X.PagedList.Extensions;
using System.Text.Json;
using shared.Models;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<CategoryController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Category
    public async Task<IActionResult> Index(CategoryFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new CategoryFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        // Optimized query with eager loading only what's needed
        IQueryable<Category> query = _context.Set<Category>()
                                             .Include(c => c.Parent)
                                             .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(lowerSearchTerm) ||
                                     (c.Description != null && c.Description.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(c => c.Type == filter.Type.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == filter.IsActive.Value);
        }

        // Get the categories with their item counts in a single query
        var categoriesWithCounts = await query
            .Select(c => new
            {
                Category = c,
                ItemCount = c.Type == CategoryType.Product ? c.Products.Count :
                            c.Type == CategoryType.Article ? c.Articles.Count :
                            c.Type == CategoryType.FAQ ? c.FAQs.Count : 0,
                HasChildren = c.Children.Any()
            })
            .OrderBy(c => c.Category.Type)
            .ThenBy(c => c.Category.ParentId == null ? 0 : 1)
            .ThenBy(c => c.Category.ParentId)
            .ThenBy(c => c.Category.OrderIndex)
            .ThenBy(c => c.Category.Name)
            .ToListAsync();

        // Map to view models
        var categoryVMs = categoriesWithCounts.Select(c =>
        {
            var vm = _mapper.Map<CategoryListItemViewModel>(c.Category);
            vm.ItemCount = c.ItemCount;
            vm.HasChildren = c.HasChildren;
            return vm;
        }).ToList();

        CalculateHierarchyLevels(categoryVMs);

        IPagedList<CategoryListItemViewModel> categoriesPaged = categoryVMs.ToPagedList(pageNumber, currentPageSize);

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
            ParentCategories = await LoadParentCategorySelectListAsync(type),
            CategoryTypes = GetCategoryTypesSelectList(type)
        };
        return View(viewModel);
    }

    // POST: Admin/Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel viewModel)
    {
        var result = await new CategoryViewModelValidator(_context).ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await RefillCategorySelectListsAsync(viewModel);
            return View(viewModel);
        }

        var category = _mapper.Map<Category>(viewModel);
        _context.Add(category);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Thêm danh mục '{category.Name}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { type = (int)category.Type });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo danh mục: {Name}", viewModel.Name);
            if (ex.InnerException?.Message?.Contains("slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại cho loại danh mục này.");
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu danh mục.");
            }
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể thêm danh mục '{viewModel.Name}'.", ToastType.Error)
        );
        await RefillCategorySelectListsAsync(viewModel);
        return View(viewModel);
    }

    // GET: Admin/Category/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        // Optimized query to get category with item count in a single query
        var categoryData = await _context.Set<Category>()
            .Where(c => c.Id == id)
            .Select(c => new
            {
                Category = c,
                ItemCount = c.Type == CategoryType.Product ? c.Products.Count :
                            c.Type == CategoryType.Article ? c.Articles.Count :
                            c.Type == CategoryType.FAQ ? c.FAQs.Count : 0,
                HasChildren = c.Children.Any()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (categoryData == null)
        {
            _logger.LogWarning("Category not found for editing: ID {Id}", id);
            return NotFound();
        }

        CategoryViewModel viewModel = _mapper.Map<CategoryViewModel>(categoryData.Category);
        viewModel.ItemCount = categoryData.ItemCount;
        viewModel.HasChildren = categoryData.HasChildren;

        viewModel.ParentCategories = await LoadParentCategorySelectListAsync(categoryData.Category.Type, categoryData.Category.ParentId, categoryData.Category.Id);
        viewModel.CategoryTypes = GetCategoryTypesSelectList(categoryData.Category.Type);

        return View(viewModel);
    }

    // POST: Admin/Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await new CategoryViewModelValidator(_context).ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await RefillCategorySelectListsAsync(viewModel);
            return View(viewModel);
        }

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy danh mục để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, category);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Cập nhật danh mục '{category.Name}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { type = (int)category.Type });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật danh mục: ID = {Id}, Name = {Name}", id, viewModel.Name);
            if (ex.InnerException?.Message?.Contains("slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                ModelState.AddModelError(nameof(viewModel.Slug), "Slug đã được sử dụng.");
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật danh mục.");
            }
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể cập nhật danh mục '{viewModel.Name}'.", ToastType.Error)
        );
        await RefillCategorySelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/Category/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var categoryData = await _context.Set<Category>()
            .Where(c => c.Id == id)
            .Select(c => new
            {
                Category = c,
                HasChildren = c.Children.Any(),
                ItemCount = c.Type == CategoryType.Product ? c.Products.Count :
                            c.Type == CategoryType.Article ? c.Articles.Count :
                            c.Type == CategoryType.FAQ ? c.FAQs.Count : 0,
            })
            .FirstOrDefaultAsync();

        if (categoryData == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy danh mục.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy danh mục." });
        }

        if (categoryData.HasChildren)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", $"Không thể xóa danh mục '{categoryData.Category.Name}' vì nó chứa danh mục con.", ToastType.Error)
            );
            return Json(new { success = false, message = $"Không thể xóa danh mục '{categoryData.Category.Name}' vì nó chứa danh mục con." });
        }

        if (categoryData.ItemCount > 0)
        {
            string itemTypeName = categoryData.Category.Type switch
            {
                CategoryType.Product => "sản phẩm",
                CategoryType.Article => "bài viết",
                CategoryType.FAQ => "FAQ",
                _ => "mục"
            };
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", $"Không thể xóa danh mục '{categoryData.Category.Name}' vì đang được sử dụng bởi {categoryData.ItemCount} {itemTypeName}.", ToastType.Error)
            );
            return Json(new { success = false, message = $"Không thể xóa danh mục '{categoryData.Category.Name}' vì đang được sử dụng bởi {categoryData.ItemCount} {itemTypeName}." });
        }

        try
        {
            string categoryName = categoryData.Category.Name;
            _context.Remove(categoryData.Category);
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa danh mục '{categoryName}' thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = $"Xóa danh mục '{categoryName}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa danh mục: ID {Id}, Name: {Name}", id, categoryData.Category.Name);
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi không mong muốn khi xóa danh mục.", ToastType.Error)
            );
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa danh mục." });
        }
    }
}

public partial class CategoryController
{
    // GET: Admin/Category/GetParentCategories
    [HttpGet]
    public async Task<IActionResult> GetParentCategories(CategoryType type)
    {
        try
        {
            var parentCategories = await LoadParentCategorySelectListAsync(type);
            var result = parentCategories.Skip(1).Select(c => new { value = c.Value, text = c.Text }).ToList();
            return Json(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading parent categories for type {Type}", type);
            return StatusCode(500, "Đã xảy ra lỗi khi tải danh mục cha");
        }
    }
}

public partial class CategoryController
{
    private void CalculateHierarchyLevels(List<CategoryListItemViewModel> categories)
    {
        var categoryDict = categories.ToDictionary(c => c.Id);
        foreach (var category in categories)
        {
            int level = 0;
            int? currentParentId = category.ParentId;
            while (currentParentId.HasValue && categoryDict.ContainsKey(currentParentId.Value))
            {
                level++;
                currentParentId = categoryDict[currentParentId.Value].ParentId;
                if (level > 10) break;
            }
            category.Level = level;
        }
    }

    private async Task<List<SelectListItem>> LoadParentCategorySelectListAsync(CategoryType categoryType, int? selectedValue = null, int? excludeCategoryId = null)
    {
        var query = _context.Set<Category>()
                          .Where(c => c.Type == categoryType)
                          .AsNoTracking();

        if (excludeCategoryId.HasValue && excludeCategoryId.Value > 0)
        {
            var allCategories = await query.ToListAsync();
            var idsToExclude = new HashSet<int> { excludeCategoryId.Value };

            var categoryDict = allCategories.ToDictionary(c => c.Id);

            bool foundNew;
            do
            {
                foundNew = false;
                foreach (var category in allCategories)
                {
                    if (category.ParentId.HasValue &&
                        idsToExclude.Contains(category.ParentId.Value) &&
                        !idsToExclude.Contains(category.Id))
                    {
                        idsToExclude.Add(category.Id);
                        foundNew = true;
                    }
                }
            } while (foundNew);

            query = query.Where(c => !idsToExclude.Contains(c.Id));
        }

        var categories = await query
            .Where(c => c.IsActive)
            .OrderBy(c => c.OrderIndex)
            .ThenBy(c => c.Name)
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn danh mục cha (để trống nếu là gốc) --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name,
            Selected = selectedValue.HasValue && c.Id == selectedValue.Value
        }));

        return items;
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

    private async Task RefillCategorySelectListsAsync(CategoryViewModel viewModel)
    {
        viewModel.ParentCategories = await LoadParentCategorySelectListAsync(viewModel.Type, viewModel.ParentId, viewModel.Id);
        viewModel.CategoryTypes = GetCategoryTypesSelectList(viewModel.Type);
    }
}