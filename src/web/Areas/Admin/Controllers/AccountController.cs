using AutoMapper;

using domain.Entities;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;

using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.Account;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class AccountController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration)
{
    public async Task<IActionResult> Index()
    {
        var users = await dbContext.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .ToListAsync();

        return View(users);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);

        if (user == null) return NotFound();

        var viewModel = _mapper.Map<UserRequest>(user);
        ViewBag.Roles = await GetRoleOptionsAsync(user.RoleId);
        return PartialView("_Edit.Modal", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserRequest model)
    {
        var validator = GetValidator<UserRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            ViewBag.Roles = await GetRoleOptionsAsync(model.RoleId);
            return result;
        }

        try
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == model.Id && u.DeletedAt == null);

            if (user == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Người dùng không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            var role = await dbContext.Roles
                .FirstOrDefaultAsync(r => r.Id == model.RoleId && r.DeletedAt == null);
            if (role == null)
            {
                var errors = new Dictionary<string, string[]>
                    {
                        { nameof(model.RoleId), ["Vai trò không tồn tại hoặc đã bị xóa."] }
                    };
                return BadRequest(new ErrorResponse(errors));
            }

            user.RoleId = model.RoleId;

            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<User>(user, "Cập nhật thông tin người dùng thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Account", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Account", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            if (Request.IsAjaxRequest())
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });

            ViewBag.Roles = await GetRoleOptionsAsync(model.RoleId);
            ModelState.AddModelError("", ex.Message);
            return PartialView("_Edit.Modal", model);
        }
    }

    private async Task<List<SelectListItem>> GetRoleOptionsAsync(int? selectedRoleId)
    {
        var roles = await dbContext.Roles
            .AsNoTracking()
            .Where(r => r.DeletedAt == null)
            .ToListAsync();

        return roles.Select(r => new SelectListItem
        {
            Text = r.Name,
            Value = r.Id.ToString(),
            Selected = r.Id == selectedRoleId
        }).ToList();
    }
}