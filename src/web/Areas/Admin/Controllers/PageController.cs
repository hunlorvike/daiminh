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
using shared.Models;
using System.Text.Json;
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

        if (page.Status == PublishStatus.Published && page.PublishedAt == null)
        {
            page.PublishedAt = DateTime.UtcNow;
        }

        _context.Add(page);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Thêm trang '{page.Title}' thành công.", ToastType.Success)
            );
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
                ModelState.AddModelError("", "Đã xảy ra lỗi cơ sở dữ liệu khi lưu trang.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo trang: {Title}", viewModel.Title);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu trang.");
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể thêm trang '{viewModel.Title}'.", ToastType.Error)
        );
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
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var validator = new PageViewModelValidator(_context);
        var result = await validator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var page = await _context.Set<Page>().FirstOrDefaultAsync(p => p.Id == id);
        if (page == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy trang để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var oldStatus = page.Status;
        _mapper.Map(viewModel, page);

        if (oldStatus != PublishStatus.Published && page.Status == PublishStatus.Published && page.PublishedAt == null)
        {
            page.PublishedAt = DateTime.UtcNow;
        }

        try
        {
            _context.Update(page);
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Cập nhật trang '{page.Title}' thành công.", ToastType.Success)
            );
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

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể cập nhật trang '{viewModel.Title}'.", ToastType.Error)
        );
        return View(viewModel);
    }


    // POST: Admin/Page/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var page = await _context.Set<Page>().FirstOrDefaultAsync(p => p.Id == id);

        if (page == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy trang.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy trang." });
        }

        try
        {
            string pageTitle = page.Title;
            _context.Remove(page);
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa trang '{pageTitle}' thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = $"Xóa trang '{pageTitle}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa trang: {Title}", page.Title);
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", $"Không thể xóa trang '{page.Title}'.", ToastType.Error)
            );
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