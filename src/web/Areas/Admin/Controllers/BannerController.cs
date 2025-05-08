using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Constants;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Validators.Banner;
using web.Areas.Admin.ViewModels.Banner;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class BannerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<BannerController> _logger;

    public BannerController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<BannerController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Banner
    public async Task<IActionResult> Index(BannerFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new BannerFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IQueryable<Banner> query = _context.Set<Banner>()
                                           .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(b => b.Title.ToLower().Contains(lowerSearchTerm) ||
                                     (b.Description != null && b.Description.ToLower().Contains(lowerSearchTerm)) ||
                                     (b.LinkUrl != null && b.LinkUrl.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(b => b.Type == filter.Type.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(b => b.IsActive == filter.IsActive.Value);
        }

        query = query.OrderBy(b => b.OrderIndex).ThenByDescending(b => b.CreatedAt);

        IPagedList<BannerListItemViewModel> bannersPaged = await query
            .ProjectTo<BannerListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.TypeOptions = GetTypeSelectList(filter.Type);
        filter.ActiveStatusOptions = GetActiveStatusSelectList(filter.IsActive);


        BannerIndexViewModel viewModel = new()
        {
            Banners = bannersPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Banner/Create
    public IActionResult Create()
    {
        BannerViewModel viewModel = new()
        {
            IsActive = true,
            OrderIndex = 0,
            TypeOptions = GetTypeSelectList(null)
        };
        return View(viewModel);
    }

    // POST: Admin/Banner/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BannerViewModel viewModel)
    {
        var validator = new BannerViewModelValidator(_context);
        var result = await validator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.TypeOptions = GetTypeSelectList(viewModel.Type);
            return View(viewModel);
        }

        var banner = _mapper.Map<Banner>(viewModel);

        _context.Add(banner);

        try
        {
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Thêm Banner '{banner.Title}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo Banner: {Title}", viewModel.Title);
            ModelState.AddModelError("", "Đã xảy ra lỗi cơ sở dữ liệu khi lưu Banner.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo Banner: {Title}", viewModel.Title);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu Banner.");
        }

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể thêm Banner '{viewModel.Title}'.", ToastType.Error)
        );
        viewModel.TypeOptions = GetTypeSelectList(viewModel.Type);
        return View(viewModel);
    }

    // GET: Admin/Banner/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Banner? banner = await _context.Set<Banner>()
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(b => b.Id == id);

        if (banner == null)
        {
            _logger.LogWarning("Banner không tồn tại khi chỉnh sửa. ID: {Id}", id);
            TempData["ErrorMessage"] = "Không tìm thấy Banner để chỉnh sửa.";
            return RedirectToAction(nameof(Index));
        }

        BannerViewModel viewModel = _mapper.Map<BannerViewModel>(banner);
        viewModel.TypeOptions = GetTypeSelectList(viewModel.Type);

        return View(viewModel);
    }

    // POST: Admin/Banner/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BannerViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var validator = new BannerViewModelValidator(_context);
        var result = await validator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.TypeOptions = GetTypeSelectList(viewModel.Type);
            return View(viewModel);
        }

        var banner = await _context.Set<Banner>().FirstOrDefaultAsync(b => b.Id == id);
        if (banner == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy Banner để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, banner);

        try
        {
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Cập nhật Banner '{banner.Title}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật Banner ID {Id}, Title {Title}", id, banner.Title);
            ModelState.AddModelError("", "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật Banner.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật Banner ID {Id}", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật Banner.");
        }

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể cập nhật Banner '{viewModel.Title}'.", ToastType.Error)
        );
        viewModel.TypeOptions = GetTypeSelectList(viewModel.Type);
        return View(viewModel);
    }

    // POST: Admin/Banner/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var banner = await _context.Set<Banner>().FirstOrDefaultAsync(b => b.Id == id);

        if (banner == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy Banner.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy Banner." });
        }

        try
        {
            string bannerTitle = banner.Title;
            _context.Remove(banner);
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa Banner '{bannerTitle}' thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = $"Xóa Banner '{bannerTitle}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa Banner: {Title}", banner.Title);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi không mong muốn khi xóa Banner.", ToastType.Error)
            );
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa Banner." });
        }
    }
}

public partial class BannerController
{
    private List<SelectListItem> GetActiveStatusSelectList(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Đang kích hoạt", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Đã hủy kích hoạt", Selected = selectedValue == false }
        };
    }

    private List<SelectListItem> GetTypeSelectList(BannerType? selectedValue)
    {
        var types = Enum.GetValues(typeof(BannerType)).Cast<BannerType>();

        var selectList = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả loại", Selected = !selectedValue.HasValue }
        };

        foreach (var type in types)
        {
            selectList.Add(new SelectListItem
            {
                Value = type.ToString(),
                Text = type.GetDisplayName(),
                Selected = selectedValue.HasValue && selectedValue.Value == type
            });
        }

        return selectList;
    }
}