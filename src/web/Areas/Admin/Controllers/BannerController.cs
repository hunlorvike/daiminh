using System.Text.Json;
using AutoMapper;
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
public partial class BannerController : Controller
{
    private readonly IBannerService _bannerService;
    private readonly IMapper _mapper;
    private readonly ILogger<BannerController> _logger;
    private readonly IValidator<BannerViewModel> _bannerViewModelValidator;

    public BannerController(
        IBannerService bannerService,
        IMapper mapper,
        ILogger<BannerController> logger,
        IValidator<BannerViewModel> bannerViewModelValidator)
    {
        _bannerService = bannerService ?? throw new ArgumentNullException(nameof(bannerService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bannerViewModelValidator = bannerViewModelValidator ?? throw new ArgumentNullException(nameof(bannerViewModelValidator));
    }

    // GET: Admin/Banner
    [Authorize(Policy = PermissionConstants.BannerView)]
    public async Task<IActionResult> Index(BannerFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new BannerFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<BannerListItemViewModel> bannersPaged = await _bannerService.GetPagedBannersAsync(filter, pageNumber, currentPageSize);

        // Call simple helpers for select lists (these don't use DB)
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
    [Authorize(Policy = PermissionConstants.BannerCreate)]
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
    [Authorize(Policy = PermissionConstants.BannerCreate)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BannerViewModel viewModel)
    {
        var validationResult = await _bannerViewModelValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.TypeOptions = GetTypeSelectList(viewModel.Type);
            return View(viewModel);
        }

        var createResult = await _bannerService.CreateBannerAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm Banner thành công.", ToastType.Success)
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
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm Banner '{viewModel.Title}'.", ToastType.Error)
            );
            viewModel.TypeOptions = GetTypeSelectList(viewModel.Type);
            return View(viewModel);
        }
    }

    // GET: Admin/Banner/Edit/5
    [Authorize(Policy = PermissionConstants.BannerEdit)]
    public async Task<IActionResult> Edit(int id)
    {
        BannerViewModel? viewModel = await _bannerService.GetBannerByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Banner không tồn tại khi chỉnh sửa. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy Banner để chỉnh sửa.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }

        viewModel.TypeOptions = GetTypeSelectList(viewModel.Type);

        return View(viewModel);
    }

    // POST: Admin/Banner/Edit/5
    [Authorize(Policy = PermissionConstants.BannerEdit)]
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

        var validationResult = await _bannerViewModelValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.TypeOptions = GetTypeSelectList(viewModel.Type);
            return View(viewModel);
        }

        var updateResult = await _bannerService.UpdateBannerAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật Banner thành công.", ToastType.Success)
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
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật Banner '{viewModel.Title}'.", ToastType.Error)
            );
            viewModel.TypeOptions = GetTypeSelectList(viewModel.Type);
            return View(viewModel);
        }
    }

    // POST: Admin/Banner/Delete/5
    [Authorize(Policy = PermissionConstants.BannerDelete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _bannerService.DeleteBannerAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa Banner thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa Banner.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
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
