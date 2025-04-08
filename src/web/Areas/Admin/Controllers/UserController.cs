using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using X.PagedList.EF;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<UserController> _logger;
    private readonly IValidator<UserCreateViewModel> _userCreateValidator;
    private readonly IValidator<UserEditViewModel> _userEditValidator;

    public UserController(ApplicationDbContext context, IMapper mapper, IPasswordHasher<User> passwordHasher,
        ILogger<UserController> logger, IValidator<UserCreateViewModel> createValidator, IValidator<UserEditViewModel> editValidator)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _userCreateValidator = createValidator;
        _userEditValidator = editValidator;
    }

    // GET: Admin/Users
    public async Task<IActionResult> Index(string? searchTerm, int page = 1, int pageSize = 10)
    {
        ViewBag.SearchTerm = searchTerm;
        int pageNumber = page;

        var query = _context.Set<User>().AsNoTracking();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(u => u.Username.Contains(searchTerm) || u.Email.Contains(searchTerm));
        }

        var users = await query.OrderByDescending(u => u.CreatedAt)
                                 .ProjectTo<UserListItemViewModel>(_mapper.ConfigurationProvider)
                                 .ToPagedListAsync(pageNumber, pageSize);

        ViewData["Title"] = "Quản lý người dùng";
        ViewData["PageTitle"] = "Danh sách người dùng";

        return View(users);
    }

    // GET: Admin/Users/Create
    public IActionResult Create()
    {
        ViewData["Title"] = "Thêm người dùng - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm người dùng mới";
        return View(new UserCreateViewModel());
    }

    // POST: Admin/Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel viewModel)
    {
        var validationResult = await _userCreateValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        try
        {
            var user = _mapper.Map<User>(viewModel);

            user.PasswordHash = _passwordHasher.HashPassword(user, viewModel.Password);

            _context.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User {Username} created successfully.", user.Username);
            TempData["success"] = "Tạo người dùng mới thành công.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user {Username}.", viewModel.Username);
            ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình tạo người dùng. Vui lòng thử lại.");
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Users/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Set<User>().FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<UserEditViewModel>(user);
        ViewData["Title"] = $"Chỉnh sửa người dùng {user.Username} - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa người dùng: {user.Username}";
        return View(viewModel);
    }

    // POST: Admin/Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _userEditValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            ViewData["Title"] = $"Chỉnh sửa người dùng {viewModel.Username} - Hệ thống quản trị";
            ViewData["PageTitle"] = $"Chỉnh sửa người dùng: {viewModel.Username}";
            return View(viewModel);
        }

        try
        {
            var userToUpdate = await _context.Set<User>().FindAsync(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            _mapper.Map(viewModel, userToUpdate);

            _context.Update(userToUpdate);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User {Username} (ID: {UserId}) updated successfully.", userToUpdate.Username, userToUpdate.Id);
            TempData["success"] = "Cập nhật thông tin người dùng thành công.";
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error updating user {Username} (ID: {UserId}).", viewModel.Username, viewModel.Id);

            if (!await _context.Set<User>().AnyAsync(u => u.Id == id))
            {
                return NotFound();
            }
            else
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin người dùng. Vui lòng thử lại.");
                return RedirectToAction(nameof(Index));
            }
        }

        return RedirectToAction(nameof(Index));
    }


    //// POST: Admin/Users/Delete/5
    //[HttpPost]
    //[ValidateAntiForgeryToken] // Cân nhắc dùng token trong header cho AJAX
    //public async Task<IActionResult> Delete(int id)
    //{
    //    // !! CẢNH BÁO: Không nên cho phép xóa user trực tiếp nếu có dữ liệu liên quan quan trọng
    //    // Cân nhắc sử dụng soft delete (thêm cột IsDeleted) thay vì xóa cứng.
    //    // Đoạn code này thực hiện xóa cứng để giống với logic Tag của bạn.

    //    var user = await _context.Set<User>().FindAsync(id);
    //    if (user == null)
    //    {
    //        _logger.LogWarning("Attempted to delete non-existent user with ID: {UserId}", id);
    //        return Json(new { success = false, message = "Không tìm thấy người dùng." });
    //    }

    //    // Thêm kiểm tra: không cho xóa user admin mặc định, hoặc user đang đăng nhập
    //    if (user.Username.Equals("admin", StringComparison.OrdinalIgnoreCase) || user.Username == User.FindFirstValue(ClaimTypes.Name))
    //    {
    //        _logger.LogWarning("Attempted to delete protected user: {Username} (ID: {UserId}) by user {CurrentUser}", user.Username, user.Id, User.FindFirstValue(ClaimTypes.Name));
    //        return Json(new { success = false, message = "Bạn không thể xóa người dùng này." });
    //    }

    //    try
    //    {
    //        _context.Users.Remove(user);
    //        await _context.SaveChangesAsync();
    //        _logger.LogInformation("User {Username} (ID: {UserId}) deleted successfully by {CurrentUser}.", user.Username, user.Id, User.FindFirstValue(ClaimTypes.Name));
    //        // TempData["success"] = $"Đã xóa người dùng {user.Username} thành công."; // Dùng với Redirect
    //        return Json(new { success = true, message = $"Đã xóa người dùng {user.Username} thành công." });
    //    }
    //    catch (Exception ex) // Bắt lỗi nếu có ràng buộc khóa ngoại không cho xóa
    //    {
    //        _logger.LogError(ex, "Error deleting user {Username} (ID: {UserId}). Might be due to foreign key constraints.", user.Username, user.Id);
    //        return Json(new { success = false, message = $"Không thể xóa người dùng {user.Username}. Có thể do người dùng này có dữ liệu liên quan." });
    //    }
    //}
}