using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.Validators.Page;
using web.Areas.Admin.ViewModels.Page;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class PageController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<PageController> _logger;

    public PageController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<PageController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Page
    public async Task<IActionResult> Index(PageFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new PageFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IQueryable<Page> query = _context.Set<Page>()
                                         .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Title.ToLower().Contains(lowerSearchTerm) ||
                                     (p.Content != null && p.Content.ToLower().Contains(lowerSearchTerm)) || // Search content too? Might be slow on large text. Title/Slug is safer. Sticking to example - search description for Brand.
                                     p.Slug.ToLower().Contains(lowerSearchTerm)); // Adding slug search as it's a key identifier.
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(p => p.Status == filter.Status.Value);
        }

        query = query.OrderByDescending(p => p.UpdatedAt).ThenBy(p => p.Title); // Order by update date then title

        IPagedList<PageListItemViewModel> pagesPaged = await query
            .ProjectTo<PageListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusSelectList(filter.Status);

        PageIndexViewModel viewModel = new()
        {
            Pages = pagesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Page/Create
    public IActionResult Create()
    {
        PageViewModel viewModel = new()
        {
            Status = PublishStatus.Draft, // Default status
            PublishedAt = null // No published date initially
        };
        return View(viewModel);
    }

    // POST: Admin/Page/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PageViewModel viewModel)
    {
        var validator = new PageViewModelValidator(_context);
        var result = await validator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var page = _mapper.Map<Page>(viewModel);

        // Set PublishedAt if status is Published and date is not provided
        if (page.Status == PublishStatus.Published && page.PublishedAt == null)
        {
            // Use UtcNow for consistency with BaseEntity
            page.PublishedAt = DateTime.UtcNow;
        }
        // If status is not Published, ensure PublishedAt is null or keep as is if it was previously published?
        // Let's clear it if status changes FROM published, but the entity structure doesn't track old status.
        // Simplest: only set PublishedAt if status IS Published and date IS NULL.

        _context.Add(page);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Thêm trang '{page.Title}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo trang: {Title}", viewModel.Title);
            if (ex.InnerException?.Message.Contains("slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã được sử dụng.");
            }
            else
            {
                // More general DB error
                ModelState.AddModelError("", "Đã xảy ra lỗi cơ sở dữ liệu khi lưu trang.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo trang: {Title}", viewModel.Title);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu trang.");
        }

        return View(viewModel);
    }

    // GET: Admin/Page/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Page? page = await _context.Set<Page>()
                                   .AsNoTracking() // Readonly operation
                                   .FirstOrDefaultAsync(p => p.Id == id);

        if (page == null)
        {
            _logger.LogWarning("Trang không tồn tại khi chỉnh sửa. ID: {Id}", id);
            TempData["ErrorMessage"] = "Không tìm thấy trang để chỉnh sửa."; // Inform user
            return RedirectToAction(nameof(Index)); // Redirect instead of NotFound view
        }

        PageViewModel viewModel = _mapper.Map<PageViewModel>(page);

        // Adjust PublishedAt for DateTimeLocal input if needed
        // viewModel.PublishedAt = page.PublishedAt?.ToLocalTime(); // If using local time in View

        return View(viewModel);
    }

    // POST: Admin/Page/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PageViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("ID trong route ({RouteId}) và ViewModel ({ViewModelId}) không khớp khi chỉnh sửa trang.", id, viewModel.Id);
            TempData["ErrorMessage"] = "Yêu cầu chỉnh sửa không hợp lệ.";
            return RedirectToAction(nameof(Index)); // Redirect for invalid ID
        }

        var validator = new PageViewModelValidator(_context);
        var result = await validator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        // Fetch the entity to update
        var page = await _context.Set<Page>().FirstOrDefaultAsync(p => p.Id == id);
        if (page == null)
        {
            _logger.LogWarning("Trang không tồn tại khi cập nhật. ID: {Id}", id);
            TempData["ErrorMessage"] = "Không tìm thấy trang để cập nhật.";
            return RedirectToAction(nameof(Index));
        }

        // Store old status to check for publish transition
        var oldStatus = page.Status;

        // Map updated values from ViewModel to Entity
        _mapper.Map(viewModel, page);

        // Handle PublishedAt on status change to Published
        if (oldStatus != PublishStatus.Published && page.Status == PublishStatus.Published && page.PublishedAt == null)
        {
            // Use UtcNow for consistency
            page.PublishedAt = DateTime.UtcNow;
        }
        // If status changes from Published to Draft/Archived, should PublishedAt be cleared?
        // Based on the entity, keeping it might be okay (indicates *first* publish date),
        // but clearing it could indicate it's no longer "currently" published.
        // Let's keep it simple: only set on the first transition TO Published.
        // If they manually set a PublishedAt date in the form, the mapper handles it.


        try
        {
            _context.Update(page); // Mark as modified explicitly if not tracking, though FirstOrDefaultAsync tracks.
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Cập nhật trang '{page.Title}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật trang ID {Id}, Title {Title}", id, page.Title);
            if (ex.InnerException?.Message.Contains("slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã được sử dụng.");
            }
            else
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật trang.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật trang ID {Id}", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật trang.");
        }

        return View(viewModel);
    }

    // POST: Admin/Page/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var page = await _context.Set<Page>()
                                  .FirstOrDefaultAsync(p => p.Id == id);

        if (page == null)
        {
            _logger.LogWarning("Trang không tồn tại khi xóa. ID: {Id}", id);
            return Json(new { success = false, message = "Không tìm thấy trang." });
        }

        // Unlike Brand, Page has no dependent entities in its current definition,
        // so no dependency check needed here.

        try
        {
            string pageTitle = page.Title;
            _context.Remove(page);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Đã xóa trang: {Title}", pageTitle);
            return Json(new { success = true, message = $"Xóa trang '{pageTitle}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa trang: {Title}", page.Title);
            // Check for specific constraint violations if needed, but generic error is often enough
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa trang." });
        }
    }
}

// Partial class for helper methods
public partial class PageController
{
    private List<SelectListItem> GetStatusSelectList(PublishStatus? selectedValue)
    {
        // Get all enum values
        var statuses = Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>();

        var selectList = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue }
        };

        foreach (var status in statuses)
        {
            selectList.Add(new SelectListItem
            {
                Value = status.ToString(),
                Text = status.GetDisplayName(),
                Selected = selectedValue.HasValue && selectedValue.Value == status
            });
        }

        return selectList;
    }
}