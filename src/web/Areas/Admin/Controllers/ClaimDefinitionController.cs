using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public class ClaimDefinitionController : Controller
{
    private readonly IClaimDefinitionService _claimDefinitionService;
    private readonly ILogger<ClaimDefinitionController> _logger;
    private readonly IValidator<ClaimDefinitionViewModel> _claimDefinitionViewModelValidator;

    public ClaimDefinitionController(
        IClaimDefinitionService claimDefinitionService,
        ILogger<ClaimDefinitionController> logger,
        IValidator<ClaimDefinitionViewModel> claimDefinitionViewModelValidator)
    {
        _claimDefinitionService = claimDefinitionService ?? throw new ArgumentNullException(nameof(claimDefinitionService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _claimDefinitionViewModelValidator = claimDefinitionViewModelValidator ?? throw new ArgumentNullException(nameof(claimDefinitionViewModelValidator));
    }

    // GET: Admin/ClaimDefinition
    [Authorize(Policy = PermissionConstants.ClaimDefinitionManage)]
    public async Task<IActionResult> Index(ClaimDefinitionFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new ClaimDefinitionFilterViewModel();

        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<ClaimDefinitionListItemViewModel> claimDefinitionsPaged =
            await _claimDefinitionService.GetPagedClaimDefinitionsAsync(filter, pageNumber, currentPageSize);

        ClaimDefinitionIndexViewModel viewModel = new()
        {
            ClaimDefinitions = claimDefinitionsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/ClaimDefinition/Create
    [Authorize(Policy = PermissionConstants.ClaimDefinitionManage)]
    public IActionResult Create()
    {
        ClaimDefinitionViewModel viewModel = new();
        return View(viewModel);
    }

    // POST: Admin/ClaimDefinition/Create
    [Authorize(Policy = PermissionConstants.ClaimDefinitionManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClaimDefinitionViewModel viewModel)
    {
        var result = await _claimDefinitionViewModelValidator.ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(viewModel);
        }

        var createResult = await _claimDefinitionService.CreateClaimDefinitionAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm định nghĩa quyền hạn thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in createResult.Errors)
                ModelState.AddModelError(string.Empty, error);
            if (!createResult.Errors.Any() && !string.IsNullOrEmpty(createResult.Message))
            {
                ModelState.AddModelError(string.Empty, createResult.Message);
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm định nghĩa quyền hạn '{viewModel.Value}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // GET: Admin/ClaimDefinition/Edit/5
    [Authorize(Policy = PermissionConstants.ClaimDefinitionManage)]
    public async Task<IActionResult> Edit(int id)
    {
        ClaimDefinitionViewModel? viewModel = await _claimDefinitionService.GetClaimDefinitionByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("ClaimDefinition not found for editing: ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy định nghĩa quyền hạn để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        return View(viewModel);
    }

    // POST: Admin/ClaimDefinition/Edit/5
    [Authorize(Policy = PermissionConstants.ClaimDefinitionManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ClaimDefinitionViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _claimDefinitionViewModelValidator.ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(viewModel);
        }

        var updateResult = await _claimDefinitionService.UpdateClaimDefinitionAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật định nghĩa quyền hạn thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
                ModelState.AddModelError(string.Empty, error);
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật định nghĩa quyền hạn '{viewModel.Value}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // POST: Admin/ClaimDefinition/Delete/5
    [Authorize(Policy = PermissionConstants.ClaimDefinitionManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _claimDefinitionService.DeleteClaimDefinitionAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa định nghĩa quyền hạn thành công.", ToastType.Success)
            );
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa định nghĩa quyền hạn.", ToastType.Error)
            );
        }
        return RedirectToAction(nameof(Index));
    }
}