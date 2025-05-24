using System.Text.Json;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public partial class SlideController : Controller
{
    private readonly ISlideService _slideService;
    private readonly IMapper _mapper;
    private readonly ILogger<SlideController> _logger;
    private readonly IValidator<SlideViewModel> _slideViewModelValidator;

    public SlideController(
        ISlideService slideService,
        IMapper mapper,
        ILogger<SlideController> logger,
        IValidator<SlideViewModel> slideViewModelValidator)
    {
        _slideService = slideService ?? throw new ArgumentNullException(nameof(slideService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _slideViewModelValidator = slideViewModelValidator ?? throw new ArgumentNullException(nameof(slideViewModelValidator));
    }

    // GET: Admin/Slide
    [Authorize(Policy = PermissionConstants.SlideView)]
    public async Task<IActionResult> Index(SlideFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new SlideFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<SlideListItemViewModel> slidesPaged = await _slideService.GetPagedSlidesAsync(filter, pageNumber, currentPageSize);

        filter.ActiveStatusOptions = GetActiveStatusSelectList(filter.IsActive);

        SlideIndexViewModel viewModel = new()
        {
            Slides = slidesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Slide/Create
    [Authorize(Policy = PermissionConstants.SlideCreate)]
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
    [Authorize(Policy = PermissionConstants.SlideCreate)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SlideViewModel viewModel)
    {
        var validationResult = await _slideViewModelValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var createResult = await _slideService.CreateSlideAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm Slide thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (!createResult.Errors.Any() && !string.IsNullOrEmpty(createResult.Message))
            {
                ModelState.AddModelError(string.Empty, createResult.Message);
            }


            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm Slide '{viewModel.Title}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // GET: Admin/Slide/Edit/5
    [Authorize(Policy = PermissionConstants.SlideEdit)]
    public async Task<IActionResult> Edit(int id)
    {
        SlideViewModel? viewModel = await _slideService.GetSlideByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Slide không tồn tại khi chỉnh sửa. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy Slide để chỉnh sửa.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    // POST: Admin/Slide/Edit/5
    [Authorize(Policy = PermissionConstants.SlideEdit)]
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

        var result = await _slideViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var updateResult = await _slideService.UpdateSlideAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật Slide thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }


            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật Slide '{viewModel.Title}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // POST: Admin/Slide/Delete/5
    [Authorize(Policy = PermissionConstants.SlideDelete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _slideService.DeleteSlideAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa Slide thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa Slide.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
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
