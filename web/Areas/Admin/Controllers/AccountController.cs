using core.Common.Constants;
using infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.Models.Account;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class AccountController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public AccountController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _dbContext.Users
            .Include(u => u.Role)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                RoleName = u.Role!.Name,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _dbContext.Users.Where(u => u.Id == id)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                RoleName = u.Role!.Name
            }).FirstOrDefaultAsync();

        if (user == null) return NotFound();

        var roles = await _dbContext.Roles.Select(r => new SelectListItem
        {
            Text = r.Name,
            Value = r.Name,
            Selected = r.Name == user.RoleName
        }).ToListAsync();

        ViewBag.Roles = roles;

        return PartialView("_EditUser.Modal", user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserViewModel model)
    {
        if (id != model.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            var roles = await _dbContext.Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = r.Name == model.RoleName
                })
                .ToListAsync();

            ViewBag.Roles = roles;
            return PartialView("_EditUser.Modal", model);
        }

        var user = await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound();

        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == model.RoleName);
        if (role == null)
        {
            ModelState.AddModelError("RoleName", "Vai trò không tồn tại");
            return PartialView("_EditUser.Modal", model);
        }

        user.Role = role;

        await _dbContext.SaveChangesAsync();
        return RedirectToAction("Index", "Account", new { area = "Admin" });
    }
}