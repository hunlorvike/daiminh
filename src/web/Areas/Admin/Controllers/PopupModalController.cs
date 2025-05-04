using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using infrastructure;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.Validators.PopupModal;
using web.Areas.Admin.ViewModels.PopupModal;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class PopupModalController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<PopupModalController> _logger;

    public PopupModalController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<PopupModalController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/PopupModal
    public async Task<IActionResult> Index(PopupModalFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new PopupModalFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IQueryable<PopupModal> query = _context.Set<PopupModal>()
                                               .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Title.ToLower().Contains(lowerSearchTerm) ||
                                     (p.Content != null && p.Content.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == filter.IsActive.Value);
        }

        query = query.OrderByDescending(p => p.CreatedAt);

        IPagedList<PopupModalListItemViewModel> popupModalsPaged = await query
            .ProjectTo<PopupModalListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.ActiveStatusOptions = GetActiveStatusSelectList(filter.IsActive);

        PopupModalIndexViewModel viewModel = new()
        {
            PopupModals = popupModalsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/PopupModal/Create
    public IActionResult Create()
    {
        PopupModalViewModel viewModel = new()
        {
            IsActive = true,
        };
        return View(viewModel);
    }

    // POST: Admin/PopupModal/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PopupModalViewModel viewModel)
    {
        var validator = new PopupModalViewModelValidator(_context);
        var result = await validator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            // Re-populate select lists if any (not needed for this simple VM, but good practice)
            return View(viewModel);
        }

        var popupModal = _mapper.Map<PopupModal>(viewModel);

        _context.Add(popupModal);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Thêm Popup '{popupModal.Title}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo Popup: {Title}", viewModel.Title);
            ModelState.AddModelError("", "Đã xảy ra lỗi cơ sở dữ liệu khi lưu Popup.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo Popup: {Title}", viewModel.Title);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu Popup.");
        }

        return View(viewModel);
    }

    // GET: Admin/PopupModal/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        PopupModal? popupModal = await _context.Set<PopupModal>()
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(p => p.Id == id);

        if (popupModal == null)
        {
            _logger.LogWarning("Popup không tồn tại khi chỉnh sửa. ID: {Id}", id);
            TempData["ErrorMessage"] = "Không tìm thấy Popup để chỉnh sửa.";
            return RedirectToAction(nameof(Index));
        }

        PopupModalViewModel viewModel = _mapper.Map<PopupModalViewModel>(popupModal);

        return View(viewModel);
    }

    // POST: Admin/PopupModal/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PopupModalViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("ID trong route ({RouteId}) và ViewModel ({ViewModelId}) không khớp khi chỉnh sửa Popup.", id, viewModel.Id);
            TempData["ErrorMessage"] = "Yêu cầu chỉnh sửa không hợp lệ.";
            return RedirectToAction(nameof(Index));
        }

        var validator = new PopupModalViewModelValidator(_context);
        var result = await validator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var popupModal = await _context.Set<PopupModal>().FirstOrDefaultAsync(p => p.Id == id);
        if (popupModal == null)
        {
            _logger.LogWarning("Popup không tồn tại khi cập nhật. ID: {Id}", id);
            TempData["ErrorMessage"] = "Không tìm thấy Popup để cập nhật.";
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, popupModal);

        try
        {
            _context.Update(popupModal);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Cập nhật Popup '{popupModal.Title}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật Popup ID {Id}, Title {Title}", id, popupModal.Title);
            ModelState.AddModelError("", "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật Popup.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật Popup ID {Id}", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật Popup.");
        }

        return View(viewModel);
    }

    // POST: Admin/PopupModal/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var popupModal = await _context.Set<PopupModal>()
                                  .FirstOrDefaultAsync(p => p.Id == id);

        if (popupModal == null)
        {
            _logger.LogWarning("Popup không tồn tại khi xóa. ID: {Id}", id);
            return Json(new { success = false, message = "Không tìm thấy Popup." });
        }

        try
        {
            string popupTitle = popupModal.Title;
            _context.Remove(popupModal);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Đã xóa Popup: {Title}", popupTitle);
            return Json(new { success = true, message = $"Xóa Popup '{popupTitle}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa Popup: {Title}", popupModal.Title);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa Popup." });
        }
    }
}

public partial class PopupModalController
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