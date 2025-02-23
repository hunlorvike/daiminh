using core.Attributes;
using core.Common.Constants;
using core.Common.Extensions;
using Core.Common.Models;
using core.Entities;
using core.Interfaces.Service;
using core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Account;
using web.Areas.Admin.Requests.Account;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class AccountController(
    IUserService userService,
    RoleService roleService,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(serviceProvider, configuration)
{
    public async Task<IActionResult> Index()
    {
        var response = await userService.GetAllAsync();

        var userViewModels = response.Select(r => new UserViewModel
        {
            Id = r.Id,
            Email = r.Email,
            RoleName = r.Role?.Name,
            CreatedAt = r.CreatedAt
        }).ToList();
        return View(userViewModels);
    }


    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await userService.GetByIdAsync(id);

        if (user == null) return NotFound();
        var viewModel = new UserRequest
        {
            Id = user.Id,
            RoleId = user.RoleId ?? throw new ArgumentNullException(nameof(user.RoleId))
        };

        ViewBag.Roles = await GetRoleOptionsAsync(viewModel.RoleId);

        return PartialView("_Edit.Modal", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserRequest model)
    {
        var validator = GetValidator<UserRequest>();

        if (await this.ValidateAndReturnView(validator, model))
        {
            ViewBag.Roles = await GetRoleOptionsAsync(model.RoleId);
            return PartialView("_Edit.Modal", model);
        }

        try
        {
            var user = await userService.GetByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.RoleId = model.RoleId;
            var response = await userService.UpdateAsync(model.Id, user);

            switch (response)
            {
                case SuccessResponse<User> successResponse:
                    if (Request.IsAjaxRequest())
                        return Json(new { redirectUrl = Url.Action("Index", "Account", new { area = "Admin" }) });

                    return RedirectToAction("Index", "Account", new { area = "Admin" });

                case ErrorResponse errorResponse:
                    foreach (var error in errorResponse.Errors) ModelState.AddModelError(error.Key, error.Value);

                    break;

                default:
                    ModelState.AddModelError("", "An unexpected error occurred.");
                    break;
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred: " + ex.Message);
        }

        ViewBag.Roles = await GetRoleOptionsAsync(model.RoleId);
        return PartialView("_Edit.Modal", model);
    }

    private async Task<List<SelectListItem>> GetRoleOptionsAsync(int selectedRoleId)
    {
        var roles = await roleService.GetAllAsync();
        return roles.Select(r => new SelectListItem
        {
            Text = r.Name,
            Value = r.Id.ToString(),
            Selected = r.Id == selectedRoleId
        }).ToList();
    }
}