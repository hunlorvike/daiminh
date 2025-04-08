using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using X.PagedList.EF;
using FluentValidation;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<UserController> _logger;

    public UserController(ApplicationDbContext context, IMapper mapper, IPasswordHasher<User> passwordHasher,
        ILogger<UserController> logger)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _logger = logger;
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
        if (ModelState.IsValid)
        {
            try
            {
                var user = _mapper.Map<User>(viewModel);

                user.PasswordHash = _passwordHasher.HashPassword(user, viewModel.Password);

                _context.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User {Username} created successfully.", user.Username);
                TempData["success"] = "Tạo người dùng mới thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user {Username}.", viewModel.Username);
                ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình tạo người dùng. Vui lòng thử lại.");
            }
        }

        ViewData["Title"] = "Thêm người dùng - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm người dùng mới";
        return View(viewModel);
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

        if (ModelState.IsValid)
        {
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
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {Username} (ID: {UserId}).", viewModel.Username, viewModel.Id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật. Vui lòng thử lại.");
            }
        }

        ViewData["Title"] = $"Chỉnh sửa người dùng {viewModel.Username} - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa người dùng: {viewModel.Username}";
        return View(viewModel);
    }
}