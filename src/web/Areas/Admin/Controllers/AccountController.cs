using application.Interfaces;
using AutoMapper;
using domain.Constants;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Account;
using web.Areas.Admin.Requests.Account;
using web.Attributes;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class AccountController(
    IUserService userService,
    IRoleService roleService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class AccountController
{
    public async Task<IActionResult> Index()
    {
        var users = await userService.GetAllAsync();
        List<UserViewModel> userViewModels = _mapper.Map<List<UserViewModel>>(users);
        return View(userViewModels);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        var viewModel = _mapper.Map<UserRequest>(user);
        ViewBag.Roles = await GetRoleOptionsAsync(viewModel.RoleId);
        return PartialView("_Edit.Modal", viewModel);
    }
}

public partial class AccountController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserRequest model)
    {
        var validator = GetValidator<UserRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var user = await userService.GetByIdAsync(model.Id);
            if (user == null) return NotFound();

            _mapper.Map(model, user);

            var response = await userService.UpdateAsync(model.Id, user);

            switch (response)
            {
                case SuccessResponse<User> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Account", new { area = "Admin" })
                    });
                case SuccessResponse<User> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Account", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return PartialView("_Edit.Modal", model);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Errors = ex.Message
            });
        }
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