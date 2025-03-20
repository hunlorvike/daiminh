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
using shared.Enums;
using shared.Extensions;
using shared.Models;

using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.ContentFieldDefinition;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class ContentFieldDefinitionController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache)
{
    public async Task<IActionResult> Index()
    {
        var contentFieldDefinitions = await dbContext.ContentFieldDefinitions
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Include(x => x.ContentType)
            .ToListAsync();

        return View(contentFieldDefinitions);
    }

    [AjaxOnly]
    public async Task<IActionResult> Create(int? contentTypeId = null)
    {
        var model = new ContentFieldDefinitionCreateRequest();
        if (contentTypeId.HasValue) model.ContentTypeId = contentTypeId.Value;

        await PopulateContentTypeDropdown();
        PopulateFieldTypeDropdown();
        return PartialView("_Create.Modal", model);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var contentFieldDefinition = await dbContext.ContentFieldDefinitions
            .AsNoTracking()
            .FirstOrDefaultAsync(cfd => cfd.Id == id && cfd.DeletedAt == null);

        if (contentFieldDefinition == null) return NotFound();
        var request = _mapper.Map<ContentFieldDefinitionUpdateRequest>(contentFieldDefinition);

        await PopulateContentTypeDropdown();
        PopulateFieldTypeDropdown();
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var contentFieldDefinition = await dbContext.ContentFieldDefinitions
            .AsNoTracking()
            .FirstOrDefaultAsync(cfd => cfd.Id == id && cfd.DeletedAt == null);

        if (contentFieldDefinition == null) return NotFound();
        var request = _mapper.Map<ContentFieldDefinitionDeleteRequest>(contentFieldDefinition);
        return PartialView("_Delete.Modal", request);
    }

    private async Task PopulateContentTypeDropdown()
    {
        var contentTypes = await dbContext.ContentTypes
            .AsNoTracking()
            .Where(ct => ct.DeletedAt == null)
            .ToListAsync();
        ViewBag.ContentTypes = new SelectList(contentTypes, "Id", "Name");
    }

    private void PopulateFieldTypeDropdown()
    {
        var fieldTypes = Enum.GetValues(typeof(FieldType))
            .Cast<FieldType>()
            .Select(ft => new SelectListItem
            {
                Value = ft.ToString(),
                Text = ft.ToString()
            });
        ViewBag.FieldTypes = new SelectList(fieldTypes, "Value", "Text");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContentFieldDefinitionCreateRequest model)
    {
        var validator = GetValidator<ContentFieldDefinitionCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateContentTypeDropdown();
            PopulateFieldTypeDropdown();
            return result;
        }

        try
        {
            var newContentFieldDefinition = _mapper.Map<ContentFieldDefinition>(model);

            // Kiểm tra ContentTypeId hợp lệ
            var contentType = await dbContext.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Id == model.ContentTypeId && ct.DeletedAt == null);
            if (contentType == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.ContentTypeId), ["Loại nội dung không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            await dbContext.ContentFieldDefinitions.AddAsync(newContentFieldDefinition);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ContentFieldDefinition>(newContentFieldDefinition, "Thêm định nghĩa trường nội dung mới thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ContentFieldDefinition", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ContentFieldDefinition", new { area = "Admin" });
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
    public async Task<IActionResult> Edit(ContentFieldDefinitionUpdateRequest model)
    {
        var validator = GetValidator<ContentFieldDefinitionUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateContentTypeDropdown();
            PopulateFieldTypeDropdown();
            return result;
        }

        try
        {
            var contentFieldDefinition = await dbContext.ContentFieldDefinitions
                .FirstOrDefaultAsync(cfd => cfd.Id == model.Id && cfd.DeletedAt == null);

            if (contentFieldDefinition == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Định nghĩa trường nội dung không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            // Kiểm tra ContentTypeId hợp lệ
            var contentType = await dbContext.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Id == model.ContentTypeId && ct.DeletedAt == null);
            if (contentType == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.ContentTypeId), ["Loại nội dung không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            _mapper.Map(model, contentFieldDefinition);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ContentFieldDefinition>(contentFieldDefinition, "Cập nhật định nghĩa trường nội dung thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ContentFieldDefinition", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ContentFieldDefinition", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            if (Request.IsAjaxRequest())
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });

            await PopulateContentTypeDropdown();
            PopulateFieldTypeDropdown();
            ModelState.AddModelError("", ex.Message);
            return PartialView("_Edit.Modal", model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ContentFieldDefinitionDeleteRequest model)
    {
        try
        {
            var contentFieldDefinition = await dbContext.ContentFieldDefinitions
                .FirstOrDefaultAsync(cfd => cfd.Id == model.Id && cfd.DeletedAt == null);

            if (contentFieldDefinition == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Định nghĩa trường nội dung không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            dbContext.ContentFieldDefinitions.Remove(contentFieldDefinition);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<ContentFieldDefinition>(contentFieldDefinition, "Xóa định nghĩa trường nội dung thành công (đã ẩn).");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "ContentFieldDefinition", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "ContentFieldDefinition", new { area = "Admin" });
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