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
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Validators.Slide;
using web.Areas.Admin.ViewModels.Slide;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class SlideController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<SlideController> _logger;

    public SlideController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<SlideController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Slide
    public async Task<IActionResult> Index(SlideFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new SlideFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IQueryable<Slide> query = _context.Set<Slide>()
                                          .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(s => s.Title.ToLower().Contains(lowerSearchTerm) ||
                                     (s.Subtitle != null && s.Subtitle.ToLower().Contains(lowerSearchTerm)) ||
                                     (s.Description != null && s.Description.ToLower().Contains(lowerSearchTerm)) ||
                                     (s.CtaText != null && s.CtaText.ToLower().Contains(lowerSearchTerm)) ||
                                     (s.CtaLink != null && s.CtaLink.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(s => s.IsActive == filter.IsActive.Value);
        }

        query = query.OrderBy(s => s.OrderIndex).ThenByDescending(s => s.CreatedAt);

        IPagedList<SlideListItemViewModel> slidesPaged = await query
            .ProjectTo<SlideListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.ActiveStatusOptions = GetActiveStatusSelectList(filter.IsActive);

        SlideIndexViewModel viewModel = new()
        {
            Slides = slidesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Slide/Create
    public IActionResult Create()
    {
        SlideViewModel viewModel = new()
        {
            IsActive = true,
            OrderIndex = 0,
            Target = "_self"
        };
        return View(viewModel);
    }

    // POST: Admin/Slide/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SlideViewModel viewModel)
    {
        var validator = new SlideViewModelValidator(_context);
        var result = await validator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var slide = _mapper.Map<Slide>(viewModel);

        _context.Add(slide);

        try
        {
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Thêm Slide '{slide.Title}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo Slide: {Title}", viewModel.Title);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu Slide.");
        }

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể thêm Slide '{viewModel.Title}'.", ToastType.Error)
        );
        return View(viewModel);
    }

    // GET: Admin/Slide/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Slide? slide = await _context.Set<Slide>()
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(s => s.Id == id);

        if (slide == null)
        {
            _logger.LogWarning("Slide không tồn tại khi chỉnh sửa. ID: {Id}", id);
            TempData["ErrorMessage"] = "Không tìm thấy Slide để chỉnh sửa.";
            return RedirectToAction(nameof(Index));
        }

        SlideViewModel viewModel = _mapper.Map<SlideViewModel>(slide);

        return View(viewModel);
    }

    // POST: Admin/Slide/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SlideViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var validator = new SlideViewModelValidator(_context);
        var result = await validator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var slide = await _context.Set<Slide>().FirstOrDefaultAsync(s => s.Id == id);
        if (slide == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy Slide để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, slide);

        try
        {
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Cập nhật Slide '{slide.Title}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật Slide: {Title}", viewModel.Title);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật Slide.");
        }

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể cập nhật Slide '{viewModel.Title}'.", ToastType.Error)
        );
        return View(viewModel);
    }

    // POST: Admin/Slide/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var slide = await _context.Set<Slide>().FirstOrDefaultAsync(s => s.Id == id);

        if (slide == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy Slide.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy Slide." });
        }

        try
        {
            string slideTitle = slide.Title;
            _context.Remove(slide);
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa Slide '{slideTitle}' thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = $"Xóa Slide '{slideTitle}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa Slide: {Title}", slide.Title);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi không mong muốn khi xóa Slide.", ToastType.Error)
            );
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa Slide." });
        }
    }
}

public partial class SlideController
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
}