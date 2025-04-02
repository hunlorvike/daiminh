using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using web.Areas.Admin.ViewModels.User;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<UserViewModel> _validator;

    public UserController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<UserViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/User
    public async Task<IActionResult> Index(string? searchTerm = null)
    {
        ViewData["PageTitle"] = "Quản lý người dùng";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Người dùng", "")
        };

        var query = _context.Set<User>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u => u.Username.Contains(searchTerm) || u.Email.Contains(searchTerm));
        }

        var users = await query
            .OrderBy(u => u.Username)
            .ToListAsync();

        var viewModels = _mapper.Map<List<UserListItemViewModel>>(users);

        ViewBag.SearchTerm = searchTerm;

        return View(viewModels);
    }

    // GET: Admin/User/Create
    public IActionResult Create()
    {
        ViewData["PageTitle"] = "Thêm người dùng mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Người dùng", "/Admin/User"),
            ("Thêm mới", "")
        };

        var viewModel = new UserViewModel();

        return View(viewModel);
    }

    // POST: Admin/User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        // Check if username already exists
        if (await _context.Set<User>().AnyAsync(u => u.Username == viewModel.Username))
        {
            ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
            return View(viewModel);
        }

        // Check if email already exists
        if (await _context.Set<User>().AnyAsync(u => u.Email == viewModel.Email))
        {
            ModelState.AddModelError("Email", "Email đã tồn tại");
            return View(viewModel);
        }

        var user = _mapper.Map<User>(viewModel);

        // Hash the password
        user.PasswordHash = HashPassword(viewModel.Password);

        _context.Add(user);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm người dùng thành công";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/User/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _context.Set<User>().FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa người dùng";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Người dùng", "/Admin/User"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<UserViewModel>(user);

        return View(viewModel);
    }

    // POST: Admin/User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            return View(viewModel);
        }

        // Check if username already exists (excluding current user)
        if (await _context.Set<User>().AnyAsync(u => u.Username == viewModel.Username && u.Id != id))
        {
            ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
            return View(viewModel);
        }

        // Check if email already exists (excluding current user)
        if (await _context.Set<User>().AnyAsync(u => u.Email == viewModel.Email && u.Id != id))
        {
            ModelState.AddModelError("Email", "Email đã tồn tại");
            return View(viewModel);
        }

        try
        {
            var user = await _context.Set<User>().FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.Username = viewModel.Username;
            user.Email = viewModel.Email;

            // Update password if provided
            if (!string.IsNullOrEmpty(viewModel.Password))
            {
                user.PasswordHash = HashPassword(viewModel.Password);
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật người dùng thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/User/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Set<User>().FindAsync(id);

        if (user == null)
        {
            return Json(new { success = false, message = "Không tìm thấy người dùng" });
        }

        // Prevent deleting the last admin user
        var userCount = await _context.Set<User>().CountAsync();
        if (userCount <= 1)
        {
            return Json(new { success = false, message = "Không thể xóa người dùng cuối cùng trong hệ thống" });
        }

        _context.Set<User>().Remove(user);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa người dùng thành công" });
    }

    private async Task<bool> UserExists(int id)
    {
        return await _context.Set<User>().AnyAsync(e => e.Id == id);
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}

