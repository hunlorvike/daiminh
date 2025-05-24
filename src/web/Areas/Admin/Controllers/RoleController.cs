using System.Text.Json;
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
public class RoleController : Controller
{
    private readonly IRoleService _roleService;
    private readonly IClaimDefinitionService _claimDefinitionService;
    private readonly ILogger<RoleController> _logger;
    private readonly IValidator<RoleViewModel> _roleViewModelValidator;

    public RoleController(
        IRoleService roleService,
        IClaimDefinitionService claimDefinitionService,
        ILogger<RoleController> logger,
        IValidator<RoleViewModel> roleViewModelValidator)
    {
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _claimDefinitionService = claimDefinitionService ?? throw new ArgumentNullException(nameof(claimDefinitionService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _roleViewModelValidator = roleViewModelValidator ?? throw new ArgumentNullException(nameof(roleViewModelValidator));
    }

    // GET: Admin/Role
    [Authorize(Policy = PermissionConstants.RoleManage)]
    public async Task<IActionResult> Index(RoleFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new RoleFilterViewModel();

        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<RoleListItemViewModel> rolesPaged =
            await _roleService.GetPagedRolesAsync(filter, pageNumber, currentPageSize);

        RoleIndexViewModel viewModel = new()
        {
            Roles = rolesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Role/Create
    [Authorize(Policy = PermissionConstants.RoleManage)]
    public async Task<IActionResult> Create()
    {
        RoleViewModel viewModel = new();
        viewModel.AvailableClaimDefinitions = await _claimDefinitionService.GetAllClaimDefinitionsAsync()
                                                                         .ContinueWith(t => t.Result.Select(cd => new SelectListItem
                                                                         {
                                                                             Value = cd.Id.ToString(),
                                                                             Text = $"{cd.Type}: {cd.Value} - {cd.Description}",
                                                                             Selected = false
                                                                         }).ToList());
        return View(viewModel);
    }

    // POST: Admin/Role/Create
    [Authorize(Policy = PermissionConstants.RoleManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoleViewModel viewModel)
    {
        var result = await _roleViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.AvailableClaimDefinitions = await _claimDefinitionService.GetAllClaimDefinitionsAsync()
                                                                             .ContinueWith(t => t.Result.Select(cd => new SelectListItem
                                                                             {
                                                                                 Value = cd.Id.ToString(),
                                                                                 Text = $"{cd.Type}: {cd.Value} - {cd.Description}",
                                                                                 Selected = viewModel.SelectedClaimDefinitionIds.Contains(cd.Id) // Giữ lại các lựa chọn của người dùng
                                                                             }).ToList());
            return View(viewModel);
        }

        var createResult = await _roleService.CreateRoleAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm vai trò thành công.", ToastType.Success)
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
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm vai trò '{viewModel.Name}'.", ToastType.Error)
            );
            viewModel.AvailableClaimDefinitions = await _claimDefinitionService.GetAllClaimDefinitionsAsync()
                                                                             .ContinueWith(t => t.Result.Select(cd => new SelectListItem
                                                                             {
                                                                                 Value = cd.Id.ToString(),
                                                                                 Text = $"{cd.Type}: {cd.Value} - {cd.Description}",
                                                                                 Selected = viewModel.SelectedClaimDefinitionIds.Contains(cd.Id)
                                                                             }).ToList());
            return View(viewModel);
        }
    }

    // GET: Admin/Role/Edit/5
    [Authorize(Policy = PermissionConstants.RoleManage)]
    public async Task<IActionResult> Edit(int id)
    {
        RoleViewModel? viewModel = await _roleService.GetRoleByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Role not found for editing: ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy vai trò để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        // Lấy tất cả Claim Definitions và đánh dấu các claim đã chọn
        viewModel.AvailableClaimDefinitions = await _claimDefinitionService.GetAllClaimDefinitionsAsync()
                                                                         .ContinueWith(t => t.Result.Select(cd => new SelectListItem
                                                                         {
                                                                             Value = cd.Id.ToString(),
                                                                             Text = $"{cd.Type}: {cd.Value} - {cd.Description}",
                                                                             Selected = viewModel.SelectedClaimDefinitionIds.Contains(cd.Id)
                                                                         }).ToList());
        return View(viewModel);
    }

    // POST: Admin/Role/Edit/5
    [Authorize(Policy = PermissionConstants.RoleManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RoleViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _roleViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            // Populate lại AvailableClaimDefinitions nếu validation thất bại
            viewModel.AvailableClaimDefinitions = await _claimDefinitionService.GetAllClaimDefinitionsAsync()
                                                                             .ContinueWith(t => t.Result.Select(cd => new SelectListItem
                                                                             {
                                                                                 Value = cd.Id.ToString(),
                                                                                 Text = $"{cd.Type}: {cd.Value} - {cd.Description}",
                                                                                 Selected = viewModel.SelectedClaimDefinitionIds.Contains(cd.Id)
                                                                             }).ToList());
            return View(viewModel);
        }

        var updateResult = await _roleService.UpdateRoleAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật vai trò thành công.", ToastType.Success)
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
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật vai trò '{viewModel.Name}'.", ToastType.Error)
            );
            // Populate lại AvailableClaimDefinitions và SelectedClaimDefinitionIds nếu thất bại
            viewModel.AvailableClaimDefinitions = await _claimDefinitionService.GetAllClaimDefinitionsAsync()
                                                                             .ContinueWith(t => t.Result.Select(cd => new SelectListItem
                                                                             {
                                                                                 Value = cd.Id.ToString(),
                                                                                 Text = $"{cd.Type}: {cd.Value} - {cd.Description}",
                                                                                 Selected = viewModel.SelectedClaimDefinitionIds.Contains(cd.Id)
                                                                             }).ToList());
            return View(viewModel);
        }
    }

    // POST: Admin/Role/Delete/5
    [Authorize(Policy = PermissionConstants.RoleManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _roleService.DeleteRoleAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa vai trò thành công.", ToastType.Success)
            );
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa vai trò.", ToastType.Error)
            );
        }
        return RedirectToAction(nameof(Index));
    }
}