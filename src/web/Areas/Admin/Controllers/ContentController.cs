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
    IConfiguration configuration,
    IDistributedCache cache)
    : DaiminhController(mapper, serviceProvider, configuration, cache)
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

        // Load all field definitions for all content types
        var allContentTypeFields = await dbContext.ContentFieldDefinitions
            .AsNoTracking()
            .Where(f => f.DeletedAt == null)
            .ToListAsync();

        // Group field definitions by content type ID
        //var fieldsByContentType = allContentTypeFields
        //    .GroupBy(f => f.ContentTypeId)
        //    .ToDictionary(g => g.Key, g => g.ToList());

        ViewBag.FieldsByContentType = allContentTypeFields;

        return PartialView("_Create.Modal", new ContentCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var content = await dbContext.Contents
            .AsNoTracking()
            .Include(c => c.ContentType)
            .Include(c => c.ContentCategories)
            .Include(c => c.ContentTags)
            .Include(c => c.FieldValues!)
            .ThenInclude(fv => fv.Field!)
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null) ?? throw new NotFoundException("Content not found.");

        await PopulateDropdowns(content);
        var request = _mapper.Map<ContentUpdateRequest>(content);

        // Load all field definitions for all content types
        var allContentTypeFields = await dbContext.ContentFieldDefinitions
            .AsNoTracking()
            .Where(f => f.DeletedAt == null)
            .ToListAsync();

        // Create a dictionary of field values
        var fieldValues = content.FieldValues?
            .ToDictionary(fv => fv.FieldId, fv => fv.Value) ?? new Dictionary<int, string>();

        ViewBag.FieldsByContentType = allContentTypeFields;
        ViewBag.FieldValues = fieldValues;

        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var content = await dbContext.Contents
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null) ?? throw new NotFoundException("Content not found.");
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
            throw new SystemException2("Error creating content.", ex);
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
            throw new SystemException2("Error updating content.", ex);
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

            dbContext.Contents.Remove(content!);
            await dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Content>(content!, "Xóa nội dung thành công (đã ẩn).");

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
            throw new SystemException2("Error deleting content.", ex);
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

