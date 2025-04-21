using System.Net.Mime;
using System.Text.RegularExpressions;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;

namespace web.Areas.Admin.Services;

public class MediaService : IMediaService
{
    private readonly ApplicationDbContext _context;
    private readonly IMinioService _minioService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MediaService> _logger;
    private readonly string _bucketName;

    public MediaService(
        ApplicationDbContext context,
        IMinioService minioService,
        IConfiguration configuration,
        ILogger<MediaService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _minioService = minioService ?? throw new ArgumentNullException(nameof(minioService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bucketName = _configuration["Minio:BucketName"] ?? throw new ArgumentNullException("Minio:BucketName configuration is missing");
    }

    private string GenerateObjectName(MediaFolder? folder, string originalFileName)
    {
        var folderPath = GetFolderPath(folder);
        var fileExtension = Path.GetExtension(originalFileName).ToLowerInvariant();
        var baseName = Path.GetFileNameWithoutExtension(originalFileName);
        var slug = Regex.Replace(baseName, @"\s+", "-").ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9-]", "");
        slug = slug.Trim('-');

        var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
        var fileName = $"{slug}-{uniqueId}{fileExtension}";

        return Path.Combine(folderPath, fileName).Replace("\\", "/");
    }

    private string GetFolderPath(MediaFolder? folder)
    {
        if (folder == null) return "root";
        var pathSegments = new List<string>();
        var currentFolder = folder;
        while (currentFolder != null)
        {
            pathSegments.Insert(0, currentFolder.Name);
            currentFolder = _context.MediaFolders.AsNoTracking().FirstOrDefault(f => f.Id == currentFolder.ParentId);
        }
        return Path.Combine(pathSegments.ToArray()).Replace("\\", "/");
    }


    public async Task<List<MediaFolder>> GetRootFoldersAsync()
    {
        return await _context.MediaFolders
                           .Where(f => !f.ParentId.HasValue)
                           .OrderBy(f => f.Name)
                           .ToListAsync();
    }

    public async Task<List<MediaFolder>> GetChildFoldersAsync(int parentId)
    {
        return await _context.MediaFolders
                            .Where(f => f.ParentId == parentId)
                            .OrderBy(f => f.Name)
                            .ToListAsync();
    }

    public async Task<MediaFolder?> GetFolderByIdAsync(int id)
    {
        return await _context.MediaFolders.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<MediaFolder> CreateFolderAsync(int? parentId, string name)
    {
        if (await _context.MediaFolders.AnyAsync(f => f.ParentId == parentId && f.Name == name))
        {
            throw new InvalidOperationException($"Thư mục '{name}' đã tồn tại trong thư mục này.");
        }

        var folder = new MediaFolder { Name = name, ParentId = parentId };
        _context.MediaFolders.Add(folder);
        await _context.SaveChangesAsync();
        return folder;
    }

    public async Task<bool> RenameFolderAsync(int id, string newName)
    {
        var folder = await _context.MediaFolders.FirstOrDefaultAsync(f => f.Id == id);
        if (folder == null) return false;

        if (await _context.MediaFolders.AnyAsync(f => f.ParentId == folder.ParentId && f.Name == newName && f.Id != id))
        {
            throw new InvalidOperationException($"Thư mục '{newName}' đã tồn tại trong thư mục này.");
        }

        folder.Name = newName;
        _context.MediaFolders.Update(folder);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteFolderAsync(int id)
    {
        var folder = await _context.MediaFolders
                                   .Include(f => f.Children)
                                   .Include(f => f.Files)
                                   .FirstOrDefaultAsync(f => f.Id == id);

        if (folder == null) return true;

        if (folder.Children != null && folder.Children.Any()) return false;
        if (folder.Files != null && folder.Files.Any()) return false;

        _context.MediaFolders.Remove(folder);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<MediaFile>> GetFilesByFolderAsync(int? folderId, MediaType? mediaTypeFilter = null)
    {
        var query = _context.MediaFiles.AsNoTracking()
                             .Where(f => f.FolderId == folderId);

        if (mediaTypeFilter.HasValue)
        {
            query = query.Where(f => f.MediaType == mediaTypeFilter.Value);
        }

        return await query.OrderByDescending(f => f.CreatedAt).ToListAsync();
    }

    public async Task<MediaFile?> GetFileByIdAsync(int id)
    {
        return await _context.MediaFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
    }


    public async Task<MediaFile> UploadFileAsync(int? folderId, IFormFile file, string? altText = null, string? description = null)
    {
        var folder = folderId.HasValue ? await GetFolderByIdAsync(folderId.Value) : null;

        var objectName = GenerateObjectName(folder, file.FileName);
        var contentType = file.ContentType ?? MediaTypeNames.Application.Octet;
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var mediaType = GetMediaTypeFromFileExtension(fileExtension, contentType);

        using (var stream = file.OpenReadStream())
        {
            await _minioService.UploadFileAsync(_bucketName, objectName, stream, contentType);
        }

        var mediaFile = new MediaFile
        {
            FileName = Path.GetFileName(objectName),
            OriginalFileName = file.FileName,
            MimeType = contentType,
            FileExtension = fileExtension,
            FilePath = objectName,
            FileSize = file.Length,
            AltText = altText ?? "",
            Description = description ?? "",
            MediaType = mediaType,
            FolderId = folderId
        };

        if (mediaFile.MediaType == MediaType.Video)
        {
            //mediaFile.Duration = GetVideoDuration(file.OpenReadStream());
        }

        _context.MediaFiles.Add(mediaFile);
        await _context.SaveChangesAsync();

        return mediaFile;
    }

    private MediaType GetMediaTypeFromFileExtension(string extension, string mimeType)
    {
        extension = extension.TrimStart('.');
        if (new[] { "jpg", "jpeg", "png", "gif", "bmp", "webp", "svg" }.Contains(extension, StringComparer.OrdinalIgnoreCase) || mimeType.StartsWith("image/"))
        {
            return MediaType.Image;
        }
        if (new[] { "mp4", "avi", "mov", "wmv", "flv", "webm" }.Contains(extension, StringComparer.OrdinalIgnoreCase) || mimeType.StartsWith("video/"))
        {
            return MediaType.Video;
        }
        if (new[] { "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "rtf" }.Contains(extension, StringComparer.OrdinalIgnoreCase) || mimeType.StartsWith("application/pdf") || mimeType.StartsWith("application/msword") || mimeType.StartsWith("application/vnd.") || mimeType == MediaTypeNames.Text.Plain)
        {
            return MediaType.Document;
        }
        return MediaType.Other;
    }


    public async Task<bool> DeleteFileAsync(int id)
    {
        var file = await _context.MediaFiles.FirstOrDefaultAsync(f => f.Id == id);
        if (file == null) return true;

        try
        {
            await _minioService.DeleteFileAsync(_bucketName, file.FilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete file {FilePath} from MinIO bucket {BucketName}", file.FilePath, _bucketName);
        }

        _context.MediaFiles.Remove(file);
        await _context.SaveChangesAsync();

        return true;
    }

    public string GetFilePublicUrl(MediaFile file)
    {
        return _minioService.GetFilePublicUrl(_bucketName, file.FilePath);
    }

    public async Task<List<MediaFolder>> GetFolderBreadcrumbsAsync(int? folderId)
    {
        var breadcrumbs = new List<MediaFolder>();
        var currentFolderId = folderId;

        while (currentFolderId.HasValue)
        {
            var folder = await _context.MediaFolders.AsNoTracking().FirstOrDefaultAsync(f => f.Id == currentFolderId.Value);
            if (folder == null) break;
            breadcrumbs.Insert(0, folder);
            currentFolderId = folder.ParentId;
        }
        breadcrumbs.Insert(0, new MediaFolder { Id = 0, Name = "Root", ParentId = null });

        return breadcrumbs;
    }
}
