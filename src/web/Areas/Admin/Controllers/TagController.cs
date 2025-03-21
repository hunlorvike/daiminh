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
            .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null) ?? throw new NotFoundException("Tag not found.");
        var request = _mapper.Map<TagUpdateRequest>(response);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await dbContext.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null) ?? throw new NotFoundException("Tag not found.");
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
            throw new SystemException2("Error creating tag.", ex);
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
            var existingTag = await dbContext.Tags
                .FirstOrDefaultAsync(t => t.Id == model.Id && t.DeletedAt == null);

            if (existingTag == null) return NotFound();

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
            throw new SystemException2("Error updating tag.", ex);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(TagDeleteRequest model)
    {
        try
        {
            Tag? tag = await dbContext.Tags
                .FirstOrDefaultAsync(t => t.Id == model.Id && t.DeletedAt == null);

            dbContext.Tags.Remove(tag!);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Tag>(tag!, "Xóa thẻ thành công (đã ẩn).");

            return Json(new
            {
                success = true,
                message = successResponse.Message,
                redirectUrl = Url.Action("Index", "Tag", new { area = "Admin" })
            });
        }
        catch (Exception ex)
        {
            throw new SystemException2("Error deleting tag.", ex);
        }
    }
}