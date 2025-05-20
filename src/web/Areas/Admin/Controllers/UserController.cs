using AutoMapper;
using domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Security.Claims;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")]
public partial class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    private readonly IValidator<UserCreateViewModel> _userCreateViewModelValidator;
    private readonly IValidator<UserEditViewModel> _userEditViewModelValidator;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;


    public UserController(
        IUserService userService,
        IMapper mapper,
        ILogger<UserController> logger,
        IValidator<UserCreateViewModel> userCreateViewModelValidator,
        IValidator<UserEditViewModel> userEditViewModelValidator,
        RoleManager<Role> roleManager,
        UserManager<User> userManager
        )
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userCreateViewModelValidator = userCreateViewModelValidator ?? throw new ArgumentNullException(nameof(userCreateViewModelValidator));
        _userEditViewModelValidator = userEditViewModelValidator ?? throw new ArgumentNullException(nameof(userEditViewModelValidator));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    // GET: Admin/User
    public async Task<IActionResult> Index(UserFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new UserFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<UserListItemViewModel> usersPaged = await _userService.GetPagedUsersAsync(filter, pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusSelectList(filter.IsActive);

        UserIndexViewModel viewModel = new()
        {
            Users = usersPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/User/Create
    public async Task<IActionResult> CreateAsync()
    {
        UserCreateViewModel viewModel = new()
        {
            IsActive = true,
            AllRoles = await GetAllRolesAsSelectListItemsAsync()
        };
        return View(viewModel);
    }

    // POST: Admin/User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel viewModel)
    {
        var result = await _userCreateViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.AllRoles = await GetAllRolesAsSelectListItemsAsync();
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
            {
                if (error.Contains("Tên đăng nhập", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.UserName), error);
                }
                else if (error.Contains("Email", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Email), error);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            if (!createResult.Errors.Any() && !string.IsNullOrEmpty(createResult.Message))
            {
                ModelState.AddModelError(string.Empty, createResult.Message);
            }

            viewModel.AllRoles = await GetAllRolesAsSelectListItemsAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm người dùng '{viewModel.UserName}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // GET: Admin/User/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        UserEditViewModel? viewModel = await _userService.GetUserByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("User not found for editing: ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy người dùng.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }

        viewModel.AllRoles = await GetAllRolesAsSelectListItemsAsync();
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            viewModel.SelectedRoles = (await _userManager.GetRolesAsync(user)).ToList();
        }
        return View(viewModel);
    }

    // POST: Admin/User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _userEditViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.AllRoles = await GetAllRolesAsSelectListItemsAsync();
            return View(viewModel);
        }

        var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int currentUserId = int.TryParse(currentUserIdString, out int uid) ? uid : 0;


        var updateResult = await _userService.UpdateUserAsync(viewModel, currentUserId);

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
            {
                if (error.Contains("Tên đăng nhập", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.UserName), error);
                }
                else if (error.Contains("Email", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Email), error);
                }
                else if (error.Contains("hủy kích hoạt", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.IsActive), error);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }

            viewModel.AllRoles = await GetAllRolesAsSelectListItemsAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật người dùng '{viewModel.UserName}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // POST: Admin/User/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int currentUserId = int.TryParse(currentUserIdString, out int uid) ? uid : 0;

        if (id == currentUserId)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Bạn không thể xóa tài khoản của chính mình.", ToastType.Error)
            );
            return Json(new { success = false, message = "Bạn không thể xóa tài khoản của chính mình." });
        }

        if (id == 1)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không thể xóa tài khoản quản trị viên chính.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không thể xóa tài khoản quản trị viên chính." });
        }


        var deleteResult = await _userService.DeleteUserAsync(id, currentUserId);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa người dùng thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = deleteResult.Message });
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa người dùng.", ToastType.Error)
            );
            return Json(new { success = false, message = deleteResult.Message });
        }
    }
}

public partial class UserController
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

    private async Task<List<SelectListItem>> GetAllRolesAsSelectListItemsAsync()
    {
        var roles = await _roleManager.Roles
                                    .AsNoTracking()
                                    .Select(r => new SelectListItem
                                    {
                                        Value = r.Name,
                                        Text = r.Name
                                    }).ToListAsync();
        return roles;
    }
}
