using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Category;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CategoryViewModel> _validator;

    public CategoryController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<CategoryViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Category
    public async Task<IActionResult> Index(CategoryType type = CategoryType.Product, string? searchTerm = null)
    {
        ViewData["PageTitle"] = GetCategoryTypeTitle(type);
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Danh mục", "")
        };

        var query = _context.Set<Category>()
            .Include(c => c.Parent)
            .Include(c => c.Children)
            .Include(c => c.ProductCategories)
            .Include(c => c.ArticleCategories)
            .Include(c => c.ProjectCategories)
            .Include(c => c.GalleryCategories)
            .Where(c => c.Type == type)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm));
        }

        var categories = await query
            .OrderBy(c => c.ParentId)
            .ThenBy(c => c.OrderIndex)
            .ToListAsync();

        var viewModels = _mapper.Map<List<CategoryListItemViewModel>>(categories);

        ViewBag.SearchTerm = searchTerm;
        ViewBag.CategoryType = type;
        ViewBag.CategoryTypes = Enum.GetValues(typeof(CategoryType))
            .Cast<CategoryType>()
            .Select(t => new { Value = t, Text = GetCategoryTypeTitle(t) });

        return View(viewModels);
    }

    // GET: Admin/Category/Create
    public async Task<IActionResult> Create(CategoryType type = CategoryType.Product)
    {
        ViewData["PageTitle"] = $"Thêm {GetCategoryTypeName(type).ToLower()} mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Danh mục", "/Admin/Category"),
            ($"Thêm {GetCategoryTypeName(type).ToLower()}", "")
        };

        var viewModel = new CategoryViewModel
        {
            IsActive = true,
            OrderIndex = 0,
            Type = type
        };

        // Load available parent categories
        viewModel.AvailableParents = await GetAvailableParentCategories(type);

        ViewBag.CategoryType = type;
        ViewBag.CategoryTypeName = GetCategoryTypeName(type);

        return View(viewModel);
    }

    // POST: Admin/Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            // Reload available parent categories
            viewModel.AvailableParents = await GetAvailableParentCategories(viewModel.Type);

            ViewBag.CategoryType = viewModel.Type;
            ViewBag.CategoryTypeName = GetCategoryTypeName(viewModel.Type);

            return View(viewModel);
        }

        // Check if slug is unique for this category type
        if (await _context.Set<Category>().AnyAsync(c => c.Slug == viewModel.Slug && c.Type == viewModel.Type))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại cho loại danh mục này, vui lòng chọn slug khác");

            // Reload available parent categories
            viewModel.AvailableParents = await GetAvailableParentCategories(viewModel.Type);

            ViewBag.CategoryType = viewModel.Type;
            ViewBag.CategoryTypeName = GetCategoryTypeName(viewModel.Type);

            return View(viewModel);
        }

        var category = _mapper.Map<Category>(viewModel);

        _context.Add(category);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Thêm {GetCategoryTypeName(viewModel.Type).ToLower()} thành công";
        return RedirectToAction(nameof(Index), new { type = viewModel.Type });
    }

    // GET: Admin/Category/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _context.Set<Category>().FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = $"Chỉnh sửa {GetCategoryTypeName(category.Type).ToLower()}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Danh mục", "/Admin/Category"),
            ($"Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<CategoryViewModel>(category);

        // Load available parent categories (excluding this category and its children)
        viewModel.AvailableParents = await GetAvailableParentCategories(category.Type, category.Id);

        ViewBag.CategoryType = category.Type;
        ViewBag.CategoryTypeName = GetCategoryTypeName(category.Type);

        return View(viewModel);
    }

    // POST: Admin/Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            viewModel.AvailableParents = await GetAvailableParentCategories(viewModel.Type, viewModel.Id);
            ViewBag.CategoryType = viewModel.Type;
            ViewBag.CategoryTypeName = GetCategoryTypeName(viewModel.Type);
            return View(viewModel);
        }

        if (await _context.Set<Category>().AnyAsync(c => c.Slug == viewModel.Slug && c.Type == viewModel.Type && c.Id != id))
        {
            ModelState.AddModelError("Slug", "Slug đã tồn tại cho loại danh mục này, vui lòng chọn slug khác");
            viewModel.AvailableParents = await GetAvailableParentCategories(viewModel.Type, viewModel.Id);
            ViewBag.CategoryType = viewModel.Type;
            ViewBag.CategoryTypeName = GetCategoryTypeName(viewModel.Type);
            return View(viewModel);
        }

        try
        {
            var category = await _context.Set<Category>().FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            if (viewModel.ParentId.HasValue)
            {
                var childIds = await GetAllChildCategoryIds(id);
                if (childIds.Contains(viewModel.ParentId.Value))
                {
                    ModelState.AddModelError("ParentId", "Không thể chọn danh mục con làm danh mục cha");
                    viewModel.AvailableParents = await GetAvailableParentCategories(viewModel.Type, viewModel.Id);
                    ViewBag.CategoryType = viewModel.Type;
                    ViewBag.CategoryTypeName = GetCategoryTypeName(viewModel.Type);
                    return View(viewModel);
                }
            }

            _mapper.Map(viewModel, category);

            _context.Update(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Cập nhật {GetCategoryTypeName(viewModel.Type).ToLower()} thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CategoryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index), new { type = viewModel.Type });
    }

    // POST: Admin/Category/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Set<Category>()
            .Include(c => c.Children)
            .Include(c => c.ProductCategories)
            .Include(c => c.ArticleCategories)
            .Include(c => c.ProjectCategories)
            .Include(c => c.GalleryCategories)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return Json(new { success = false, message = "Không tìm thấy danh mục" });
        }

        // Check if category has children
        if (category.Children != null && category.Children.Any())
        {
            return Json(new { success = false, message = "Không thể xóa danh mục có chứa danh mục con. Vui lòng xóa các danh mục con trước." });
        }

        // Check if category has associated items
        bool hasItems = false;
        string itemType = "";

        switch (category.Type)
        {
            case CategoryType.Product:
                hasItems = category.ProductCategories != null && category.ProductCategories.Any();
                itemType = "sản phẩm";
                break;
            case CategoryType.Article:
                hasItems = category.ArticleCategories != null && category.ArticleCategories.Any();
                itemType = "bài viết";
                break;
            case CategoryType.Project:
                hasItems = category.ProjectCategories != null && category.ProjectCategories.Any();
                itemType = "dự án";
                break;
            case CategoryType.Gallery:
                hasItems = category.GalleryCategories != null && category.GalleryCategories.Any();
                itemType = "thư viện";
                break;
        }

        if (hasItems)
        {
            return Json(new { success = false, message = $"Không thể xóa danh mục có chứa {itemType}. Vui lòng xóa hoặc chuyển các {itemType} sang danh mục khác trước." });
        }

        _context.Set<Category>().Remove(category);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa danh mục thành công" });
    }

    // POST: Admin/Category/ToggleActive/5
    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var category = await _context.Set<Category>().FindAsync(id);

        if (category == null)
        {
            return Json(new { success = false, message = "Không tìm thấy danh mục" });
        }

        category.IsActive = !category.IsActive;
        await _context.SaveChangesAsync();

        return Json(new { success = true, active = category.IsActive });
    }

    private async Task<bool> CategoryExists(int id)
    {
        return await _context.Set<Category>().AnyAsync(e => e.Id == id);
    }

    private async Task<IEnumerable<CategorySelectViewModel>> GetAvailableParentCategories(CategoryType type, int? excludeId = null)
    {
        var query = _context.Set<Category>()
            .Where(c => c.Type == type)
            .OrderBy(c => c.ParentId)
            .ThenBy(c => c.OrderIndex)
            .AsQueryable();

        if (excludeId.HasValue)
        {
            // Exclude the category itself and all its children
            var childIds = await GetAllChildCategoryIds(excludeId.Value);
            childIds.Add(excludeId.Value);
            query = query.Where(c => !childIds.Contains(c.Id));
        }

        var categories = await query.ToListAsync();
        var result = _mapper.Map<List<CategorySelectViewModel>>(categories);

        // Set the level for each category (for indentation in dropdowns)
        var categoryDict = result.ToDictionary(c => c.Id);
        foreach (var category in result)
        {
            category.Level = CalculateCategoryLevel(category, categoryDict);
        }

        return result;
    }

    private int CalculateCategoryLevel(CategorySelectViewModel category, Dictionary<int, CategorySelectViewModel> categoryDict)
    {
        int level = 0;
        var currentCategory = category;

        while (currentCategory.ParentId.HasValue && categoryDict.ContainsKey(currentCategory.ParentId.Value))
        {
            level++;
            currentCategory = categoryDict[currentCategory.ParentId.Value];
        }

        return level;
    }

    private async Task<List<int>> GetAllChildCategoryIds(int categoryId)
    {
        var result = new List<int>();
        var directChildren = await _context.Set<Category>()
            .Where(c => c.ParentId == categoryId)
            .Select(c => c.Id)
            .ToListAsync();

        result.AddRange(directChildren);

        foreach (var childId in directChildren)
        {
            var grandChildren = await GetAllChildCategoryIds(childId);
            result.AddRange(grandChildren);
        }

        return result;
    }

    private string GetCategoryTypeTitle(CategoryType type)
    {
        return type switch
        {
            CategoryType.Product => "Danh mục sản phẩm",
            CategoryType.Article => "Danh mục bài viết",
            CategoryType.Project => "Danh mục dự án",
            CategoryType.Gallery => "Danh mục thư viện",
            _ => "Danh mục"
        };
    }

    private string GetCategoryTypeName(CategoryType type)
    {
        return type switch
        {
            CategoryType.Product => "Danh mục sản phẩm",
            CategoryType.Article => "Danh mục bài viết",
            CategoryType.Project => "Danh mục dự án",
            CategoryType.Gallery => "Danh mục thư viện",
            _ => "Danh mục"
        };
    }
}

