using System.Net.Mime;
using System.Text.RegularExpressions;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;

namespace web.Areas.Admin.Services;

public partial class MediaService : IMediaService
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


    public async Task<List<MediaFile>> GetFilesAsync(MediaType? mediaTypeFilter = null)
    {
        var query = _context.MediaFiles.AsNoTracking();

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


    public async Task<MediaFile> UploadFileAsync(IFormFile file, string? altText = null, string? description = null)
    {
        try
        {
            await _minioService.CreateBucketIfNotExistsAsync(_bucketName);
            var objectName = GenerateObjectName(file.FileName);
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
            };

            if (mediaFile.MediaType == MediaType.Video)
            {
                //mediaFile.Duration = GetVideoDuration(file.OpenReadStream());
            }

            _context.MediaFiles.Add(mediaFile);
            await _context.SaveChangesAsync();

            return mediaFile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file {FileName} to MinIO bucket {BucketName}", file.FileName, _bucketName);
            throw;
        }
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
}


public partial class MediaService
{
    private string GenerateObjectName(string originalFileName)
    {
        var fileExtension = Path.GetExtension(originalFileName).ToLowerInvariant();
        var baseName = Path.GetFileNameWithoutExtension(originalFileName);
        var slug = Regex.Replace(baseName, @"\s+", "-").ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9-]", "");
        slug = slug.Trim('-');

        var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
        var fileName = $"{slug}-{uniqueId}{fileExtension}";

        return Path.Combine(fileName).Replace("\\", "/");
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
}