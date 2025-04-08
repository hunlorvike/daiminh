using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.FAQ;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class FAQController : Controller
{
    private readonly ApplicationDbContext? _context;
    private readonly IMapper? _mapper;
    private readonly IValidator<FAQViewModel>? _validator;
    private readonly ILogger<FAQController>? _logger;

    public FAQController(
       ApplicationDbContext context,
       IMapper mapper,
       IValidator<FAQViewModel> validator,
       ILogger<FAQController> logger)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    // GET: Admin/FAQ
    public async Task<IActionResult> Index(string? searchTerm = null, int? categoryId = null)
    {
        ViewData["PageTitle"] = "Quản lý Hỏi Đáp (FAQ)";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", "") };

        var query = _context.Set<FAQ>()
                            .Include(f => f.FAQCategories)
                            .ThenInclude(fc => fc.Category)
                            .AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(f => f.Question.Contains(searchTerm) || f.Answer.Contains(searchTerm));
        }
        if (categoryId.HasValue)
        {
            query = query.Where(f => f.FAQCategories.Any(fc => fc.CategoryId == categoryId.Value));
        }

        var faqs = await query.OrderBy(f => f.OrderIndex).ThenBy(f => f.Question).ToListAsync();
        var viewModels = _mapper.Map<List<FAQListItemViewModel>>(faqs);

        // Load categories for filter dropdown
        ViewBag.Categories = await _context.Set<Category>()
                                     .Where(c => c.Type == CategoryType.FAQ && c.IsActive)
                                     .OrderBy(c => c.Name)
                                     .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                                     .ToListAsync();

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedCategoryId = categoryId;

        return View(viewModels);
    }

    // GET: Admin/FAQ/Create
    public async Task<IActionResult> Create()
    {
        ViewData["PageTitle"] = "Thêm FAQ mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ("Thêm mới", "") };

        var viewModel = new FAQViewModel { IsActive = true };
        viewModel.CategoryList = await LoadCategoriesAsync(); // Load categories for multi-select

        return View(viewModel);
    }

    // POST: Admin/FAQ/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FAQViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            _logger.LogWarning("FAQ creation validation failed.");
            viewModel.CategoryList = await LoadCategoriesAsync(); // Reload dropdown
            ViewData["PageTitle"] = "Thêm FAQ mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ("Thêm mới", "") };
            return View(viewModel);
        }

        var faq = _mapper.Map<FAQ>(viewModel);

        // Manually add FAQCategory associations
        faq.FAQCategories = viewModel.SelectedCategoryIds
                                .Select(catId => new FAQCategory { CategoryId = catId })
                                .ToList();

        _context.Add(faq);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm FAQ thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error saving new FAQ.");
            ModelState.AddModelError("", "Không thể lưu FAQ. Vui lòng kiểm tra lại dữ liệu.");
            viewModel.CategoryList = await LoadCategoriesAsync(); // Reload dropdown
            ViewData["PageTitle"] = "Thêm FAQ mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ("Thêm mới", "") };
            return View(viewModel);
        }
    }

    // GET: Admin/FAQ/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var faq = await _context.Set<FAQ>()
                              .Include(f => f.FAQCategories) // Include join table
                              .FirstOrDefaultAsync(f => f.Id == id);

        if (faq == null) return NotFound();

        var viewModel = _mapper.Map<FAQViewModel>(faq);
        viewModel.CategoryList = await LoadCategoriesAsync(); // Load categories

        ViewData["PageTitle"] = "Chỉnh sửa FAQ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };

        return View(viewModel);
    }

    // POST: Admin/FAQ/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FAQViewModel viewModel)
    {
        if (id != viewModel.Id) return BadRequest();

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            _logger.LogWarning("FAQ editing validation failed for ID: {FaqId}", id);
            viewModel.CategoryList = await LoadCategoriesAsync(); // Reload dropdown
            ViewData["PageTitle"] = "Chỉnh sửa FAQ";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };
            return View(viewModel);
        }

        var faq = await _context.Set<FAQ>()
                              .Include(f => f.FAQCategories) // Include join table for update
                              .FirstOrDefaultAsync(f => f.Id == id);

        if (faq == null) return NotFound();

        // Map scalar properties
        _mapper.Map(viewModel, faq);

        // --- Update Many-to-Many (FAQCategory) ---
        var currentCategoryIds = faq.FAQCategories.Select(fc => fc.CategoryId).ToList();
        var categoriesToAdd = viewModel.SelectedCategoryIds
                                    .Except(currentCategoryIds)
                                    .Select(catId => new FAQCategory { FAQId = id, CategoryId = catId });
        var categoriesToRemove = faq.FAQCategories
                                    .Where(fc => !viewModel.SelectedCategoryIds.Contains(fc.CategoryId))
                                    .ToList();

        _context.FAQCategories.RemoveRange(categoriesToRemove);
        await _context.FAQCategories.AddRangeAsync(categoriesToAdd);
        // --- End Many-to-Many Update ---

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật FAQ thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await FAQExists(id)) return NotFound();
            _logger.LogWarning("Concurrency conflict updating FAQ ID: {FaqId}", id);
            TempData["ErrorMessage"] = "Lỗi: Xung đột dữ liệu khi cập nhật.";
            viewModel.CategoryList = await LoadCategoriesAsync();
            return View(viewModel);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating FAQ ID: {FaqId}", id);
            ModelState.AddModelError("", "Không thể lưu FAQ. Vui lòng kiểm tra lại dữ liệu.");
            viewModel.CategoryList = await LoadCategoriesAsync();
            return View(viewModel);
        }
    }

    // POST: Admin/FAQ/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var faq = await _context.Set<FAQ>().FindAsync(id);
        if (faq == null)
        {
            return Json(new { success = false, message = "Không tìm thấy FAQ." });
        }

        _context.Remove(faq); // Cascade delete handles FAQCategory

        try
        {
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Xóa FAQ thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting FAQ ID: {FaqId}", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa FAQ." });
        }
    }

    // --- Helper Methods ---
    private async Task<SelectList> LoadCategoriesAsync()
    {
        var categories = await _context.Set<Category>()
                                     .Where(c => c.Type == CategoryType.FAQ && c.IsActive)
                                     .OrderBy(c => c.Name)
                                     .Select(c => new { c.Id, c.Name })
                                     .ToListAsync();
        return new SelectList(categories, "Id", "Name");
    }

    private async Task<bool> FAQExists(int id)
    {
        return await _context.Set<FAQ>().AnyAsync(e => e.Id == id);
    }
}
