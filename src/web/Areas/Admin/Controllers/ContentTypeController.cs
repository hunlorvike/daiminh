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
using web.Areas.Admin.Requests.ContentType;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class ContentTypeController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache)
{
    public async Task<IActionResult> Index()
    {
        var contentTypes = await dbContext.ContentTypes
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .ToListAsync();

        return View(contentTypes);
    }

    [AjaxOnly]
    public IActionResult Create()
    {
        return PartialView("_Create.Modal", new ContentTypeCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var contentType = await dbContext.ContentTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null) ?? throw new NotFoundException("Content type not found.");
        var request = _mapper.Map<ContentTypeUpdateRequest>(contentType);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var contentType = await dbContext.ContentTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null) ?? throw new NotFoundException("Content type not found.");
        var request = _mapper.Map<ContentTypeDeleteRequest>(contentType);
        return PartialView("_Delete.Modal", request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContentTypeCreateRequest model)
    {
        var validator = GetValidator<ContentTypeCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var newContentType = _mapper.Map<ContentType>(model);
            await dbContext.ContentTypes.AddAsync(newContentType);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ContentType>(newContentType, "Thêm loại nội dung mới thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ContentType", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ContentType", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error creating content type.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ContentTypeUpdateRequest model)
    {
        var validator = GetValidator<ContentTypeUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var contentType = await dbContext.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Id == model.Id && ct.DeletedAt == null);

            if (contentType == null) return NotFound();

            _mapper.Map(model, contentType);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ContentType>(contentType, "Cập nhật loại nội dung thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ContentType", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ContentType", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error updating content type.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ContentTypeDeleteRequest model)
    {
        try
        {
            var contentType = await dbContext.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Id == model.Id && ct.DeletedAt == null);

            dbContext.ContentTypes.Remove(contentType!);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ContentType>(contentType!, "Xóa loại nội dung thành công (đã ẩn).");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ContentType", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ContentType", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error deleting content type.", ex);
        }
    }
}