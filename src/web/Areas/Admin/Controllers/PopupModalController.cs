using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public partial class PopupModalController : Controller
{
    private readonly IPopupModalService _popupModalService;
    private readonly IValidator<PopupModalViewModel> _popupModalViewModelValidator;
    private readonly ILogger<PopupModalController> _logger;

    public PopupModalController(
        IPopupModalService popupModalService,
        IValidator<PopupModalViewModel> popupModalViewModelValidator,
        ILogger<PopupModalController> logger)
    {
        _popupModalService = popupModalService ?? throw new ArgumentNullException(nameof(popupModalService));
        _popupModalViewModelValidator = popupModalViewModelValidator ?? throw new ArgumentNullException(nameof(popupModalViewModelValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/PopupModal
    [Authorize(Policy = PermissionConstants.PopupModalView)]
    public async Task<IActionResult> Index(PopupModalFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new PopupModalFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<PopupModalListItemViewModel> popupModalsPaged = await _popupModalService.GetPagedPopupModalsAsync(filter, pageNumber, currentPageSize);

        filter.ActiveStatusOptions = GetActiveStatusSelectList(filter.IsActive);

        PopupModalIndexViewModel viewModel = new()
        {
            PopupModals = popupModalsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/PopupModal/Create
    [Authorize(Policy = PermissionConstants.PopupModalCreate)]
    public IActionResult Create()
    {
        PopupModalViewModel viewModel = new()
        {
            IsActive = true,
            TargetPages = "AllPages",
            DisplayFrequency = DisplayFrequency.Always,
            DelaySeconds = 0,
            OrderIndex = 0
        };
        PopulateSelectLists(viewModel);
        return View(viewModel);
    }

    // POST: Admin/PopupModal/Create
    [Authorize(Policy = PermissionConstants.PopupModalCreate)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PopupModalViewModel viewModel)
    {
        var validationResult = await _popupModalViewModelValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            PopulateSelectLists(viewModel);
            return View(viewModel);
        }

        var createResult = await _popupModalService.CreatePopupModalAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm Popup thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in createResult.Errors)
                ModelState.AddModelError(string.Empty, error);
            if (!createResult.Errors.Any() && !string.IsNullOrEmpty(createResult.Message))
                ModelState.AddModelError(string.Empty, createResult.Message);

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm Popup '{viewModel.Title}'.", ToastType.Error)
            );
            PopulateSelectLists(viewModel);
            return View(viewModel);
        }
    }

    // GET: Admin/PopupModal/Edit/5
    [Authorize(Policy = PermissionConstants.PopupModalEdit)]
    public async Task<IActionResult> Edit(int id)
    {
        PopupModalViewModel? viewModel = await _popupModalService.GetPopupModalByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Popup không tồn tại khi chỉnh sửa. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy Popup để chỉnh sửa.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
        PopulateSelectLists(viewModel);
        return View(viewModel);
    }

    // POST: Admin/PopupModal/Edit/5
    [Authorize(Policy = PermissionConstants.PopupModalEdit)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PopupModalViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var validationResult = await _popupModalViewModelValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            PopulateSelectLists(viewModel);
            return View(viewModel);
        }

        var updateResult = await _popupModalService.UpdatePopupModalAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật Popup thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
                ModelState.AddModelError(string.Empty, error);
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
                ModelState.AddModelError(string.Empty, updateResult.Message);

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật Popup '{viewModel.Title}'.", ToastType.Error)
            );
            PopulateSelectLists(viewModel);
            return View(viewModel);
        }
    }

    // POST: Admin/PopupModal/Delete/5
    [Authorize(Policy = PermissionConstants.PopupModalDelete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _popupModalService.DeletePopupModalAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa Popup thành công.", ToastType.Success)
            );
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa Popup.", ToastType.Error)
            );
        }
        return RedirectToAction(nameof(Index));
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

    private void PopulateSelectLists(PopupModalViewModel viewModel)
    {
        viewModel.DisplayFrequencyOptions = Enum.GetValues(typeof(DisplayFrequency))
            .Cast<DisplayFrequency>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.GetDisplayName(), Selected = e == viewModel.DisplayFrequency })
            .ToList();
    }
}