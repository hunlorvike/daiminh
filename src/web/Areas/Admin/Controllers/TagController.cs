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
using web.Areas.Admin.Requests.Tag;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class TagController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache);

public partial class TagController
{
    public async Task<IActionResult> Index()
    {
        var tags = await dbContext.Tags
            .AsNoTracking()
            .ToListAsync();

        return View(tags);
    }

    [AjaxOnly]
    public IActionResult Create()
    {
        return PartialView("_Create.Modal", new TagCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await dbContext.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);

        if (response == null) return NotFound();
        var request = _mapper.Map<TagUpdateRequest>(response);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await dbContext.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);

        if (response == null) return NotFound();
        var request = _mapper.Map<TagDeleteRequest>(response);
        return PartialView("_Delete.Modal", request);
    }
}

public partial class TagController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TagCreateRequest model)
    {
        var validator = GetValidator<TagCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var newTag = _mapper.Map<Tag>(model);

            var existingTag = await dbContext.Tags
                .FirstOrDefaultAsync(t => t.Slug == newTag.Slug && t.DeletedAt == null);

            if (existingTag != null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."] }
                };

                return BadRequest(new ErrorResponse(errors));
            }

            await dbContext.Tags.AddAsync(newTag);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Tag>(newTag, "Thêm thẻ mới thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Tag", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Tag", new { area = "Admin" });
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
    public async Task<IActionResult> Edit(TagUpdateRequest model)
    {
        var validator = GetValidator<TagUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var existingSlug = await dbContext.Tags
                .FirstOrDefaultAsync(t => t.Slug == model.Slug && t.Id != model.Id && t.DeletedAt == null);

            if (existingSlug != null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."] }
                };

                return BadRequest(new ErrorResponse(errors));
            }

            var existingTag = await dbContext.Tags
                .FirstOrDefaultAsync(t => t.Id == model.Id && t.DeletedAt == null);

            if (existingTag == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Thẻ không tồn tại hoặc đã bị xóa."] }
                };

                return BadRequest(new ErrorResponse(errors));
            }

            _mapper.Map(model, existingTag);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Tag>(existingTag, "Cập nhật thẻ thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Tag", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Tag", new { area = "Admin" });
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
    public async Task<IActionResult> Delete(TagDeleteRequest model)
    {
        try
        {
            var tag = await dbContext.Tags
                .FirstOrDefaultAsync(t => t.Id == model.Id && t.DeletedAt == null);

            if (tag == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Thẻ không tồn tại hoặc đã bị xóa."] }
                };

                return BadRequest(new ErrorResponse(errors));
            }

            dbContext.Tags.Remove(tag);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Tag>(tag, "Xóa thẻ thành công (đã ẩn).");

            return Json(new
            {
                success = true,
                message = successResponse.Message,
                redirectUrl = Url.Action("Index", "Tag", new { area = "Admin" })
            });
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