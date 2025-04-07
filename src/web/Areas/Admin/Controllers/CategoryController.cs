using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Category;
using shared.Enums; // For CategoryType
using System.Linq; // For Count() and Select()

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
        ViewData["PageTitle"] = $"Quản lý Danh mục ({GetCategoryTypeDisplayName(type)})";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Danh mục", "") // Active
        };

        var query = _context.Set<Category>()
                            .Where(c => c.Type == type)
                            .Include(c => c.Parent) // Needed for ParentName display
                                                    // Include necessary collections for ItemCount
                            .Include(c => c.ProductCategories)
                            .Include(c => c.ArticleCategories)
                            .Include(c => c.ProjectCategories)
                            .Include(c => c.GalleryCategories)
                            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.Name.Contains(searchTerm) || (c.Description != null && c.Description.Contains(searchTerm)));
        }

        // Order by hierarchy (Parent first) then OrderIndex, then Name
        // This requires fetching all and sorting in memory if you want strict hierarchical display.
        // A simpler sort for DB performance:
        var categories = await query
            .OrderBy(c => c.ParentId) // Group children under parents visually (approximate)
            .ThenBy(c => c.OrderIndex)
            .ThenBy(c => c.Name)
            .ToListAsync();

        var viewModels = _mapper.Map<List<CategoryListItemViewModel>>(categories);

        // Pass data for filter dropdown
        ViewBag.CategoryTypes = Enum.GetValues(typeof(CategoryType))
            .Cast<CategoryType>()
            .Select(ct => new SelectListItem
            {
                Value = ct.ToString(),
                Text = GetCategoryTypeDisplayName(ct),
                Selected = ct == type
            }).ToList();

        ViewBag.CurrentType = type;
        ViewBag.SearchTerm = searchTerm;

        return View(viewModels);
    }

    // GET: Admin/Category/Create
    public async Task<IActionResult> Create(CategoryType type = CategoryType.Product)
    {
        ViewData["PageTitle"] = $"Thêm Danh mục mới ({GetCategoryTypeDisplayName(type)})";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Danh mục", Url.Action(nameof(Index), new { type = type })),
            ("Thêm mới", "") // Active
        };

        var viewModel = new CategoryViewModel
        {
            Type = type,
            IsActive = true,
            SitemapPriority = 0.7, // Example default
            SitemapChangeFrequency = "weekly"
        };

        // Load potential parent categories of the same type
        viewModel.ParentCategoryList = await LoadParentCategoriesAsync(type);

        return View(viewModel);
    }

    // POST: Admin/Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel viewModel)
    {
        // Manual check: Slug unique within type
        if (await _context.Set<Category>().AnyAsync(c => c.Slug == viewModel.Slug && c.Type == viewModel.Type))
        {
            ModelState.AddModelError(nameof(CategoryViewModel.Slug), "Slug đã tồn tại cho loại danh mục này.");
        }
        // Manual check: ParentId exists (if provided)
        if (viewModel.ParentId.HasValue && !await _context.Set<Category>().AnyAsync(c => c.Id == viewModel.ParentId.Value && c.Type == viewModel.Type))
        {
            ModelState.AddModelError(nameof(CategoryViewModel.ParentId), "Danh mục cha không hợp lệ.");
        }


        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty); // Add FluentValidation errors
            // Repopulate necessary data if returning view
            ViewData["PageTitle"] = $"Thêm Danh mục mới ({GetCategoryTypeDisplayName(viewModel.Type)})";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Danh mục", Url.Action(nameof(Index), new { type = viewModel.Type })), ("Thêm mới", "") };
            viewModel.ParentCategoryList = await LoadParentCategoriesAsync(viewModel.Type);
            return View(viewModel);
        }

        var category = _mapper.Map<Category>(viewModel);
        // Initialize collections if needed (if not done in entity constructor)
        category.Children = new List<Category>();
        // Initialize other collections based on type if necessary...

        _context.Add(category);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm danh mục thành công!";
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

        var viewModel = _mapper.Map<CategoryViewModel>(category);

        ViewData["PageTitle"] = $"Chỉnh sửa Danh mục ({GetCategoryTypeDisplayName(category.Type)})";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Danh mục", Url.Action(nameof(Index), new { type = category.Type })),
            ("Chỉnh sửa", "") // Active
        };

        // Load potential parent categories (same type, excluding self)
        viewModel.ParentCategoryList = await LoadParentCategoriesAsync(category.Type, id);

        return View(viewModel);
    }

    // POST: Admin/Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        // Manual check: Slug unique within type (excluding self)
        if (await _context.Set<Category>().AnyAsync(c => c.Slug == viewModel.Slug && c.Type == viewModel.Type && c.Id != id))
        {
            ModelState.AddModelError(nameof(CategoryViewModel.Slug), "Slug đã tồn tại cho loại danh mục này.");
        }
        // Manual check: ParentId exists (if provided) and is not self or a descendant (basic check: not self)
        if (viewModel.ParentId.HasValue)
        {
            if (viewModel.ParentId == id)
            {
                ModelState.AddModelError(nameof(CategoryViewModel.ParentId), "Danh mục không thể là con của chính nó.");
            }
            else if (!await _context.Set<Category>().AnyAsync(c => c.Id == viewModel.ParentId.Value && c.Type == viewModel.Type))
            {
                ModelState.AddModelError(nameof(CategoryViewModel.ParentId), "Danh mục cha không hợp lệ.");
            }
            // Add descendant check here if needed (more complex query)
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            ViewData["PageTitle"] = $"Chỉnh sửa Danh mục ({GetCategoryTypeDisplayName(viewModel.Type)})";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Danh mục", Url.Action(nameof(Index), new { type = viewModel.Type })), ("Chỉnh sửa", "") };
            viewModel.ParentCategoryList = await LoadParentCategoriesAsync(viewModel.Type, id);
            return View(viewModel);
        }

        var category = await _context.Set<Category>().FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        // Prevent changing Type after creation
        if (category.Type != viewModel.Type)
        {
            ModelState.AddModelError(nameof(CategoryViewModel.Type), "Không thể thay đổi loại danh mục sau khi tạo.");
            ViewData["PageTitle"] = $"Chỉnh sửa Danh mục ({GetCategoryTypeDisplayName(viewModel.Type)})";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Danh mục", Url.Action(nameof(Index), new { type = viewModel.Type })), ("Chỉnh sửa", "") };
            viewModel.ParentCategoryList = await LoadParentCategoriesAsync(viewModel.Type, id);
            return View(viewModel);
        }


        _mapper.Map(viewModel, category);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CategoryExists(id)) return NotFound();
            TempData["ErrorMessage"] = "Lỗi: Xung đột dữ liệu khi cập nhật.";
            viewModel.ParentCategoryList = await LoadParentCategoriesAsync(viewModel.Type, id);
            return View(viewModel);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Không thể lưu thay đổi. Vui lòng kiểm tra lại dữ liệu.");
            ViewData["PageTitle"] = $"Chỉnh sửa Danh mục ({GetCategoryTypeDisplayName(viewModel.Type)})";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Danh mục", Url.Action(nameof(Index), new { type = viewModel.Type })), ("Chỉnh sửa", "") };
            viewModel.ParentCategoryList = await LoadParentCategoriesAsync(viewModel.Type, id);
            return View(viewModel);
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
            return Json(new { success = false, message = "Không tìm thấy danh mục." });
        }

        // Check for children
        if (category.Children != null && category.Children.Any())
        {
            return Json(new { success = false, message = $"Không thể xóa danh mục '{category.Name}' vì nó có chứa danh mục con." });
        }

        // Check for associated items based on type
        bool hasItems = category.Type switch
        {
            CategoryType.Product => category.ProductCategories?.Any() ?? false,
            CategoryType.Article => category.ArticleCategories?.Any() ?? false,
            CategoryType.Project => category.ProjectCategories?.Any() ?? false,
            CategoryType.Gallery => category.GalleryCategories?.Any() ?? false,
            _ => false,
        };

        if (hasItems)
        {
            string itemTypeName = category.Type switch
            {
                CategoryType.Product => "sản phẩm",
                CategoryType.Article => "bài viết",
                CategoryType.Project => "dự án",
                CategoryType.Gallery => "thư viện ảnh",
                _ => "mục"
            };
            return Json(new { success = false, message = $"Không thể xóa danh mục '{category.Name}' vì nó đang được gán cho các {itemTypeName}." });
        }


        _context.Remove(category);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa danh mục thành công." });
    }

    private async Task<SelectList> LoadParentCategoriesAsync(CategoryType type, int? excludeId = null)
    {
        var query = _context.Set<Category>()
                           .Where(c => c.Type == type)
                           .OrderBy(c => c.Name)
                           .AsQueryable();

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
            // Add logic here to exclude descendants if needed (complex query)
        }

        var categories = await query.Select(c => new { c.Id, c.Name }).ToListAsync();

        return new SelectList(categories, "Id", "Name");
    }

    private async Task<bool> CategoryExists(int id)
    {
        return await _context.Set<Category>().AnyAsync(e => e.Id == id);
    }

    private string GetCategoryTypeDisplayName(CategoryType type)
    {
        // Consider using Display attribute on Enum or a resource file for localization
        return type switch
        {
            CategoryType.Product => "Sản phẩm",
            CategoryType.Article => "Bài viết",
            CategoryType.Project => "Dự án",
            CategoryType.Gallery => "Thư viện",
            _ => type.ToString()
        };
    }
}