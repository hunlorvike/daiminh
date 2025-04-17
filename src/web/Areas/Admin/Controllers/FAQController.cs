using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.FAQ;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class FAQController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<FAQController> _logger;

    public FAQController(
       ApplicationDbContext context,
       IMapper mapper,
       ILogger<FAQController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/FAQ
    public async Task<IActionResult> Index(FAQFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        ViewData["Title"] = "Quản lý Hỏi Đáp (FAQ) - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách FAQ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))) };

        filter ??= new FAQFilterViewModel();

        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        var query = _context.Set<FAQ>()
                            .Include(fc => fc.Category)
                            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(f => f.Question.ToLower().Contains(lowerSearchTerm)
                                  || f.Answer.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.CategoryId.HasValue && filter.CategoryId > 0)
        {
            query = query.Where(f => f.CategoryId == filter.CategoryId.Value);
        }

        var faqsPaged = await query
            .OrderBy(f => f.OrderIndex)
            .ThenBy(f => f.Question)
            .ProjectTo<FAQListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.Categories = await LoadCategoriesSelectListAsync(filter.CategoryId);

        var viewModel = new FAQIndexViewModel
        {
            FAQs = faqsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/FAQ/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Thêm FAQ mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm FAQ mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ("Thêm mới", "") };

        var viewModel = new FAQViewModel
        {
            IsActive = true,
            OrderIndex = 0,
            CategoryList = await LoadCategoriesSelectListAsync()
        };

        return View(viewModel);
    }

    // POST: Admin/FAQ/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FAQViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("FAQ creation failed for '{Question}'. Model state is invalid.", viewModel.Question);
            viewModel.CategoryList = await LoadCategoriesSelectListAsync(viewModel.CategoryId);
            ViewData["Title"] = "Thêm FAQ mới - Hệ thống quản trị";
            ViewData["PageTitle"] = "Thêm FAQ mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ("Thêm mới", "") };
            return View(viewModel);
        }

        var faq = _mapper.Map<FAQ>(viewModel);
        _context.Add(faq);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("FAQ '{Question}' created successfully by {User}.", faq.Question, User.Identity?.Name ?? "Unknown");
            TempData["success"] = $"Thêm FAQ '{Truncate(faq.Question)}' thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error saving new FAQ '{Question}'.", viewModel.Question);
            ModelState.AddModelError("", "Không thể lưu FAQ. Lỗi cơ sở dữ liệu.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating FAQ '{Question}'.", viewModel.Question);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi thêm FAQ.");
        }

        // If save fails, repopulate and return view
        viewModel.CategoryList = await LoadCategoriesSelectListAsync(viewModel.CategoryId);
        ViewData["Title"] = "Thêm FAQ mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm FAQ mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ("Thêm mới", "") };
        return View(viewModel);
    }

    // GET: Admin/FAQ/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var faq = await _context.Set<FAQ>()
                              .AsNoTracking()
                              .FirstOrDefaultAsync(f => f.Id == id);

        if (faq == null)
        {
            _logger.LogWarning("Edit GET: FAQ with ID {FaqId} not found.", id);
            return NotFound();
        }

        var viewModel = _mapper.Map<FAQViewModel>(faq);
        viewModel.CategoryList = await LoadCategoriesSelectListAsync(viewModel.CategoryId);

        ViewData["Title"] = "Chỉnh sửa FAQ - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa FAQ: {Truncate(faq.Question)}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(faq.Question)}", "") };

        return View(viewModel);
    }

    // POST: Admin/FAQ/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FAQViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("FAQ editing failed for ID: {FaqId}. Model state is invalid.", id);
            viewModel.CategoryList = await LoadCategoriesSelectListAsync(viewModel.CategoryId);
            ViewData["Title"] = "Chỉnh sửa FAQ - Hệ thống quản trị";
            ViewData["PageTitle"] = $"Chỉnh sửa FAQ: {Truncate(viewModel.Question)}";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(viewModel.Question)}", "") };
            return View(viewModel);
        }

        var faq = await _context.Set<FAQ>().FirstOrDefaultAsync(f => f.Id == id);

        if (faq == null)
        {
            _logger.LogWarning("Edit POST: FAQ with ID {FaqId} not found for update.", id);
            TempData["error"] = "Không tìm thấy FAQ để cập nhật.";
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, faq);
        _context.Entry(faq).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("FAQ ID {FaqId} ('{Question}') updated successfully by {User}.", id, faq.Question, User.Identity?.Name ?? "Unknown");
            TempData["success"] = $"Cập nhật FAQ '{Truncate(faq.Question)}' thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict updating FAQ ID: {FaqId}", id);
            TempData["error"] = "Lỗi: Xung đột dữ liệu khi cập nhật. Dữ liệu có thể đã được thay đổi bởi người khác.";
            viewModel.CategoryList = await LoadCategoriesSelectListAsync(viewModel.CategoryId);
            ModelState.AddModelError("", "Dữ liệu đã bị thay đổi bởi người khác. Vui lòng tải lại trang và thử lại.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating FAQ ID: {FaqId}", id);
            ModelState.AddModelError("", "Không thể lưu FAQ. Lỗi cơ sở dữ liệu.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating FAQ ID: {FaqId}", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật FAQ.");
        }

        // If save fails, repopulate and return view
        viewModel.CategoryList = await LoadCategoriesSelectListAsync(viewModel.CategoryId);
        ViewData["Title"] = "Chỉnh sửa FAQ - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa FAQ: {Truncate(viewModel.Question)}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("FAQ", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(viewModel.Question)}", "") };
        return View(viewModel);
    }


    // POST: Admin/FAQ/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var faq = await _context.Set<FAQ>().FindAsync(id);
        if (faq == null)
        {
            _logger.LogWarning("Delete POST: FAQ with ID {FaqId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy FAQ." });
        }

        try
        {
            string question = faq.Question;
            _context.Remove(faq);
            await _context.SaveChangesAsync();

            _logger.LogInformation("FAQ ID {FaqId} ('{Question}') deleted successfully by {User}.", id, question, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = $"Xóa FAQ '{Truncate(question)}' thành công." });
        }
        catch (DbUpdateException ex) 
        {
            _logger.LogError(ex, "Error deleting FAQ ID: {FaqId}. Possible related data.", id);
            return Json(new { success = false, message = "Lỗi: Không thể xóa FAQ. Có thể có dữ liệu liên quan." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting FAQ ID: {FaqId}", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa FAQ." });
        }
    }

    // --- Helper Methods ---

    private async Task<SelectList> LoadCategoriesSelectListAsync(int? selectedValue = null)
    {
        var categories = await _context.Set<Category>()
                                     .Where(c => c.Type == CategoryType.FAQ && c.IsActive)
                                     .OrderBy(c => c.Name)
                                     .Select(c => new { c.Id, c.Name })
                                     .AsNoTracking()
                                     .ToListAsync();
        // Pass selectedValue to the SelectList constructor
        return new SelectList(categories, "Id", "Name", selectedValue);
    }

    private string Truncate(string value, int maxLength = 30)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }
}
