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
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IClaimDefinitionService _claimDefinitionService;
    private readonly ILogger<UserController> _logger;
    private readonly IValidator<UserViewModel> _userViewModelValidator;
    private readonly IValidator<UserChangePasswordViewModel> _userChangePasswordViewModelValidator;

    public UserController(
        IUserService userService,
        IRoleService roleService,
        IClaimDefinitionService claimDefinitionService,
        ILogger<UserController> logger,
        IValidator<UserViewModel> userViewModelValidator,
        IValidator<UserChangePasswordViewModel> userChangePasswordViewModelValidator)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _claimDefinitionService = claimDefinitionService ?? throw new ArgumentNullException(nameof(claimDefinitionService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userViewModelValidator = userViewModelValidator ?? throw new ArgumentNullException(nameof(userViewModelValidator));
        _userChangePasswordViewModelValidator = userChangePasswordViewModelValidator ?? throw new ArgumentNullException(nameof(userChangePasswordViewModelValidator));
    }

    // GET: Admin/User
    [Authorize(Policy = PermissionConstants.UserManage)]
    public async Task<IActionResult> Index(UserFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new UserFilterViewModel();

        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<UserListItemViewModel> usersPaged =
            await _userService.GetPagedUsersAsync(filter, pageNumber, currentPageSize);

        filter.AvailableRoles = await _roleService.GetAllRolesAsSelectListAsync(filter.RoleIdFilter);
        filter.AvailableRoles.Insert(0, new SelectListItem { Value = "", Text = "— Tất cả vai trò —" });

        UserIndexViewModel viewModel = new()
        {
            Users = usersPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/User/Create
    [Authorize(Policy = PermissionConstants.UserManage)]
    public async Task<IActionResult> Create()
    {
        UserViewModel viewModel = new()
        {
            IsActive = true,
        };

        viewModel.AvailableRoles = await _roleService.GetAllRolesAsSelectListAsync();
        viewModel.AvailableRoles.Insert(0, new SelectListItem { Value = "", Text = "— Chọn vai trò —" });

        viewModel.AvailableClaimDefinitions = await _claimDefinitionService.GetAllClaimDefinitionsAsync()
                                                                         .ContinueWith(t => t.Result.Select(cd => new SelectListItem
                                                                         {
                                                                             Value = cd.Id.ToString(),
                                                                             Text = $"{cd.Type}: {cd.Value} - {cd.Description}",
                                                                             Selected = false
                                                                         }).ToList());
        return View(viewModel);
    }

    // POST: Admin/User/Create
    [Authorize(Policy = PermissionConstants.UserManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel viewModel)
    {
        var result = await _userViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.AvailableRoles = await _roleService.GetAllRolesAsSelectListAsync();
            viewModel.AvailableRoles.Insert(0, new SelectListItem { Value = "", Text = "— Chọn vai trò —" });
            viewModel.AvailableClaimDefinitions = await _claimDefinitionService.GetAllClaimDefinitionsAsync()
                                                                             .ContinueWith(t => t.Result.Select(cd => new SelectListItem
                                                                             {
                                                                                 Value = cd.Id.ToString(),
                                                                                 Text = $"{cd.Type}: {cd.Value} - {cd.Description}",
                                                                                 Selected = viewModel.SelectedClaimDefinitionIds.Contains(cd.Id)
                                                                             }).ToList());
            return View(viewModel);
        }

        var createResult = await _userService.CreateUserAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm người dùng thành công.", ToastType.Success)
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
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm người dùng '{viewModel.Email}'.", ToastType.Error)
            );
            viewModel.AvailableRoles = await _roleService.GetAllRolesAsSelectListAsync();
            viewModel.AvailableRoles.Insert(0, new SelectListItem { Value = "", Text = "— Chọn vai trò —" });
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

    // GET: Admin/User/Edit/5
    [Authorize(Policy = PermissionConstants.UserManage)]
    public async Task<IActionResult> Edit(int id)
    {
        UserViewModel? viewModel = await _userService.GetUserByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("User not found for editing: ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy người dùng để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        viewModel.AvailableRoles = await _roleService.GetAllRolesAsSelectListAsync();
        foreach (var item in viewModel.AvailableRoles)
        {
            if (int.TryParse(item.Value, out int roleIdValue))
            {
                item.Selected = viewModel.SelectedRoleIds.Contains(roleIdValue);
            }
        }

        viewModel.AvailableClaimDefinitions = await _claimDefinitionService.GetAllClaimDefinitionsAsync()
                                                                         .ContinueWith(t => t.Result.Select(cd => new SelectListItem
                                                                         {
                                                                             Value = cd.Id.ToString(),
                                                                             Text = $"{cd.Type}: {cd.Value} - {cd.Description}",
                                                                             Selected = viewModel.SelectedClaimDefinitionIds.Contains(cd.Id)
                                                                         }).ToList());
        return View(viewModel);
    }

    // POST: Admin/User/Edit/5
    [Authorize(Policy = PermissionConstants.UserManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _userViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.AvailableRoles = await _roleService.GetAllRolesAsSelectListAsync();
            foreach (var item in viewModel.AvailableRoles)
            {
                if (int.TryParse(item.Value, out int roleIdValue))
                {
                    item.Selected = viewModel.SelectedRoleIds.Contains(roleIdValue);
                }
            }
            viewModel.AvailableClaimDefinitions = await _claimDefinitionService.GetAllClaimDefinitionsAsync()
                                                                             .ContinueWith(t => t.Result.Select(cd => new SelectListItem
                                                                             {
                                                                                 Value = cd.Id.ToString(),
                                                                                 Text = $"{cd.Type}: {cd.Value} - {cd.Description}",
                                                                                 Selected = viewModel.SelectedClaimDefinitionIds.Contains(cd.Id)
                                                                             }).ToList());
            return View(viewModel);
        }

        var updateResult = await _userService.UpdateUserAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật người dùng thành công.", ToastType.Success)
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
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật người dùng '{viewModel.Email}'.", ToastType.Error)
            );
            viewModel.AvailableRoles = await _roleService.GetAllRolesAsSelectListAsync();
            foreach (var item in viewModel.AvailableRoles)
            {
                if (int.TryParse(item.Value, out int roleIdValue))
                {
                    item.Selected = viewModel.SelectedRoleIds.Contains(roleIdValue);
                }
            }
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

    // POST: Admin/User/Delete/5
    [Authorize(Policy = PermissionConstants.UserManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _userService.DeleteUserAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa người dùng thành công.", ToastType.Success)
            );
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa người dùng.", ToastType.Error)
            );
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/User/ToggleActive/5
    [Authorize(Policy = PermissionConstants.UserManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id, bool isActive)
    {
        var result = await _userService.ToggleUserActiveStatusAsync(id, isActive);
        if (result.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", result.Message ?? "Cập nhật trạng thái người dùng thành công.", ToastType.Success)
            );
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", result.Message ?? "Không thể cập nhật trạng thái người dùng.", ToastType.Error)
            );
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/User/ToggleLockout/5
    [Authorize(Policy = PermissionConstants.UserManage)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleLockout(int id, bool lockAccount)
    {
        var result = await _userService.ToggleUserLockoutAsync(id, lockAccount);
        if (result.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", result.Message ?? "Cập nhật trạng thái khóa tài khoản thành công.", ToastType.Success)
            );
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", result.Message ?? "Không thể cập nhật trạng thái khóa tài khoản.", ToastType.Error)
            );
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/User/ResetPassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(UserChangePasswordViewModel viewModel)
    {
        if (viewModel.UserId == 0)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
               new ToastData("Lỗi", "ID người dùng không hợp lệ.", ToastType.Error)
           );
            return RedirectToAction(nameof(Index));
        }

        var result = await _userChangePasswordViewModelValidator.ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Mật khẩu không hợp lệ. Vui lòng kiểm tra lại.", ToastType.Error)
            );
            return RedirectToAction(nameof(Edit), new { id = viewModel.UserId });
        }

        var resetResult = await _userService.ResetUserPasswordAsync(viewModel);

        if (resetResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", resetResult.Message ?? "Đặt lại mật khẩu thành công.", ToastType.Success)
            );
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", resetResult.Message ?? "Không thể đặt lại mật khẩu.", ToastType.Error)
            );
        }
        return RedirectToAction(nameof(Edit), new { id = viewModel.UserId });
    }
}