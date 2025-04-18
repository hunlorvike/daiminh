using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.ViewModels.Category;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<CategoryController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: Admin/Category
    public async Task<IActionResult> Index(CategoryType type = CategoryType.Product, string? searchTerm = null, int page = 1, int pageSize = 15)
    {
        ViewData["Title"] = $"Quản lý Danh mục ({type.GetDisplayName()}) - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Danh sách: {type.GetDisplayName()}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> {
            ($"Danh mục {type.GetDisplayName()}", Url.Action("Index", "Category", new { area = "Admin", type }))
        };

        int pageNumber = page;

        var query = _context.Set<Category>()
                            .Where(c => c.Type == type)
                            .Include(c => c.Parent)
                            .Include(c => c.Products)
                            .Include(c => c.Articles)
                            .Include(c => c.Projects)
                            .Include(c => c.Galleries)
                            .Include(c => c.FAQs)
                            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(lowerSearchTerm) || (c.Description != null && c.Description.ToLower().Contains(lowerSearchTerm)));
        }

        var categoriesPaged = await query
            .OrderBy(c => c.ParentId == null ? 0 : 1)
            .ThenBy(c => c.ParentId)
            .ThenBy(c => c.OrderIndex)
            .ThenBy(c => c.Name)
            .ProjectTo<CategoryListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        ViewBag.CurrentType = type;
        ViewBag.SearchTerm = searchTerm;
        ViewBag.CategoryTypes = Enum.GetValues(typeof(CategoryType))
            .Cast<CategoryType>()
            .Select(ct => new SelectListItem
            {
                Value = ct.ToString(),
                Text = ct.GetDisplayName(),
                Selected = ct.Equals(type)
            }).ToList();

        return View(categoriesPaged);
    }

    // GET: Admin/Category/Create
    public async Task<IActionResult> Create(CategoryType type = CategoryType.Product)
    {
        ViewData["Title"] = $"Thêm Danh mục ({type.GetDisplayName()}) - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Thêm Danh mục {type.GetDisplayName()} mới";

        var viewModel = new CategoryViewModel
        {
            Type = type,
            IsActive = true,
            OrderIndex = 0,
            SitemapPriority = 0.7,
            SitemapChangeFrequency = "weekly"
        };

        viewModel.ParentCategoryList = await LoadParentCategoriesAsync(type);

        return View(viewModel);
    }

    // POST: Admin/Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var category = _mapper.Map<Category>(viewModel);

                _context.Add(category);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Category '{CategoryName}' (Type: {CategoryType}) created successfully by {User}.", category.Name, category.Type, category.CreatedBy);
                TempData["success"] = $"Thêm danh mục '{category.Name}' thành công!";
                return RedirectToAction(nameof(Index), new { type = viewModel.Type });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category '{CategoryName}'.", viewModel.Name);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi thêm danh mục.");
            }
        }

        _logger.LogWarning("Failed to create category '{CategoryName}'. Model state is invalid.", viewModel.Name);
        ViewData["Title"] = $"Thêm Danh mục ({viewModel.Type.GetDisplayName()}) - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Thêm Danh mục {viewModel.Type.GetDisplayName()} mới";
        viewModel.ParentCategoryList = await LoadParentCategoriesAsync(viewModel.Type);
        return View(viewModel);
    }

    // GET: Admin/Category/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _context.Set<Category>()
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            _logger.LogWarning("Edit GET: Category with ID {CategoryId} not found.", id);
            return NotFound();
        }

        var viewModel = _mapper.Map<CategoryViewModel>(category);
        ViewData["Title"] = $"Chỉnh sửa Danh mục ({viewModel.Type.GetDisplayName()}) - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Danh mục: {category.Name}";

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
            _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var categoryToUpdate = await _context.Set<Category>().FindAsync(id);

            if (categoryToUpdate == null)
            {
                _logger.LogWarning("Edit POST: Category with ID {CategoryId} not found for update.", id);
                return NotFound();
            }

            if (categoryToUpdate.Type != viewModel.Type)
            {
                _logger.LogWarning("Attempted to change Category Type for ID {CategoryId} from {OriginalType} to {NewType}.", id, categoryToUpdate.Type, viewModel.Type);
                ModelState.AddModelError(nameof(CategoryViewModel.Type), "Không thể thay đổi loại danh mục sau khi đã tạo.");
                ViewData["Title"] = $"Chỉnh sửa Danh mục ({viewModel.Type.GetDisplayName()}) - Hệ thống quản trị";
                ViewData["PageTitle"] = $"Chỉnh sửa Danh mục: {categoryToUpdate.Name}";
                viewModel.ParentCategoryList = await LoadParentCategoriesAsync(categoryToUpdate.Type, id);
                return View(viewModel);
            }

            try
            {
                _mapper.Map(viewModel, categoryToUpdate);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Category '{CategoryName}' (ID: {CategoryId}) updated successfully by {User}.", categoryToUpdate.Name, categoryToUpdate.Id, categoryToUpdate.UpdatedBy);
                TempData["success"] = $"Cập nhật danh mục '{categoryToUpdate.Name}' thành công!";
                return RedirectToAction(nameof(Index), new { type = categoryToUpdate.Type });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category '{CategoryName}' (ID: {CategoryId}).", viewModel.Name, viewModel.Id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật danh mục.");
            }
        }

        _logger.LogWarning("Failed to update category '{CategoryName}' (ID: {CategoryId}). Model state is invalid.", viewModel.Name, viewModel.Id);
        ViewData["Title"] = $"Chỉnh sửa Danh mục ({viewModel.Type.GetDisplayName()}) - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Danh mục: {viewModel.Name}"; // Dùng tên từ viewmodel
        viewModel.ParentCategoryList = await LoadParentCategoriesAsync(viewModel.Type, id); // Load lại SelectList
        return View(viewModel);
    }


    // POST: Admin/Category/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Set<Category>()
                                    .Include(c => c.Children)
                                    .Include(c => c.Products)
                                    .Include(c => c.Articles)
                                    .Include(c => c.Projects)
                                    .Include(c => c.Galleries)
                                    .Include(c => c.FAQs)
                                    .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            _logger.LogWarning("Delete POST: Category with ID {CategoryId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy danh mục." });
        }

        if (category.Children != null && category.Children.Any())
        {
            _logger.LogWarning("Attempted to delete category '{CategoryName}' (ID: {CategoryId}) which has child categories.", category.Name, id);
            return Json(new { success = false, message = $"Không thể xóa danh mục '{category.Name}' vì có chứa danh mục con. Vui lòng xóa hoặc di chuyển các danh mục con trước." });
        }

        bool hasItems = category.Type switch
        {
            CategoryType.Product => category.Products?.Any() ?? false,
            CategoryType.Article => category.Articles?.Any() ?? false,
            CategoryType.Project => category.Projects?.Any() ?? false,
            CategoryType.Gallery => category.Galleries?.Any() ?? false,
            CategoryType.FAQ => category.FAQs?.Any() ?? false,
            _ => false,
        };

        if (hasItems)
        {
            _logger.LogWarning("Attempted to delete category '{CategoryName}' (ID: {CategoryId}) which is assigned to {ItemType}.", category.Name, id, category.Type.GetDisplayName());
            return Json(new { success = false, message = $"Không thể xóa danh mục '{category.Name}' vì đang được gán cho các {category.Type.GetDisplayName()}." });
        }

        try
        {
            string categoryName = category.Name;

            _context.Remove(category);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = $"Xóa danh mục '{categoryName}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while deleting category '{CategoryName}' (ID: {CategoryId}).", category.Name, id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa danh mục." });
        }
    }

    private async Task<SelectList> LoadParentCategoriesAsync(CategoryType type, int? excludeId = null)
    {
        var query = _context.Set<Category>()
                           .Where(c => c.Type == type)
                           .OrderBy(c => c.Name)
                           .AsNoTracking();

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        var categories = await query.Select(c => new { c.Id, c.Name }).ToListAsync();

        var items = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Không có --" } };
        items.AddRange(categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }));

        return new SelectList(items, "Value", "Text");
    }
}