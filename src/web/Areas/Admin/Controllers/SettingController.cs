using AutoMapper;

using domain.Entities;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;

using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.Setting;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class SettingController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache)
{
    public async Task<IActionResult> Index()
    {
        var settings = await dbContext.Settings
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .ToListAsync();

        return View(settings);
    }

    [AjaxOnly]
    public IActionResult Create()
    {
        return PartialView("_Create.Modal", new SettingCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var setting = await dbContext.Settings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (setting == null) return NotFound();
        var request = _mapper.Map<SettingUpdateRequest>(setting);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Details(int id)
    {
        var setting = await dbContext.Settings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (setting == null) return NotFound();
        return PartialView("_Detail.Modal", setting);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var setting = await dbContext.Settings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null);

        if (setting == null) return NotFound();
        var request = _mapper.Map<SettingDeleteRequest>(setting);
        return PartialView("_Delete.Modal", request);
    }

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
            await dbContext.Settings.AddAsync(newSetting);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Setting>(newSetting, "Thêm cài đặt mới thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Setting", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Setting", new { area = "Admin" });
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
            var setting = await dbContext.Settings
                .FirstOrDefaultAsync(s => s.Id == model.Id && s.DeletedAt == null);

            if (setting == null) return NotFound();

            _mapper.Map(model, setting);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Setting>(setting, "Cập nhật cài đặt thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Setting", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Setting", new { area = "Admin" });
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
    public async Task<IActionResult> Delete(SettingDeleteRequest model)
    {
        try
        {
            var setting = await dbContext.Settings
                .FirstOrDefaultAsync(s => s.Id == model.Id && s.DeletedAt == null);

            if (setting == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Cài đặt không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            dbContext.Settings.Remove(setting);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Setting>(setting, "Xóa cài đặt thành công (đã ẩn).");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Setting", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Setting", new { area = "Admin" });
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