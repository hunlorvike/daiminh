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
using web.Areas.Admin.Models.Content;
using web.Areas.Admin.Requests.Content;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}", AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class ContentController : DaiminhController
{
    private readonly ApplicationDbContext _dbContext;

    public ContentController(
        ApplicationDbContext dbContext,
        IMapper mapper,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
        : base(mapper, serviceProvider, configuration)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var contents = await _dbContext.Set<Content>()
            .Include(c => c.ContentType)
            .Include(c => c.Author)
            .ToListAsync();

        List<ContentViewModel> models = _mapper.Map<List<ContentViewModel>>(contents);
        return View(models);
    }

    [AjaxOnly]
    public async Task<IActionResult> Create()
    {
        var contentTypes = await _dbContext.Set<ContentType>().ToListAsync();
        ViewBag.ContentTypes = new SelectList(contentTypes, "Id", "Name");

        var authors = await _dbContext.Set<User>().ToListAsync();
        ViewBag.Authors = new SelectList(authors, "Id", "Username");

        var tags = await _dbContext.Set<Tag>().ToListAsync();
        ViewBag.Tags = new MultiSelectList(tags, "Id", "Name");

        var categories = await _dbContext.Set<Category>().ToListAsync();
        ViewBag.Categories = new MultiSelectList(categories, "Id", "Name");

        return PartialView("_Create.Modal", new ContentCreateRequest());
    }

    [AjaxOnly]
    public async Task<IActionResult> GetContentTypeFields(int contentTypeId)
    {
        var fields = await _dbContext.Set<ContentFieldDefinition>()
            .Where(f => f.ContentTypeId == contentTypeId)
            .ToListAsync();

        return Json(fields);
    }

    [AjaxOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var content = await _dbContext.Set<Content>()
            .Include(c => c.ContentType)
            .Include(c => c.ContentCategories)
            .Include(c => c.ContentTags)
            .Include(c => c.FieldValues)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (content == null) return NotFound();

        var contentTypes = await _dbContext.Set<ContentType>().ToListAsync();
        ViewBag.ContentTypes = new SelectList(contentTypes, "Id", "Name", content.ContentTypeId);

        var authors = await _dbContext.Set<User>().ToListAsync();
        ViewBag.Authors = new SelectList(authors, "Id", "Username", content.AuthorId);

        var tags = await _dbContext.Set<Tag>().ToListAsync();
        ViewBag.Tags = new MultiSelectList(tags, "Id", "Name", content.ContentTags?.Select(ct => ct.TagId).ToList());

        var categories = await _dbContext.Set<Category>().ToListAsync();
        ViewBag.Categories = new MultiSelectList(categories, "Id", "Name", content.ContentCategories?.Select(ct => ct.CategoryId).ToList());

        var request = _mapper.Map<ContentUpdateRequest>(content);

        return PartialView("_Edit.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> Delete(int id)
    {
        var content = await _dbContext.Set<Content>().FindAsync(id);
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
        if (result != null) return result;

        try
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var newContent = _mapper.Map<Content>(model);

            // Add categories
            if (model.CategoryIds != null && model.CategoryIds.Count != 0)
            {
                newContent.ContentCategories = model.CategoryIds.Select(categoryId =>
                    new ContentCategory
                    {
                        CategoryId = categoryId
                    }).ToList();
            }

            // Add tags
            if (model.TagIds != null && model.TagIds.Count != 0)
            {
                newContent.ContentTags = model.TagIds.Select(tagId =>
                    new ContentTag
                    {
                        TagId = tagId
                    }).ToList();
            }

            // Add field values
            if (model.FieldValues != null && model.FieldValues.Count != 0)
            {
                newContent.FieldValues = model.FieldValues.Select(kv =>
                    new ContentFieldValue
                    {
                        FieldId = kv.Key,
                        Value = kv.Value
                    }).ToList();
            }

            await _dbContext.AddAsync(newContent);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var successResponse = new SuccessResponse<Content>(newContent, "Content created successfully");

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
            return BadRequest(new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            }));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ContentUpdateRequest model)
    {
        var validator = GetValidator<ContentUpdateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var existingContent = await _dbContext.Set<Content>()
                .Include(c => c.ContentCategories)
                .Include(c => c.ContentTags)
                .Include(c => c.FieldValues)
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            if (existingContent == null) return NotFound();

            _mapper.Map(model, existingContent);

            // Update categories
            _dbContext.RemoveRange(existingContent.ContentCategories ?? Enumerable.Empty<ContentCategory>());

            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                existingContent.ContentCategories = model.CategoryIds.Select(categoryId =>
                    new ContentCategory
                    {
                        ContentId = model.Id,
                        CategoryId = categoryId
                    }).ToList();
            }

            // Update tags
            _dbContext.RemoveRange(existingContent.ContentTags ?? Enumerable.Empty<ContentTag>());

            if (model.TagIds != null && model.TagIds.Any())
            {
                existingContent.ContentTags = model.TagIds.Select(tagId =>
                    new ContentTag
                    {
                        ContentId = model.Id,
                        TagId = tagId
                    }).ToList();
            }

            // Update field values
            _dbContext.RemoveRange(existingContent.FieldValues ?? Enumerable.Empty<ContentFieldValue>());

            if (model.FieldValues != null && model.FieldValues.Any())
            {
                existingContent.FieldValues = model.FieldValues.Select(kv =>
                    new ContentFieldValue
                    {
                        ContentId = model.Id,
                        FieldId = kv.Key,
                        Value = kv.Value
                    }).ToList();
            }

            _dbContext.Update(existingContent);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var successResponse = new SuccessResponse<Content>(existingContent, "Content updated successfully");

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
            return BadRequest(new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            }));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ContentDeleteRequest model)
    {
        try
        {
            var content = await _dbContext.Set<Content>().FindAsync(model.Id);
            if (content == null) return NotFound();

            _dbContext.Remove(content);
            await _dbContext.SaveChangesAsync();

            var successResponse = new SuccessResponse<Content>(content, "Content deleted successfully");

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
            return BadRequest(new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            }));
        }
    }
}