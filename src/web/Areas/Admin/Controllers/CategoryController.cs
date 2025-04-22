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
using web.Areas.Admin.ViewModels.Category;
using X.PagedList;
using X.PagedList.Extensions;

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

        IQueryable<Category> query = _context.Set<Category>()
                                             .Include(c => c.Parent)
                                             .Include(c => c.Products)
                                             .Include(c => c.Articles)
                                             .Include(c => c.FAQs)
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

        query = query.OrderBy(c => c.Type)
                     .ThenBy(c => c.ParentId == null ? 0 : 1)
                     .ThenBy(c => c.ParentId)
                     .ThenBy(c => c.OrderIndex)
                     .ThenBy(c => c.Name);

        var filteredCategories = await query.ToListAsync();

        var categoryVMs = _mapper.Map<List<CategoryListItemViewModel>>(filteredCategories);
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
            Seo = new ViewModels.Shared.SeoViewModel
            {
                SitemapPriority = 0.5,
                SitemapChangeFrequency = "monthly"
            },
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
        if (ModelState.IsValid)
        {
            Category category = _mapper.Map<Category>(viewModel);

            _context.Add(category);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Category created successfully: {Name} (ID: {Id})", category.Name, category.Id);
                TempData["SuccessMessage"] = $"Đã thêm danh mục '{category.Name}' thành công.";
                return RedirectToAction(nameof(Index), new { type = (int)category.Type });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating category: {Name}", viewModel.Name);
                if (ex.InnerException?.Message.Contains("idx_categories_slug_type", StringComparison.OrdinalIgnoreCase) == true ||
                    ex.InnerException?.Message.Contains("categories.slug", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại cho loại danh mục này.");
                }
                else
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu danh mục.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating category: {Name}", viewModel.Name);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu danh mục.");
            }

            viewModel.ParentCategories = await LoadParentCategorySelectListAsync(viewModel.Type, viewModel.ParentId);
            viewModel.CategoryTypes = GetCategoryTypesSelectList(viewModel.Type);
        }

        return View(viewModel);
    }


    // GET: Admin/Category/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Category? category = await _context.Set<Category>()
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            _logger.LogWarning("Category not found for editing: ID {Id}", id);
            return NotFound();
        }

        CategoryViewModel viewModel = _mapper.Map<CategoryViewModel>(category);

        viewModel.ParentCategories = await LoadParentCategorySelectListAsync(category.Type, category.ParentId, category.Id);
        viewModel.CategoryTypes = GetCategoryTypesSelectList(category.Type);

        return View(viewModel);
    }

    // POST: Admin/Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            if (id != viewModel.Id)
            {
                _logger.LogWarning("Bad request: ID mismatch during Category edit. Path ID: {PathId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
                return BadRequest();
            }

            Category? category = await _context.Set<Category>().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                _logger.LogWarning("Category not found for editing (POST): ID {Id}", id);
                TempData["ErrorMessage"] = "Không tìm thấy danh mục để cập nhật.";
                return RedirectToAction(nameof(Index));
            }
            viewModel.Type = category.Type;

            _mapper.Map(viewModel, category);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Category updated successfully: {Name} (ID: {Id})", category.Name, category.Id);
                TempData["SuccessMessage"] = $"Đã cập nhật danh mục '{category.Name}' thành công.";
                return RedirectToAction(nameof(Index), new { type = (int)category.Type });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating category: ID {Id}, Name: {Name}", id, viewModel.Name);
                if (ex.InnerException?.Message.Contains("idx_categories_slug_type", StringComparison.OrdinalIgnoreCase) == true ||
                    ex.InnerException?.Message.Contains("categories.slug", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã được sử dụng bởi danh mục khác cùng loại.");
                }
                else
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật danh mục.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating category: ID {Id}, Name: {Name}", id, viewModel.Name);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật danh mục.");
            }

            viewModel.ParentCategories = await LoadParentCategorySelectListAsync(viewModel.Type, viewModel.ParentId, viewModel.Id);
            viewModel.CategoryTypes = GetCategoryTypesSelectList(viewModel.Type);
        }

        return View(viewModel);
    }


    // POST: Admin/Category/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Category? category = await _context.Set<Category>()
                                           .Include(c => c.Children)
                                           .Include(c => c.Products)
                                           .Include(c => c.Articles)
                                           .Include(c => c.FAQs)
                                           .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            _logger.LogWarning("Category not found for deletion: ID {Id}", id);
            return Json(new { success = false, message = "Không tìm thấy danh mục." });
        }

        if (category.Children != null && category.Children.Any())
        {
            _logger.LogWarning("Attempted to delete category '{Name}' (ID: {Id}) which has {ChildCount} child categories.", category.Name, category.Id, category.Children.Count);
            return Json(new { success = false, message = $"Không thể xóa danh mục '{category.Name}' vì nó chứa danh mục con. Vui lòng xóa hoặc di chuyển các danh mục con trước." });
        }

        int totalItems = (category.Products?.Count ?? 0) +
                         (category.Articles?.Count ?? 0) +
                         (category.FAQs?.Count ?? 0);

        if (totalItems > 0)
        {
            string itemTypeName = category.Type switch
            {
                CategoryType.Product => "sản phẩm",
                CategoryType.Article => "bài viết",
                CategoryType.FAQ => "FAQ",
                _ => "mục"
            };
            _logger.LogWarning("Attempted to delete category '{Name}' (ID: {Id}) which is used by {ItemCount} {ItemType}.", category.Name, category.Id, totalItems, itemTypeName);
            return Json(new { success = false, message = $"Không thể xóa danh mục '{category.Name}' vì đang được sử dụng bởi {totalItems} {itemTypeName}. Vui lòng gỡ danh mục khỏi các {itemTypeName} trước." });
        }


        try
        {
            string categoryName = category.Name;
            var categoryType = category.Type;
            _context.Remove(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Category deleted successfully: {Name} (ID: {Id})", categoryName, id);
            return Json(new { success = true, message = $"Xóa danh mục '{categoryName}' thành công." });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting category (DbUpdateException): ID {Id}, Name: {Name}", id, category.Name);
            return Json(new { success = false, message = "Đã xảy ra lỗi cơ sở dữ liệu khi xóa danh mục. Có thể danh mục vẫn còn được liên kết." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category: ID {Id}, Name: {Name}", id, category.Name);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa danh mục." });
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
                          .Where(c => c.Type == categoryType && c.IsActive)
                          .OrderBy(c => c.OrderIndex)
                          .ThenBy(c => c.Name)
                          .AsNoTracking();

        List<int> idsToExclude = new List<int>();
        if (excludeCategoryId.HasValue && excludeCategoryId.Value > 0)
        {
            idsToExclude.Add(excludeCategoryId.Value);
            idsToExclude.AddRange(await GetDescendantIdsAsync(excludeCategoryId.Value));
        }

        if (idsToExclude.Any())
        {
            query = query.Where(c => !idsToExclude.Contains(c.Id));
        }


        var categories = await query.Select(c => new { c.Id, c.Name }).ToListAsync();

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

    private async Task<List<int>> GetDescendantIdsAsync(int categoryId)
    {
        var descendantIds = new List<int>();
        var children = await _context.Set<Category>()
                                     .Where(c => c.ParentId == categoryId)
                                     .Select(c => c.Id)
                                     .ToListAsync();
        descendantIds.AddRange(children);
        foreach (var childId in children)
        {
            descendantIds.AddRange(await GetDescendantIdsAsync(childId));
        }
        return descendantIds;
    }


    private List<SelectListItem> GetCategoryTypesSelectList(CategoryType? selectedType)
    {
        var items = Enum.GetValues(typeof(CategoryType))
            .Cast<CategoryType>()
            .Select(t => new SelectListItem
            {
                Value = ((int)t).ToString(),
                Text = t.GetDisplayName(),
                Selected = selectedType.HasValue && t == selectedType.Value
            })
            .OrderBy(t => t.Text)
            .ToList();
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