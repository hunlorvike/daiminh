using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.User;
using X.PagedList.EF;
using System.Security.Claims;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<UserController> _logger;

    public UserController(ApplicationDbContext context, IMapper mapper, IPasswordHasher<User> passwordHasher, ILogger<UserController> logger)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    // GET: Admin/Users
    public async Task<IActionResult> Index(string? searchTerm = null, int page = 1, int pageSize = 15)
    {
        ViewData["Title"] = "Quản lý người dùng - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách người dùng";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Người dùng", Url.Action(nameof(Index)))
        };

        ViewBag.SearchTerm = searchTerm;
        int pageNumber = page;

        var query = _context.Set<User>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(u => u.Username.ToLower().Contains(lowerSearchTerm)
                                  || u.Email.ToLower().Contains(lowerSearchTerm));
        }

        var usersPaged = await query.OrderByDescending(u => u.CreatedAt)
                                    .ProjectTo<UserListItemViewModel>(_mapper.ConfigurationProvider)
                                    .ToPagedListAsync(pageNumber, pageSize);

        return View(usersPaged);
    }

    // GET: Admin/Users/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Thêm người dùng - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm người dùng mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Người dùng", Url.Action(nameof(Index))),
            ("Thêm mới", "")
        };
        return View(new UserCreateViewModel());
    }

    // POST: Admin/Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = _mapper.Map<User>(viewModel);

                user.PasswordHash = _passwordHasher.HashPassword(user, viewModel.Password);

                _context.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User '{Username}' created successfully by {AdminUser}.", user.Username, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Tạo người dùng '{user.Username}' thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error creating user '{Username}'. Check unique constraints.", viewModel.Username);
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: users.username", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError(nameof(viewModel.Username), "Tên đăng nhập này đã tồn tại.");
                }
                else if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: users.email", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError(nameof(viewModel.Email), "Địa chỉ email này đã được sử dụng.");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi tạo người dùng.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user '{Username}'.", viewModel.Username);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi tạo người dùng.");
            }
        }
        else
        {
            _logger.LogWarning("Failed to create user '{Username}'. Model state is invalid.", viewModel.Username);
        }

        ViewData["Title"] = "Thêm người dùng - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm người dùng mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Người dùng", Url.Action(nameof(Index))), ("Thêm mới", "") };
        return View(viewModel);
    }

    // GET: Admin/Users/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Edit GET: User ID is null.");
            return BadRequest("User ID is required.");
        }

        var user = await _context.Set<User>().AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            _logger.LogWarning("Edit GET: User with ID {UserId} not found.", id);
            return NotFound();
        }

        var viewModel = _mapper.Map<UserEditViewModel>(user);
        ViewData["Title"] = $"Chỉnh sửa người dùng - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa người dùng: {user.Username}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Người dùng", Url.Action(nameof(Index))),
            ($"Chỉnh sửa: {user.Username}", "")
        };
        return View(viewModel);
    }

    // POST: Admin/Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var userToUpdate = await _context.Set<User>().FindAsync(id);
            if (userToUpdate == null)
            {
                _logger.LogWarning("Edit POST: User with ID {UserId} not found for update.", id);
                TempData["error"] = "Không tìm thấy người dùng để cập nhật.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _mapper.Map(viewModel, userToUpdate);

                await _context.SaveChangesAsync();

                _logger.LogInformation("User '{Username}' (ID: {UserId}) updated successfully by {AdminUser}.", userToUpdate.Username, id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật thông tin người dùng '{userToUpdate.Username}' thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating user '{Username}' (ID: {UserId}). Check unique constraints.", viewModel.Username, id);
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: users.username", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError(nameof(viewModel.Username), "Tên đăng nhập này đã tồn tại.");
                }
                else if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: users.email", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError(nameof(viewModel.Email), "Địa chỉ email này đã được sử dụng.");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi cập nhật người dùng.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user '{Username}' (ID: {UserId}).", viewModel.Username, id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật.");
            }
        }
        else
        {
            _logger.LogWarning("Failed to update user '{Username}' (ID: {UserId}). Model state is invalid.", viewModel.Username, id);
        }

        ViewData["Title"] = $"Chỉnh sửa người dùng - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa người dùng: {viewModel.Username}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Người dùng", Url.Action(nameof(Index))), ($"Chỉnh sửa: {viewModel.Username}", "") };
        return View(viewModel);
    }


    // POST: Admin/User/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (id == 1)
        {
            _logger.LogWarning("Attempted to delete seeded admin user (ID: 1) by {AdminUser}.", User.Identity?.Name ?? "Unknown");
            return Json(new { success = false, message = "Không thể xóa người dùng quản trị viên mặc định." });
        }

        if (currentUserIdString != null && id.ToString() == currentUserIdString)
        {
            _logger.LogWarning("User {AdminUser} attempted to delete their own account (ID: {UserId}).", User.Identity?.Name ?? "Unknown", id);
            return Json(new { success = false, message = "Bạn không thể xóa tài khoản của chính mình." });
        }

        var user = await _context.Set<User>().FindAsync(id);
        if (user == null)
        {
            _logger.LogWarning("Delete POST: User with ID {UserId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy người dùng." });
        }

        try
        {
            string deletedUsername = user.Username;
            _context.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User '{DeletedUsername}' (ID: {UserId}) deleted successfully by {AdminUser}.", deletedUsername, id, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = $"Xóa người dùng '{deletedUsername}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user '{Username}' (ID: {UserId}).", user.Username, id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa người dùng." });
        }
    }
}
