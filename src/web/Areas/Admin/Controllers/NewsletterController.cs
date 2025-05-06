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
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Validators.Newsletter;
using web.Areas.Admin.ViewModels.Newsletter;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class NewsletterController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<NewsletterController> _logger;

    public NewsletterController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<NewsletterController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Newsletter
    public async Task<IActionResult> Index(NewsletterFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new NewsletterFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IQueryable<Newsletter> query = _context.Set<Newsletter>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(n => n.Email.ToLower().Contains(lowerSearchTerm) ||
                                     (n.Name != null && n.Name.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(n => n.IsActive == filter.IsActive.Value);
        }

        query = query.OrderByDescending(n => n.CreatedAt);

        IPagedList<NewsletterListItemViewModel> newslettersPaged = await query
            .ProjectTo<NewsletterListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusSelectList(filter.IsActive);

        NewsletterIndexViewModel viewModel = new()
        {
            Newsletters = newslettersPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Newsletter/Create
    public IActionResult Create()
    {
        NewsletterViewModel viewModel = new()
        {
            IsActive = true
        };
        return View(viewModel);
    }

    // POST: Admin/Newsletter/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsletterViewModel viewModel)
    {
        var result = await new NewsletterViewModelValidator(_context).ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var newsletter = _mapper.Map<Newsletter>(viewModel);
        ApplyStatusTracking(newsletter);
        SetTrackingInfo(newsletter);

        _context.Add(newsletter);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Thêm đăng ký email '{newsletter.Email}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo đăng ký mới cho email: {Email}", viewModel.Email);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi lưu đăng ký.");
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể thêm đăng ký email '{viewModel.Email}'.", ToastType.Error)
        );
        return View(viewModel);
    }

    // GET: Admin/Newsletter/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Newsletter? newsletter = await _context.Set<Newsletter>()
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(n => n.Id == id);

        if (newsletter == null)
        {
            return NotFound();
        }

        NewsletterViewModel viewModel = _mapper.Map<NewsletterViewModel>(newsletter);
        return View(viewModel);
    }

    // POST: Admin/Newsletter/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NewsletterViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await new NewsletterViewModelValidator(_context).ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var newsletter = await _context.Set<Newsletter>().FindAsync(id);
        if (newsletter == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy đăng ký để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, newsletter);
        ApplyStatusTracking(newsletter);

        try
        {
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Cập nhật đăng ký email '{newsletter.Email}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật đăng ký email: {Email}", viewModel.Email);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi cập nhật đăng ký.");
        }

        TempData["ToastMessage"] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể cập nhật đăng ký email '{viewModel.Email}'.", ToastType.Error)
        );
        return View(viewModel);
    }

    // POST: Admin/Newsletter/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var newsletter = await _context.Set<Newsletter>().FindAsync(id);
        if (newsletter == null)
        {
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy đăng ký.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy đăng ký." });
        }

        try
        {
            string email = newsletter.Email;
            _context.Remove(newsletter);
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa đăng ký email '{email}' thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = $"Xóa đăng ký email '{email}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa đăng ký email: {Email}", newsletter.Email);
            TempData["ToastMessage"] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi không mong muốn khi xóa đăng ký.", ToastType.Error)
            );
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa đăng ký." });
        }
    }
}

public partial class NewsletterController
{
    private List<SelectListItem> GetStatusSelectList(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Đang hoạt động", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Không hoạt động", Selected = selectedValue == false }
        };
    }

    private void ApplyStatusTracking(Newsletter newsletter)
    {
        if (newsletter.IsActive)
        {
            newsletter.UnsubscribedAt = null;
            newsletter.ConfirmedAt = DateTime.UtcNow;
        }
        else
        {
            newsletter.UnsubscribedAt = DateTime.UtcNow;
            newsletter.ConfirmedAt = null;
        }
    }

    private void SetTrackingInfo(Newsletter newsletter)
    {
        newsletter.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        newsletter.UserAgent = Request.Headers["User-Agent"].ToString();
    }
}