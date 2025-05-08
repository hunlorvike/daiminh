using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
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
using web.Areas.Admin.Validators.User;
using web.Areas.Admin.ViewModels.User;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<UserController> logger,
        IPasswordHasher<User> passwordHasher)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    // GET: Admin/User
    public async Task<IActionResult> Index(UserFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new UserFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IQueryable<User> query = _context.Set<User>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(u => u.Username.ToLower().Contains(lowerSearchTerm) ||
                                     u.Email.ToLower().Contains(lowerSearchTerm) ||
                                     (u.FullName != null && u.FullName.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(u => u.IsActive == filter.IsActive.Value);
        }

        query = query.OrderBy(u => u.Username);

        IPagedList<UserListItemViewModel> usersPaged = await query
            .ProjectTo<UserListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusSelectList(filter.IsActive);

        UserIndexViewModel viewModel = new()
        {
            Users = usersPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/User/Create
    public IActionResult Create()
    {
        UserCreateViewModel viewModel = new()
        {
            IsActive = true
        };
        return View(viewModel);
    }

    // POST: Admin/User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel viewModel)
    {
        var result = await new UserCreateViewModelValidator(_context).ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var user = _mapper.Map<User>(viewModel);
        user.PasswordHash = _passwordHasher.HashPassword(user, viewModel.Password);

        _context.Add(user);
        try
        {
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Thêm người dùng '{user.Username}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo người dùng: {Username}", user.Username);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi tạo người dùng.");
        }

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể thêm người dùng '{viewModel.Username}'.", ToastType.Error)
        );
        return View(viewModel);
    }

    // GET: Admin/User/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        User? user = await _context.Set<User>()
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            _logger.LogWarning("User not found for editing: ID {Id}", id);
            return NotFound();
        }

        UserEditViewModel viewModel = _mapper.Map<UserEditViewModel>(user);
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

        var result = await new UserEditViewModelValidator(_context).ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var currentUserId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int uid) ? uid : 0;

        var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy người dùng để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        if (!viewModel.IsActive && (user.Id == currentUserId || user.Id == 1))
        {
            ModelState.AddModelError(nameof(viewModel.IsActive), "Không thể hủy kích hoạt chính mình hoặc tài khoản quản trị viên chính.");
            return View(viewModel);
        }

        _mapper.Map(viewModel, user);

        try
        {
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Cập nhật người dùng '{user.Username}' thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật người dùng: {Username}", user.Username);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi cập nhật người dùng.");
        }

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Lỗi", $"Không thể cập nhật người dùng '{viewModel.Username}'.", ToastType.Error)
        );
        return View(viewModel);
    }

    // POST: Admin/User/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int? currentUserId = int.TryParse(currentUserIdString, out int uid) ? uid : (int?)null;

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

        User? user = await _context.Set<User>().FindAsync(id);
        if (user == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy người dùng.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy người dùng." });
        }

        try
        {
            string username = user.Username;
            _context.Remove(user);
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa người dùng '{username}' thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = $"Xóa người dùng '{username}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa người dùng: {Username}", user.Username);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", $"Không thể xóa người dùng '{user.Username}'.", ToastType.Error)
            );
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa người dùng." });
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
}