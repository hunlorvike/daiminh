using application.Interfaces;
using AutoMapper;
using domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Setting;
using web.Areas.Admin.Requests.Setting;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class SettingController(
    ISettingService settingService,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class SettingController
{
    public async Task<IActionResult> Index()
    {
        var setting = await settingService.GetAllAsync();
        List<SettingViewModel> models = _mapper.Map<List<SettingViewModel>>(setting);
        return View(models);
    }


    [AjaxOnly]
    public IActionResult Create()
    {
        return PartialView("_Create.Modal", new SettingCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await settingService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<SettingUpdateRequest>(response);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Details(int id)
    {
        var setting = await settingService.GetByIdAsync(id);
        if (setting == null) return NotFound();
        var settingDetail = _mapper.Map<SettingViewModel>(setting);
        return PartialView("_Detail.Modal", settingDetail);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await settingService.GetByIdAsync(id);
        if (response == null) return NotFound();
        var request = _mapper.Map<SettingDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }
}

public partial class SettingController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SettingCreateRequest model)
    {
        var validator = GetValidator<SettingCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var newSetting = _mapper.Map<Setting>(model);
            var response = await settingService.AddAsync(newSetting);

            switch (response)
            {
                case SuccessResponse<Setting> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Setting", new { area = "Admin" })
                    });
                case SuccessResponse<Setting> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Setting", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return PartialView("_Create.Modal", model);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SettingUpdateRequest model)
    {
        var validator = GetValidator<SettingUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var setting = await settingService.GetByIdAsync(model.Id);
            if (setting == null) return NotFound();

            _mapper.Map(model, setting);

            var response = await settingService.UpdateAsync(model.Id, setting);

            switch (response)
            {
                case SuccessResponse<Setting> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Setting", new { area = "Admin" })
                    });
                case SuccessResponse<Setting> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Setting", new { area = "Admin" });
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
            if (Request.IsAjaxRequest())
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });

            ModelState.AddModelError("", ex.Message);
            return PartialView("_Edit.Modal", model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(SettingDeleteRequest model)
    {
        try
        {
            var response = await settingService.DeleteAsync(model.Id);

            switch (response)
            {
                case SuccessResponse<Setting> successResponse when Request.IsAjaxRequest():
                    return Json(new
                    {
                        success = true,
                        message = successResponse.Message,
                        redirectUrl = Url.Action("Index", "Setting", new { area = "Admin" })
                    });
                case SuccessResponse<Setting> successResponse:
                    TempData["SuccessMessage"] = successResponse.Message;
                    return RedirectToAction("Index", "Setting", new { area = "Admin" });
                case ErrorResponse errorResponse when Request.IsAjaxRequest():
                    return BadRequest(errorResponse);
                case ErrorResponse errorResponse:
                    {
                        return BadRequest(errorResponse);
                    }
            }

            return PartialView("_Delete.Modal", model);
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
}