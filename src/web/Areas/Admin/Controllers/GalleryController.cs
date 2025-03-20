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
using web.Areas.Admin.Models.Gallery;
using web.Areas.Admin.Requests.Gallery;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class GalleryController(
    ApplicationDbContext dbContext,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration) : DaiminhController(mapper, serviceProvider, configuration)
{
    public async Task<IActionResult> Index(int? folderId = null)
    {
        GalleryViewModel viewModel = new()
        {
            CurrentFolder = folderId.HasValue
                ? await dbContext.Folders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == folderId.Value && f.DeletedAt == null)
                : null,
            Folders = await dbContext.Folders
                .AsNoTracking()
                .Where(f => f.ParentId == folderId && f.DeletedAt == null)
                .OrderBy(f => f.Name)
                .ToListAsync(),
            MediaFiles = await dbContext.MediaFiles
                .AsNoTracking()
                .Where(m => m.FolderId == folderId && m.DeletedAt == null)
                .OrderBy(m => m.Name)
                .ToListAsync(),
            Breadcrumbs = await GetBreadcrumbsAsync(folderId),
            AllFolders = await dbContext.Folders
                .AsNoTracking()
                .Where(f => f.DeletedAt == null)
                .OrderBy(f => f.Name)
                .ToListAsync()
        };

        ViewBag.CurrentFolderId = folderId;

        var folderSelectList = await dbContext.Folders
            .AsNoTracking()
            .Where(f => f.DeletedAt == null)
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
    public async Task<IActionResult> CreateFile()
    {
        await PopulateFolderDropdown();
        return PartialView("_CreateFile.Modal");
    }

    [AjaxOnly]
    public async Task<IActionResult> CreateFolder()
    {
        await PopulateFolderDropdown();
        return PartialView("_CreateFolder.Modal");
    }

    [AjaxOnly]
    public async Task<IActionResult> EditFolder(int id)
    {
        var folder = await dbContext.Folders
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id && f.DeletedAt == null);

        if (folder == null) return NotFound();
        var request = _mapper.Map<FolderEditRequest>(folder);
        await PopulateFolderDropdown();
        return PartialView("_EditFolder.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> EditFile(int id)
    {
        var file = await dbContext.MediaFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);

        if (file == null) return NotFound();
        var request = _mapper.Map<FileEditRequest>(file);
        await PopulateFolderDropdown();
        return PartialView("_EditFile.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> DeleteFolder(int id)
    {
        var folder = await dbContext.Folders
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id && f.DeletedAt == null);

        if (folder == null) return NotFound();
        var request = _mapper.Map<FolderDeleteRequest>(folder);
        return PartialView("_DeleteFolder.Modal", request);
    }

    [AjaxOnly]
    public async Task<IActionResult> DeleteFile(int id)
    {
        var file = await dbContext.MediaFiles
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);

        if (file == null) return NotFound();
        var request = _mapper.Map<FileDeleteRequest>(file);
        return PartialView("_DeleteFile.Modal", request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFile([FromForm] FileCreateRequest model)
    {
        var validator = GetValidator<FileCreateRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateFolderDropdown();
            return result;
        }

        try
        {
            var file = model.File;
            if (file == null || file.Length == 0)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.File), ["Vui lòng chọn một tệp để tải lên."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            var fileName = Path.GetFileName(file.FileName);
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

            await dbContext.MediaFiles.AddAsync(mediaFile);
            await dbContext.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Tệp đã được tải lên thành công.",
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
        if (result != null)
        {
            await PopulateFolderDropdown();
            return result;
        }

        try
        {
            string parentFolderPath = await GetFolderPathAsync(model.ParentId);
            string folderPath = Path.Combine(parentFolderPath, model.Name.ToUrlSlug());

            // Kiểm tra tính duy nhất của Path
            var existingFolder = await dbContext.Folders
                .FirstOrDefaultAsync(f => f.Path == folderPath && f.DeletedAt == null);
            if (existingFolder != null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.Name), ["Tên thư mục này đã tồn tại trong thư mục cha."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            var fullFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderPath);
            Directory.CreateDirectory(fullFolderPath);

            Folder folder = new()
            {
                Name = model.Name,
                Path = folderPath,
                ParentId = model.ParentId
            };

            await dbContext.Folders.AddAsync(folder);
            await dbContext.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Thư mục đã được tạo thành công.",
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFolder([FromForm] FolderEditRequest model)
    {
        var validator = GetValidator<FolderEditRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateFolderDropdown();
            return result;
        }

        try
        {
            var folder = await dbContext.Folders
                .FirstOrDefaultAsync(f => f.Id == model.Id && f.DeletedAt == null);

            if (folder == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Thư mục không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            string oldPath = folder.Path;
            string newPathSegment = model.Name!.ToUrlSlug();
            string newPath = folder.ParentId.HasValue
                ? Path.Combine(await GetFolderPathAsync(folder.ParentId), newPathSegment)
                : newPathSegment;

            // Kiểm tra tính duy nhất của Path
            var existingFolder = await dbContext.Folders
                .FirstOrDefaultAsync(f => f.Path == newPath && f.Id != model.Id && f.DeletedAt == null);
            if (existingFolder != null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.Name), ["Tên thư mục này đã tồn tại trong thư mục cha."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            folder.Name = model.Name;
            folder.Path = newPath;

            string oldFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", oldPath);
            string newFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", newPath);

            if (Directory.Exists(oldFullPath) && !Directory.Exists(newFullPath))
            {
                Directory.Move(oldFullPath, newFullPath);
            }
            else if (!Directory.Exists(oldFullPath))
            {
                Directory.CreateDirectory(newFullPath);
            }

            await UpdateSubfolderPaths(folder.Id, oldPath, newPath);
            await UpdateFilePaths(folder.Id, oldPath, newPath);

            await dbContext.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Đổi tên thư mục thành công.",
                redirectUrl = Url.Action("Index", "Gallery", new { area = "Admin", folderId = folder.ParentId })
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
    public async Task<IActionResult> EditFile([FromForm] FileEditRequest model)
    {
        var validator = GetValidator<FileEditRequest>();
        var result = await this.ValidateAndReturnBadRequest(validator, model);
        if (result != null)
        {
            await PopulateFolderDropdown();
            return result;
        }

        try
        {
            var file = await dbContext.MediaFiles
                .FirstOrDefaultAsync(m => m.Id == model.Id && m.DeletedAt == null);

            if (file == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Tệp không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            string oldPath = file.Path;
            string folderPath = await GetFolderPathAsync(file.FolderId);
            string extension = file.Extension;
            string uniquePrefix = Path.GetFileName(oldPath).Contains('_')
                ? Path.GetFileName(oldPath).Substring(0, Path.GetFileName(oldPath).IndexOf('_') + 1)
                : $"{Guid.NewGuid()}_";
            string newFileName = $"{uniquePrefix}{model.Name}{extension}";
            string newRelativePath = $"/{Path.Combine("uploads", folderPath, newFileName).Replace("\\", "/")}";

            // Kiểm tra tính duy nhất của Path
            var existingFile = await dbContext.MediaFiles
                .FirstOrDefaultAsync(m => m.Path == newRelativePath && m.Id != model.Id && m.DeletedAt == null);
            if (existingFile != null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { nameof(model.Name), ["Tên tệp này đã tồn tại trong thư mục."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            file.Name = model.Name;
            file.Path = newRelativePath;
            file.Url = newRelativePath;

            string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string oldFullPath = Path.Combine(wwwrootPath, oldPath.TrimStart('/'));
            string newFullPath = Path.Combine(wwwrootPath, newRelativePath.TrimStart('/'));

            if (System.IO.File.Exists(oldFullPath) && oldFullPath != newFullPath)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(newFullPath));
                System.IO.File.Move(oldFullPath, newFullPath);
            }

            await dbContext.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Cập nhật thông tin tệp thành công.",
                redirectUrl = Url.Action("Index", "Gallery", new { area = "Admin", folderId = file.FolderId })
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
    public async Task<IActionResult> DeleteFolder([FromForm] FolderDeleteRequest model)
    {
        try
        {
            var folder = await dbContext.Folders
                .FirstOrDefaultAsync(f => f.Id == model.Id && f.DeletedAt == null);

            if (folder == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Thư mục không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            // Kiểm tra xem thư mục có chứa tệp hoặc thư mục con không
            var hasChildren = await dbContext.Folders.AnyAsync(f => f.ParentId == model.Id && f.DeletedAt == null);
            var hasFiles = await dbContext.MediaFiles.AnyAsync(m => m.FolderId == model.Id && m.DeletedAt == null);
            if (hasChildren || hasFiles)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Không thể xóa thư mục vì vẫn còn tệp hoặc thư mục con bên trong."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folder.Path);
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
            }

            dbContext.Folders.Remove(folder);
            await dbContext.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Xóa thư mục thành công.",
                redirectUrl = Url.Action("Index", "Gallery", new { area = "Admin", folderId = folder.ParentId })
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
    public async Task<IActionResult> DeleteFile([FromForm] FileDeleteRequest model)
    {
        try
        {
            var file = await dbContext.MediaFiles
                .FirstOrDefaultAsync(m => m.Id == model.Id && m.DeletedAt == null);

            if (file == null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "General", ["Tệp không tồn tại hoặc đã bị xóa."] }
                };
                return BadRequest(new ErrorResponse(errors));
            }

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.Path.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            dbContext.MediaFiles.Remove(file);
            await dbContext.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Xóa tệp thành công.",
                redirectUrl = Url.Action("Index", "Gallery", new { area = "Admin", folderId = file.FolderId })
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

    private async Task<List<Folder>> GetBreadcrumbsAsync(int? folderId)
    {
        var breadcrumbs = new List<Folder>();
        if (folderId == null) return breadcrumbs;

        var folder = await dbContext.Folders
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == folderId && f.DeletedAt == null);

        if (folder != null)
        {
            breadcrumbs.Add(folder);
            var parentId = folder.ParentId;

            while (parentId != null)
            {
                var parent = await dbContext.Folders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == parentId && f.DeletedAt == null);
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
        if (folderId == null) return string.Empty;

        var folder = await dbContext.Folders
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == folderId && f.DeletedAt == null);

        return folder?.Path ?? string.Empty;
    }

    private async Task UpdateSubfolderPaths(int parentId, string oldParentPath, string newParentPath)
    {
        var subfolders = await dbContext.Folders
            .Where(f => f.ParentId == parentId && f.DeletedAt == null)
            .ToListAsync();

        foreach (var subfolder in subfolders)
        {
            string oldPath = subfolder.Path;
            string relativePath = oldPath.Replace(oldParentPath, "").TrimStart('/').TrimStart('\\');
            string newPath = Path.Combine(newParentPath, relativePath).Replace("\\", "/");

            subfolder.Path = newPath;
            await UpdateSubfolderPaths(subfolder.Id, oldPath, newPath);
        }
    }

    private async Task UpdateFilePaths(int folderId, string oldFolderPath, string newFolderPath)
    {
        var files = await dbContext.MediaFiles
            .Where(f => f.FolderId == folderId && f.DeletedAt == null)
            .ToListAsync();

        foreach (var file in files)
        {
            string fileName = Path.GetFileName(file.Path);
            string newPath = "/" + Path.Combine("uploads", newFolderPath, fileName).Replace("\\", "/");

            file.Path = newPath;
            file.Url = newPath;
        }
    }

    private async Task PopulateFolderDropdown()
    {
        var folders = await dbContext.Folders
            .AsNoTracking()
            .Where(f => f.DeletedAt == null)
            .OrderBy(f => f.Name)
            .ToListAsync();
        ViewBag.Folders = new SelectList(folders, "Id", "Name");
    }
}