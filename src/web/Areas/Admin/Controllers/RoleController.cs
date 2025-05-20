using AutoMapper;
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
[Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;
    private readonly ILogger<RoleController> _logger;
    private readonly IValidator<RoleViewModel> _roleValidator;

    public RoleController(
        IRoleService roleService,
        IMapper mapper,
        ILogger<RoleController> logger,
        IValidator<RoleViewModel> roleValidator)
    {
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _roleValidator = roleValidator ?? throw new ArgumentNullException(nameof(roleValidator));
    }

    // GET: Admin/Role
    public async Task<IActionResult> Index(string? searchTerm, int page = 1, int pageSize = 15)
    {
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<RoleListItemViewModel> rolesPaged = await _roleService.GetPagedRolesAsync(searchTerm, pageNumber, currentPageSize);

        RoleIndexViewModel viewModel = new()
        {
            Roles = rolesPaged,
            SearchTerm = searchTerm
        };

        return View(viewModel);
    }

    // GET: Admin/Role/Create
    public IActionResult Create()
    {
        var viewModel = new RoleViewModel();
        return View(viewModel);
    }

    // POST: Admin/Role/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoleViewModel viewModel)
    {
        var validationResult = await _roleValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors) ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(viewModel);
        }

        if (await _roleService.RoleNameExistsAsync(viewModel.Name))
        {
            ModelState.AddModelError(nameof(viewModel.Name), "Tên vai trò đã tồn tại.");
            return View(viewModel);
        }

        var result = await _roleService.CreateRoleAsync(viewModel);

        if (result.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", result.Message ?? "Tạo vai trò thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            ModelState.AddModelError(string.Empty, result.Message ?? "Không thể tạo vai trò.");
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", result.Message ?? "Không thể tạo vai trò.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // GET: Admin/Role/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await _roleService.GetRoleByIdAsync(id);

        if (viewModel == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy vai trò.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    // POST: Admin/Role/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RoleViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
               new ToastData("Lỗi", "Yêu cầu không hợp lệ.", ToastType.Error)
           );
            return RedirectToAction(nameof(Index));
        }

        var validationResult = await _roleValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors) ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(viewModel);
        }

        if (await _roleService.RoleNameExistsAsync(viewModel.Name, viewModel.Id))
        {
            ModelState.AddModelError(nameof(viewModel.Name), "Tên vai trò đã tồn tại.");
            return View(viewModel);
        }

        var result = await _roleService.UpdateRoleAsync(viewModel);

        if (result.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", result.Message ?? "Cập nhật vai trò thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            ModelState.AddModelError(string.Empty, result.Message ?? "Không thể cập nhật vai trò.");
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", result.Message ?? "Không thể cập nhật vai trò.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // POST: Admin/Role/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        if (role != null && role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase) && id == 1)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
               new ToastData("Lỗi", "Không thể xóa vai trò Quản trị viên gốc.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _roleService.DeleteRoleAsync(id);

        if (result.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", result.Message ?? "Xóa vai trò thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", result.Message ?? "Không thể xóa vai trò.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }
}