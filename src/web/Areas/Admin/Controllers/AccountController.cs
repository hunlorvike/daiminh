using AutoMapper;

using domain.Entities;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
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
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache)
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
            .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null) ?? throw new NotFoundException("User not found.");
        var viewModel = _mapper.Map<AccountUpdateRequestValidator>(user);
        ViewBag.Roles = await GetRoleOptionsAsync(user.RoleId);
        return PartialView("_Edit.Modal", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AccountUpdateRequest model)
    {
        var validator = GetValidator<AccountUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            User? user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == model.Id && u.DeletedAt == null);
            user!.RoleId = model.RoleId;
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
            throw new SystemException2("Error updating user.", ex);
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