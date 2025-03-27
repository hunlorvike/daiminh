using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.Services;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public class MediaController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<MediaFolderViewModel> _folderValidator;
    private readonly IValidator<MediaFileViewModel> _fileValidator;
    private readonly IValidator<MediaUploadViewModel> _uploadValidator;
    private readonly IMediaService _mediaService;

    public MediaController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<MediaFolderViewModel> folderValidator,
        IValidator<MediaFileViewModel> fileValidator,
        IValidator<MediaUploadViewModel> uploadValidator,
        IMediaService mediaService)
    {
        _context = context;
        _mapper = mapper;
        _folderValidator = folderValidator;
        _fileValidator = fileValidator;
        _uploadValidator = uploadValidator;
        _mediaService = mediaService;
    }

    // GET: Admin/Media
    public async Task<IActionResult> Index(int? folderId = null, string? searchTerm = null, string? fileType = null)
    {
        ViewData["PageTitle"] = "Quản lý Media";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Media", "")
        };

        var viewModel = new MediaBrowserViewModel
        {
            CurrentFolderId = folderId,
            SearchTerm = searchTerm,
            FileType = fileType
        };

        // Get current folder info if folderId is provided
        if (folderId.HasValue)
        {
            var currentFolder = await _context.Set<MediaFolder>()
                .Include(f => f.Parent)
                .FirstOrDefaultAsync(f => f.Id == folderId.Value);

            if (currentFolder != null)
            {
                viewModel.CurrentFolderName = currentFolder.Name;
                viewModel.ParentFolderId = currentFolder.ParentId;

                // Build breadcrumbs
                await BuildBreadcrumbsAsync(viewModel.Breadcrumbs, currentFolder);
            }
        }

        // Get folders
        var foldersQuery = _context.Set<MediaFolder>()
            .Include(f => f.Parent)
            .Include(f => f.Children)
            .Include(f => f.Files)
            .Where(f => f.ParentId == folderId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            foldersQuery = foldersQuery.Where(f => f.Name.Contains(searchTerm) || f.Description.Contains(searchTerm));
        }

        var folders = await foldersQuery
            .OrderBy(f => f.Name)
            .ToListAsync();

        viewModel.Folders = _mapper.Map<List<MediaFolderListItemViewModel>>(folders);

        // Get files
        var filesQuery = _context.Set<MediaFile>()
            .Include(f => f.MediaFolder)
            .Where(f => f.FolderId == folderId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            filesQuery = filesQuery.Where(f =>
                f.FileName.Contains(searchTerm) ||
                f.OriginalFileName.Contains(searchTerm) ||
                f.Description.Contains(searchTerm) ||
                f.AltText.Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(fileType))
        {
            if (Enum.TryParse<MediaType>(fileType, true, out var mediaType))
            {
                filesQuery = filesQuery.Where(f => f.MediaType == mediaType);
            }
        }

        var files = await filesQuery
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

        viewModel.Files = _mapper.Map<List<MediaFileListItemViewModel>>(files);

        return View(viewModel);
    }

    // GET: Admin/Media/CreateFolder
    public async Task<IActionResult> CreateFolder(int? parentId = null)
    {
        ViewData["PageTitle"] = "Tạo thư mục mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Media", "/Admin/Media"),
            ("Tạo thư mục", "")
        };

        var viewModel = new MediaFolderViewModel
        {
            ParentId = parentId
        };

        // Load available parent folders
        viewModel.AvailableParents = await GetAvailableParentFoldersAsync();

        return View(viewModel);
    }

    // POST: Admin/Media/CreateFolder
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFolder(MediaFolderViewModel viewModel)
    {
        var validationResult = await _folderValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            // Reload available parent folders
            viewModel.AvailableParents = await GetAvailableParentFoldersAsync();

            return View(viewModel);
        }

        var folder = _mapper.Map<MediaFolder>(viewModel);

        _context.Add(folder);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Tạo thư mục thành công";
        return RedirectToAction(nameof(Index), new { folderId = folder.ParentId });
    }

    // GET: Admin/Media/EditFolder/5
    public async Task<IActionResult> EditFolder(int id)
    {
        var folder = await _context.Set<MediaFolder>().FindAsync(id);

        if (folder == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa thư mục";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Media", "/Admin/Media"),
            ("Chỉnh sửa thư mục", "")
        };

        var viewModel = _mapper.Map<MediaFolderViewModel>(folder);

        // Load available parent folders (excluding this folder and its children)
        viewModel.AvailableParents = await GetAvailableParentFoldersAsync(folder.Id);

        return View(viewModel);
    }

    // POST: Admin/Media/EditFolder/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFolder(int id, MediaFolderViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _folderValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            // Reload available parent folders
            viewModel.AvailableParents = await GetAvailableParentFoldersAsync(viewModel.Id);

            return View(viewModel);
        }

        try
        {
            var folder = await _context.Set<MediaFolder>().FindAsync(id);

            if (folder == null)
            {
                return NotFound();
            }

            // Check for circular reference in parent-child relationship
            if (viewModel.ParentId.HasValue)
            {
                var childIds = await GetAllChildFolderIdsAsync(id);
                if (childIds.Contains(viewModel.ParentId.Value))
                {
                    ModelState.AddModelError("ParentId", "Không thể chọn thư mục con làm thư mục cha");

                    // Reload available parent folders
                    viewModel.AvailableParents = await GetAvailableParentFoldersAsync(viewModel.Id);

                    return View(viewModel);
                }
            }

            _mapper.Map(viewModel, folder);

            _context.Update(folder);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật thư mục thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await FolderExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index), new { folderId = viewModel.ParentId });
    }

    // POST: Admin/Media/DeleteFolder/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFolder(int id)
    {
        var folder = await _context.Set<MediaFolder>()
            .Include(f => f.Children)
            .Include(f => f.Files)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (folder == null)
        {
            return Json(new { success = false, message = "Không tìm thấy thư mục" });
        }

        // Check if folder has children
        if (folder.Children != null && folder.Children.Any())
        {
            return Json(new { success = false, message = "Không thể xóa thư mục có chứa thư mục con. Vui lòng xóa các thư mục con trước." });
        }

        // Check if folder has files
        if (folder.Files != null && folder.Files.Any())
        {
            return Json(new { success = false, message = "Không thể xóa thư mục có chứa file. Vui lòng xóa hoặc di chuyển các file trước." });
        }

        _context.Set<MediaFolder>().Remove(folder);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa thư mục thành công" });
    }

    // GET: Admin/Media/Upload
    public async Task<IActionResult> Upload(int? folderId = null)
    {
        ViewData["PageTitle"] = "Tải lên file";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Media", "/Admin/Media"),
            ("Tải lên", "")
        };

        var viewModel = new MediaUploadViewModel
        {
            FolderId = folderId
        };

        // Load available folders
        viewModel.AvailableFolders = await GetAvailableParentFoldersAsync();

        return View(viewModel);
    }

    // POST: Admin/Media/Upload
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(MediaUploadViewModel viewModel)
    {
        var validationResult = await _uploadValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            // Reload available folders
            viewModel.AvailableFolders = await GetAvailableParentFoldersAsync();

            return View(viewModel);
        }

        foreach (var file in viewModel.Files)
        {
            var mediaFile = new MediaFile
            {
                OriginalFileName = file.FileName,
                FileExtension = _mediaService.GetFileExtension(file.FileName),
                MimeType = _mediaService.GetMimeType(file.FileName),
                FileSize = file.Length,
                Description = viewModel.Description,
                AltText = viewModel.AltText,
                FolderId = viewModel.FolderId
            };

            // Generate a sanitized file name
            mediaFile.FileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "_") + mediaFile.FileExtension;

            // Determine media type
            mediaFile.MediaType = _mediaService.DetermineMediaType(mediaFile.MimeType, mediaFile.FileExtension);

            // Process file based on media type
            switch (mediaFile.MediaType)
            {
                case MediaType.Image:
                    var imageResult = await _mediaService.ProcessImageAsync(file, "images");
                    mediaFile.FilePath = imageResult.FilePath;
                    mediaFile.ThumbnailPath = imageResult.ThumbnailPath;
                    mediaFile.MediumSizePath = imageResult.MediumPath;
                    mediaFile.LargeSizePath = imageResult.LargePath;
                    mediaFile.Width = imageResult.Width;
                    mediaFile.Height = imageResult.Height;
                    break;
                case MediaType.Video:
                    var videoResult = await _mediaService.ProcessVideoAsync(file, "videos");
                    mediaFile.FilePath = videoResult.FilePath;
                    mediaFile.ThumbnailPath = videoResult.ThumbnailPath;
                    mediaFile.Duration = videoResult.Duration;
                    break;
                case MediaType.Audio:
                    mediaFile.FilePath = await _mediaService.SaveMediaFileAsync(file, "audio");
                    break;
                case MediaType.Document:
                    mediaFile.FilePath = await _mediaService.SaveMediaFileAsync(file, "documents");
                    break;
                default:
                    mediaFile.FilePath = await _mediaService.SaveMediaFileAsync(file, "other");
                    break;
            }

            _context.Add(mediaFile);
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Đã tải lên {viewModel.Files.Count} file thành công";
        return RedirectToAction(nameof(Index), new { folderId = viewModel.FolderId });
    }

    // GET: Admin/Media/EditFile/5
    public async Task<IActionResult> EditFile(int id)
    {
        var file = await _context.Set<MediaFile>().FindAsync(id);

        if (file == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa file";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Media", "/Admin/Media"),
            ("Chỉnh sửa file", "")
        };

        var viewModel = _mapper.Map<MediaFileViewModel>(file);

        // Load available folders
        viewModel.AvailableFolders = await GetAvailableParentFoldersAsync();

        return View(viewModel);
    }

    // POST: Admin/Media/EditFile/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFile(int id, MediaFileViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _fileValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            // Reload available folders
            viewModel.AvailableFolders = await GetAvailableParentFoldersAsync();

            return View(viewModel);
        }

        try
        {
            var file = await _context.Set<MediaFile>().FindAsync(id);

            if (file == null)
            {
                return NotFound();
            }

            // Save the current paths before mapping
            var currentFilePath = file.FilePath;
            var currentThumbnailPath = file.ThumbnailPath;
            var currentMediumSizePath = file.MediumSizePath;
            var currentLargeSizePath = file.LargeSizePath;

            _mapper.Map(viewModel, file);

            // Handle file upload if provided
            if (viewModel.FileUpload != null)
            {
                // Delete old files
                await _mediaService.DeleteMediaFileAsync(currentFilePath);
                await _mediaService.DeleteMediaFileAsync(currentThumbnailPath);
                await _mediaService.DeleteMediaFileAsync(currentMediumSizePath);
                await _mediaService.DeleteMediaFileAsync(currentLargeSizePath);

                // Update file properties
                file.OriginalFileName = viewModel.FileUpload.FileName;
                file.FileExtension = _mediaService.GetFileExtension(viewModel.FileUpload.FileName);
                file.MimeType = _mediaService.GetMimeType(viewModel.FileUpload.FileName);
                file.FileSize = viewModel.FileUpload.Length;

                // Generate a sanitized file name
                file.FileName = Path.GetFileNameWithoutExtension(viewModel.FileUpload.FileName).Replace(" ", "_") + file.FileExtension;

                // Determine media type
                file.MediaType = _mediaService.DetermineMediaType(file.MimeType, file.FileExtension);

                // Process file based on media type
                string subFolder = file.MediaType switch
                {
                    MediaType.Image => "images",
                    MediaType.Video => "videos",
                    MediaType.Audio => "audio",
                    MediaType.Document => "documents",
                    _ => "other"
                };

                switch (file.MediaType)
                {
                    case MediaType.Image:
                        var imageResult = await _mediaService.ProcessImageAsync(viewModel.FileUpload, subFolder);
                        file.FilePath = imageResult.FilePath;
                        file.ThumbnailPath = imageResult.ThumbnailPath;
                        file.MediumSizePath = imageResult.MediumPath;
                        file.LargeSizePath = imageResult.LargePath;
                        file.Width = imageResult.Width;
                        file.Height = imageResult.Height;
                        break;
                    case MediaType.Video:
                        var videoResult = await _mediaService.ProcessVideoAsync(viewModel.FileUpload, subFolder);
                        file.FilePath = videoResult.FilePath;
                        file.ThumbnailPath = videoResult.ThumbnailPath;
                        file.Duration = videoResult.Duration;
                        break;
                    default:
                        file.FilePath = await _mediaService.SaveMediaFileAsync(viewModel.FileUpload, subFolder);
                        break;
                }
            }

            _context.Update(file);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật file thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await FileExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index), new { folderId = viewModel.FolderId });
    }

    // POST: Admin/Media/DeleteFile/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFile(int id)
    {
        var file = await _context.Set<MediaFile>().FindAsync(id);

        if (file == null)
        {
            return Json(new { success = false, message = "Không tìm thấy file" });
        }

        // Delete physical files
        await _mediaService.DeleteMediaFileAsync(file.FilePath);
        await _mediaService.DeleteMediaFileAsync(file.ThumbnailPath);
        await _mediaService.DeleteMediaFileAsync(file.MediumSizePath);
        await _mediaService.DeleteMediaFileAsync(file.LargeSizePath);

        _context.Set<MediaFile>().Remove(file);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa file thành công" });
    }

    // GET: Admin/Media/Browser?callback=callbackFunction
    public async Task<IActionResult> Browser(string? callback = null, string? fileType = null, int? folderId = null)
    {
        ViewData["PageTitle"] = "Chọn file";
        ViewData["Callback"] = callback;
        ViewData["FileType"] = fileType;

        var viewModel = new MediaBrowserViewModel
        {
            CurrentFolderId = folderId,
            FileType = fileType
        };

        // Get current folder info if folderId is provided
        if (folderId.HasValue)
        {
            var currentFolder = await _context.Set<MediaFolder>()
                .Include(f => f.Parent)
                .FirstOrDefaultAsync(f => f.Id == folderId.Value);

            if (currentFolder != null)
            {
                viewModel.CurrentFolderName = currentFolder.Name;
                viewModel.ParentFolderId = currentFolder.ParentId;

                // Build breadcrumbs
                await BuildBreadcrumbsAsync(viewModel.Breadcrumbs, currentFolder);
            }
        }

        // Get folders
        var folders = await _context.Set<MediaFolder>()
            .Include(f => f.Parent)
            .Include(f => f.Children)
            .Include(f => f.Files)
            .Where(f => f.ParentId == folderId)
            .OrderBy(f => f.Name)
            .ToListAsync();

        viewModel.Folders = _mapper.Map<List<MediaFolderListItemViewModel>>(folders);

        // Get files
        var filesQuery = _context.Set<MediaFile>()
            .Include(f => f.MediaFolder)
            .Where(f => f.FolderId == folderId);

        if (!string.IsNullOrWhiteSpace(fileType))
        {
            if (fileType.Equals("image", StringComparison.OrdinalIgnoreCase))
            {
                filesQuery = filesQuery.Where(f => f.MediaType == MediaType.Image);
            }
            else if (fileType.Equals("video", StringComparison.OrdinalIgnoreCase))
            {
                filesQuery = filesQuery.Where(f => f.MediaType == MediaType.Video);
            }
            else if (fileType.Equals("document", StringComparison.OrdinalIgnoreCase))
            {
                filesQuery = filesQuery.Where(f => f.MediaType == MediaType.Document);
            }
        }

        var files = await filesQuery
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();

        viewModel.Files = _mapper.Map<List<MediaFileListItemViewModel>>(files);

        return View(viewModel);
    }

    // Helper methods
    private async Task<bool> FolderExists(int id)
    {
        return await _context.Set<MediaFolder>().AnyAsync(e => e.Id == id);
    }

    private async Task<bool> FileExists(int id)
    {
        return await _context.Set<MediaFile>().AnyAsync(e => e.Id == id);
    }

    private async Task<IEnumerable<MediaFolderSelectViewModel>> GetAvailableParentFoldersAsync(int? excludeId = null)
    {
        var query = _context.Set<MediaFolder>()
            .OrderBy(f => f.Name)
            .AsQueryable();

        if (excludeId.HasValue)
        {
            // Exclude the folder itself and all its children
            var childIds = await GetAllChildFolderIdsAsync(excludeId.Value);
            childIds.Add(excludeId.Value);
            query = query.Where(f => !childIds.Contains(f.Id));
        }

        var folders = await query.ToListAsync();
        var result = _mapper.Map<List<MediaFolderSelectViewModel>>(folders);

        // Set the level for each folder (for indentation in dropdowns)
        var folderDict = result.ToDictionary(f => f.Id);
        foreach (var folder in result)
        {
            folder.Level = CalculateFolderLevel(folder, folderDict);
        }

        return result;
    }

    private int CalculateFolderLevel(MediaFolderSelectViewModel folder, Dictionary<int, MediaFolderSelectViewModel> folderDict)
    {
        int level = 0;
        var currentFolder = folder;

        while (currentFolder.ParentId.HasValue && folderDict.ContainsKey(currentFolder.ParentId.Value))
        {
            level++;
            currentFolder = folderDict[currentFolder.ParentId.Value];
        }

        return level;
    }

    private async Task<List<int>> GetAllChildFolderIdsAsync(int folderId)
    {
        var result = new List<int>();
        var directChildren = await _context.Set<MediaFolder>()
            .Where(f => f.ParentId == folderId)
            .Select(f => f.Id)
            .ToListAsync();

        result.AddRange(directChildren);

        foreach (var childId in directChildren)
        {
            var grandChildren = await GetAllChildFolderIdsAsync(childId);
            result.AddRange(grandChildren);
        }

        return result;
    }

    private async Task BuildBreadcrumbsAsync(List<(int Id, string Name)> breadcrumbs, MediaFolder folder)
    {
        var path = new List<(int Id, string Name)>();
        var current = folder;

        // Add current folder
        path.Add((current.Id, current.Name));

        // Add parent folders
        while (current.ParentId.HasValue)
        {
            current = await _context.Set<MediaFolder>().FindAsync(current.ParentId.Value);
            if (current != null)
            {
                path.Add((current.Id, current.Name));
            }
            else
            {
                break;
            }
        }

        // Reverse to get correct order (root -> ... -> parent -> current)
        path.Reverse();
        breadcrumbs.AddRange(path);
    }
}

