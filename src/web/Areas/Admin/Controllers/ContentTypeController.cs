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
            .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);

        if (contentType == null) return NotFound();
        var request = _mapper.Map<ContentTypeUpdateRequest>(contentType);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var contentType = await dbContext.ContentTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);

        if (contentType == null) return NotFound();
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

            // Kiểm tra tính duy nhất của Slug
            var existingContentType = await dbContext.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Slug == newContentType.Slug && ct.DeletedAt == null);
            if (existingContentType != null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

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
            return BadRequest(new
            {
                Success = false,
                Errors = ex.Message
            });
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

            if (contentType == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Loại nội dung không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            // Kiểm tra tính duy nhất của Slug
            var existingContentType = await dbContext.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.Id != model.Id && ct.DeletedAt == null);
            if (existingContentType != null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

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
    public async Task<IActionResult> Delete(ContentTypeDeleteRequest model)
    {
        try
        {
            var contentType = await dbContext.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Id == model.Id && ct.DeletedAt == null);

            if (contentType == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Loại nội dung không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            dbContext.ContentTypes.Remove(contentType);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ContentType>(contentType, "Xóa loại nội dung thành công (đã ẩn).");

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
            return BadRequest(new
            {
                Success = false,
                Errors = ex.Message
            });
        }
    }
}