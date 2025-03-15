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
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Admin.Models.Gallery;
using web.Areas.Admin.Requests.Gallery;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public partial class GalleryController(
    ApplicationDbContext context,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration);

public partial class GalleryController
{
    public async Task<IActionResult> Index(int? folderId = null)
    {
        GalleryViewModel viewModel = new()
        {
            CurrentFolder = folderId.HasValue
                    ? await context.Set<Folder>().FindAsync(folderId.Value)
                    : null,
            Folders = await context.Set<Folder>()
                    .Where(f => f.ParentId == folderId)
                    .OrderBy(f => f.Name)
                    .ToListAsync(),
            MediaFiles = await context.Set<MediaFile>()
                    .Where(m => m.FolderId == folderId)
                    .OrderBy(m => m.Name)
                    .ToListAsync(),
            Breadcrumbs = await GetBreadcrumbsAsync(folderId),
            AllFolders = await context.Set<Folder>().OrderBy(f => f.Name).ToListAsync()
        };

        ViewBag.CurrentFolderId = folderId;

        var folderSelectList = await context.Set<Folder>()
            .OrderBy(f => f.Name)
            .Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Name
            })
            .ToListAsync();

        ViewBag.Folders = folderSelectList;

        return View(viewModel);
    }

    [AjaxOnly]
    public IActionResult CreateFile()
    {
        var folders = context.Set<Folder>().ToList();
        ViewBag.Folders = new SelectList(folders, "Id", "Name");
        return PartialView("_CreateFile.Modal");
    }

    [AjaxOnly]
    public IActionResult CreateFolder()
    {
        var folders = context.Set<Folder>().ToList();
        ViewBag.Folders = new SelectList(folders, "Id", "Name");
        return PartialView("_CreateFolder.Modal");
    }

    [AjaxOnly]
    public async Task<IActionResult> EditFolder(int folderId)
    {
        var folder = await context.Set<Folder>().FindAsync(folderId);
        return PartialView("_EditFolder.Modal", folder);
    }

    [AjaxOnly]
    public async Task<IActionResult> EditFile(int fileId)
    {
        var file = await context.Set<MediaFile>().FindAsync(fileId);
        return PartialView("_EditFile.Modal", file);
    }

    [AjaxOnly]
    public async Task<IActionResult> DeleteFolder(int folderId)
    {
        var folder = await context.Set<Folder>().FindAsync(folderId);
        return PartialView("_DeleteFolder.Modal", folder);
    }

    [AjaxOnly]
    public async Task<IActionResult> DeleteFile(int fileId)
    {
        var file = await context.Set<MediaFile>().FindAsync(fileId);
        return PartialView("_DeleteFile.Modal", file);
    }
}


public partial class GalleryController
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFile([FromForm] FileCreateRequest model)
    {
        var validator = GetValidator<FileCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            var file = model.File;
            var fileName = Path.GetFileName(file!.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

            string folderPath = await GetFolderPathAsync(model.FolderId);

            var uploadPath = Path.Combine("uploads", folderPath);
            var fullUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uploadPath);

            Directory.CreateDirectory(fullUploadPath);

            var filePath = Path.Combine(fullUploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var urlPath = $"/{uploadPath.Replace("\\", "/")}/{uniqueFileName}";

            MediaFile mediaFile = new()
            {
                Name = fileName,
                Path = urlPath,
                Url = urlPath,
                MimeType = file.ContentType,
                Size = file.Length,
                Extension = Path.GetExtension(file.FileName),
                FolderId = model.FolderId,
            };

            context.Set<MediaFile>().Add(mediaFile);
            await context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Thành công",
                redirectUrl = Url.Action("Index", "Gallery", new { area = "Admin", folderId = model.FolderId })
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFolder([FromForm] FolderCreateRequest model)
    {
        var validator = GetValidator<FolderCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null) return result;

        try
        {
            string parentFolderPath = await GetFolderPathAsync(model.ParentId);

            string folderPath = Path.Combine(parentFolderPath, model.Name.ToUrlSlug());

            var fullFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderPath);
            Directory.CreateDirectory(fullFolderPath);

            Folder folder = new()
            {
                Name = model.Name,
                Path = folderPath,
                ParentId = model.ParentId
            };

            context.Set<Folder>().Add(folder);
            await context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Thành công",
                redirectUrl = Url.Action("Index", "Gallery", new { area = "Admin", folderId = model.ParentId })
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

public partial class GalleryController
{
    private async Task<List<Folder>> GetBreadcrumbsAsync(int? folderId)
    {
        var breadcrumbs = new List<Folder>();
        if (folderId == null)
            return breadcrumbs;

        var folder = await context.Set<Folder>().FindAsync(folderId);
        if (folder != null)
        {
            breadcrumbs.Add(folder);
            var parentId = folder.ParentId;

            while (parentId != null)
            {
                var parent = await context.Set<Folder>().FindAsync(parentId);
                if (parent != null)
                {
                    breadcrumbs.Insert(0, parent);
                    parentId = parent.ParentId;
                }
                else
                {
                    break;
                }
            }
        }

        return breadcrumbs;
    }
    private async Task<string> GetFolderPathAsync(int? folderId)
    {
        if (folderId == null)
            return string.Empty;

        var folder = await context.Set<Folder>().FindAsync(folderId);
        if (folder == null)
            return string.Empty;

        return folder.Path;
    }
}


