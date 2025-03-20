using AutoMapper;

using domain.Entities;

using infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using shared.Attributes;
using shared.Constants;
using shared.Extensions;
using shared.Models;

using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Requests.Content;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}", AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class ContentController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration)
{
    public async Task<IActionResult> Index()
    {
        var contents = await dbContext.Contents
            .AsNoTracking()
            .Where(c => c.DeletedAt == null)
            .Include(c => c.ContentType)
            .Include(c => c.Author)
            .ToListAsync();

        return View(contents);
    }

    [AjaxOnly]
    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
        return PartialView("_Create.Modal", new ContentCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> GetContentTypeFields(int contentTypeId)
    {
        var fields = await dbContext.ContentFieldDefinitions
            .AsNoTracking()
            .Where(f => f.ContentTypeId == contentTypeId && f.DeletedAt == null)
            .ToListAsync();

        return Json(fields);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var content = await dbContext.Contents
            .AsNoTracking()
            .Include(c => c.ContentType)
            .Include(c => c.ContentCategories)
            .Include(c => c.ContentTags)
            .Include(c => c.FieldValues)
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (content == null) return NotFound();

        await PopulateDropdowns(content);
        var request = _mapper.Map<ContentUpdateRequest>(content);
        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var content = await dbContext.Contents
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (content == null) return NotFound();
        var request = _mapper.Map<ContentDeleteRequest>(content);
        return PartialView("_Delete.Modal", request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContentCreateRequest model)
    {
        var validator = GetValidator<ContentCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateDropdowns();
            return result;
        }

        try
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            var newContent = _mapper.Map<Content>(model);

            // Kiểm tra tính duy nhất của Slug
            var existingContent = await dbContext.Contents
                .FirstOrDefaultAsync(c => c.Slug == newContent.Slug && c.DeletedAt == null);
            if (existingContent != null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."] }
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

            // Kiểm tra AuthorId hợp lệ (nếu có)
            if (model.AuthorId.HasValue)
            {
                var author = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == model.AuthorId.Value);
                if (author == null)
                {
                    var errors = new Dictionary<string, string[]>
                    {
                        { nameof(model.AuthorId), ["Tác giả không tồn tại."] }
                    };
                    return BadRequest(new ErrorResponse(errors));
                }
            }

            // Add categories
            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                newContent.ContentCategories = model.CategoryIds.Select(categoryId => new ContentCategory
                {
                    CategoryId = categoryId
                }).ToList();
            }

            // Add tags
            if (model.TagIds != null && model.TagIds.Any())
            {
                newContent.ContentTags = model.TagIds.Select(tagId => new ContentTag
                {
                    TagId = tagId
                }).ToList();
            }

            // Add field values
            if (model.FieldValues != null && model.FieldValues.Any())
            {
                newContent.FieldValues = model.FieldValues.Select(kv => new ContentFieldValue
                {
                    FieldId = kv.Key,
                    Value = kv.Value
                }).ToList();
            }

            await dbContext.Contents.AddAsync(newContent);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var successResponse = new SuccessResponse<Content>(newContent, "Thêm nội dung mới thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Content", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Content", new { area = "Admin" });
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
    public async Task<IActionResult> Edit(ContentUpdateRequest model)
    {
        var validator = GetValidator<ContentUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            var content = await dbContext.Contents
                .AsNoTracking()
                .Include(c => c.ContentType)
                .Include(c => c.ContentCategories)
                .Include(c => c.ContentTags)
                .Include(c => c.FieldValues)
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.DeletedAt == null);
            await PopulateDropdowns(content);
            return result;
        }

        try
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            var existingContent = await dbContext.Contents
                .Include(c => c.ContentCategories)
                .Include(c => c.ContentTags)
                .Include(c => c.FieldValues)
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.DeletedAt == null);

            if (existingContent == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Nội dung không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            // Kiểm tra tính duy nhất của Slug
            var existingContentWithSlug = await dbContext.Contents
                .FirstOrDefaultAsync(c => c.Slug == model.Slug && c.Id != model.Id && c.DeletedAt == null);
            if (existingContentWithSlug != null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."] }
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

            // Kiểm tra AuthorId hợp lệ (nếu có)
            if (model.AuthorId.HasValue)
            {
                var author = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == model.AuthorId.Value);
                if (author == null)
                {
                    var errors = new Dictionary<string, string[]>
                    {
                        { nameof(model.AuthorId), ["Tác giả không tồn tại."] }
                    };
                    return BadRequest(new ErrorResponse(errors));
                }
            }

            // Cập nhật nội dung
            _mapper.Map(model, existingContent);

            // Update categories
            dbContext.ContentCategories.RemoveRange(existingContent.ContentCategories ?? Enumerable.Empty<ContentCategory>());
            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                existingContent.ContentCategories = model.CategoryIds.Select(categoryId => new ContentCategory
                {
                    ContentId = model.Id,
                    CategoryId = categoryId
                }).ToList();
            }

            // Update tags
            dbContext.ContentTags.RemoveRange(existingContent.ContentTags ?? Enumerable.Empty<ContentTag>());
            if (model.TagIds != null && model.TagIds.Any())
            {
                existingContent.ContentTags = model.TagIds.Select(tagId => new ContentTag
                {
                    ContentId = model.Id,
                    TagId = tagId
                }).ToList();
            }

            // Update field values
            dbContext.ContentFieldValues.RemoveRange(existingContent.FieldValues ?? Enumerable.Empty<ContentFieldValue>());
            if (model.FieldValues != null && model.FieldValues.Any())
            {
                existingContent.FieldValues = model.FieldValues.Select(kv => new ContentFieldValue
                {
                    ContentId = model.Id,
                    FieldId = kv.Key,
                    Value = kv.Value
                }).ToList();
            }

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var successResponse = new SuccessResponse<Content>(existingContent, "Cập nhật nội dung thành công.");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Content", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Content", new { area = "Admin" });
        }
        catch (Exception ex)
        {
            if (Request.IsAjaxRequest())
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });

            var content = await dbContext.Contents
                .AsNoTracking()
                .Include(c => c.ContentType)
                .Include(c => c.ContentCategories)
                .Include(c => c.ContentTags)
                .Include(c => c.FieldValues)
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.DeletedAt == null);
            await PopulateDropdowns(content);
            ModelState.AddModelError("", ex.Message);
            return PartialView("_Edit.Modal", model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ContentDeleteRequest model)
    {
        try
        {
            var content = await dbContext.Contents
                .FirstOrDefaultAsync(c => c.Id == model.Id && c.DeletedAt == null);

            if (content == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Nội dung không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            dbContext.Contents.Remove(content);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Content>(content, "Xóa nội dung thành công (đã ẩn).");

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = successResponse.Message,
                    redirectUrl = Url.Action("Index", "Content", new { area = "Admin" })
                });
            }

            TempData["SuccessMessage"] = successResponse.Message;
            return RedirectToAction("Index", "Content", new { area = "Admin" });
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

    private async Task PopulateDropdowns(Content? content = null)
    {
        var contentTypes = await dbContext.ContentTypes
            .AsNoTracking()
            .Where(ct => ct.DeletedAt == null)
            .ToListAsync();
        ViewBag.ContentTypes = new SelectList(contentTypes, "Id", "Name", content?.ContentTypeId);

        var authors = await dbContext.Users
            .AsNoTracking()
            .ToListAsync();
        ViewBag.Authors = new SelectList(authors, "Id", "Username", content?.AuthorId);

        var tags = await dbContext.Tags
            .AsNoTracking()
            .Where(t => t.DeletedAt == null)
            .ToListAsync();
        ViewBag.Tags = new MultiSelectList(tags, "Id", "Name", content?.ContentTags?.Select(ct => ct.TagId).ToList());

        var categories = await dbContext.Categories
            .AsNoTracking()
            .Where(c => c.DeletedAt == null)
            .ToListAsync();
        ViewBag.Categories = new MultiSelectList(categories, "Id", "Name", content?.ContentCategories?.Select(ct => ct.CategoryId).ToList());
    }
}