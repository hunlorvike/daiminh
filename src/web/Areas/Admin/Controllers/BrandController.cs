using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")]
public partial class BrandController : Controller
{
    private readonly IBrandService _brandService;
    private readonly ILogger<BrandController> _logger;
    private readonly IValidator<BrandViewModel> _brandViewModelValidator;

    public BrandController(
        IBrandService brandService,
        ILogger<BrandController> logger,
        IValidator<BrandViewModel> brandViewModelValidator)
    {
        _brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _brandViewModelValidator = brandViewModelValidator;
    }

    // GET: Admin/Brand
    public async Task<IActionResult> Index(BrandFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new BrandFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<BrandListItemViewModel> brandsPaged = await _brandService.GetPagedBrandsAsync(filter, pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusSelectList(filter.IsActive);

        BrandIndexViewModel viewModel = new()
        {
            Brands = brandsPaged,
            Filter = filter
        };
        viewModel.Brands = brandsPaged;


        return View(viewModel);
    }

    // GET: Admin/Brand/Create
    public IActionResult Create()
    {
        BrandViewModel viewModel = new()
        {
            IsActive = true,
        };
        return View(viewModel);
    }

    // POST: Admin/Brand/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandViewModel viewModel)
    {
        var result = await _brandViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(viewModel);
        }

        var createResult = await _brandService.CreateBrandAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm thương hiệu thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (!createResult.Errors.Any())
            {
                ModelState.AddModelError(string.Empty, createResult.Message ?? "Không thể thêm thương hiệu.");
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm thương hiệu '{viewModel.Name}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // GET: Admin/Brand/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        BrandViewModel? viewModel = await _brandService.GetBrandByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Brand not found for editing. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy thương hiệu để chỉnh sửa.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }

        return View(viewModel);
    }

    // POST: Admin/Brand/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BrandViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _brandViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var updateResult = await _brandService.UpdateBrandAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật thương hiệu thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (!updateResult.Errors.Any())
            {
                ModelState.AddModelError(string.Empty, updateResult.Message ?? "Không thể cập nhật thương hiệu.");
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật thương hiệu '{viewModel.Name}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // POST: Admin/Brand/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _brandService.DeleteBrandAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa thương hiệu thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa thương hiệu.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }
}

public partial class BrandController
{
    private List<SelectListItem> GetStatusSelectList(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Đang kích hoạt", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Đã hủy kích hoạt", Selected = selectedValue == false }
        };
    }
}