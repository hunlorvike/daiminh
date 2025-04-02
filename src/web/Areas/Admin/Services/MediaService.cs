using shared.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;

namespace web.Areas.Admin.Services;

public class MediaService : IMediaService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _uploadsFolder;
    private readonly Dictionary<string, MediaType> _mimeTypeMap;

    public MediaService(IWebHostEnvironment environment)
    {
        _environment = environment;
        _uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

        // Ensure uploads folder exists
        if (!Directory.Exists(_uploadsFolder))
        {
            Directory.CreateDirectory(_uploadsFolder);
        }

        // Initialize mime type to media type mapping
        _mimeTypeMap = new Dictionary<string, MediaType>
        {
            // Images
            { "image/jpeg", MediaType.Image },
            { "image/png", MediaType.Image },
            { "image/gif", MediaType.Image },
            { "image/webp", MediaType.Image },
            { "image/svg+xml", MediaType.Image },
            
            // Videos
            { "video/mp4", MediaType.Video },
            { "video/webm", MediaType.Video },
            { "video/ogg", MediaType.Video },
            { "video/quicktime", MediaType.Video },
            
            // Audio
            { "audio/mpeg", MediaType.Audio },
            { "audio/ogg", MediaType.Audio },
            { "audio/wav", MediaType.Audio },
            { "audio/webm", MediaType.Audio },
            
            // Documents
            { "application/pdf", MediaType.Document },
            { "application/msword", MediaType.Document },
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", MediaType.Document },
            { "application/vnd.ms-excel", MediaType.Document },
            { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", MediaType.Document },
            { "application/vnd.ms-powerpoint", MediaType.Document },
            { "application/vnd.openxmlformats-officedocument.presentationml.presentation", MediaType.Document },
            { "text/plain", MediaType.Document },
            
            // Archives
            { "application/zip", MediaType.Archive },
            { "application/x-rar-compressed", MediaType.Archive },
            { "application/x-7z-compressed", MediaType.Archive },
            { "application/x-tar", MediaType.Archive }
        };
    }

    public async Task<string> SaveMediaFileAsync(IFormFile file, string? subFolder = null)
    {
        if (file == null || file.Length == 0)
        {
            return string.Empty;
        }

        // Create folder if it doesn't exist
        var folderPath = _uploadsFolder;
        if (!string.IsNullOrEmpty(subFolder))
        {
            folderPath = Path.Combine(folderPath, subFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}_{SanitizeFileName(Path.GetFileName(file.FileName))}";
        var filePath = Path.Combine(folderPath, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return relative path for storage in database
        return $"/uploads/{(string.IsNullOrEmpty(subFolder) ? "" : subFolder + "/")}{fileName}";
    }

    public async Task<(string FilePath, string ThumbnailPath, string MediumPath, string LargePath, int? Width, int? Height)> ProcessImageAsync(IFormFile file, string? subFolder = null)
    {
        if (file == null || file.Length == 0)
        {
            return (string.Empty, string.Empty, string.Empty, string.Empty, null, null);
        }

        // Create folder if it doesn't exist
        var folderPath = _uploadsFolder;
        if (!string.IsNullOrEmpty(subFolder))
        {
            folderPath = Path.Combine(folderPath, subFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        // Generate unique filename
        var fileName = $"{Guid.NewGuid()}_{SanitizeFileName(Path.GetFileName(file.FileName))}";
        var filePath = Path.Combine(folderPath, fileName);
        var thumbnailFileName = $"thumb_{fileName}";
        var thumbnailPath = Path.Combine(folderPath, thumbnailFileName);
        var mediumFileName = $"medium_{fileName}";
        var mediumPath = Path.Combine(folderPath, mediumFileName);
        var largeFileName = $"large_{fileName}";
        var largePath = Path.Combine(folderPath, largeFileName);

        // Save original file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        int? width = null;
        int? height = null;

        // Process image and create thumbnails
        try
        {
            using (var image = await Image.LoadAsync(filePath))
            {
                // Get original dimensions
                width = image.Width;
                height = image.Height;

                // Create thumbnail (150x150)
                using (var thumbnail = image.Clone(ctx => ctx.Resize(new ResizeOptions
                {
                    Size = new Size(150, 150),
                    Mode = ResizeMode.Max
                })))
                {
                    await thumbnail.SaveAsync(thumbnailPath);
                }

                // Create medium size (800px width)
                using (var medium = image.Clone(ctx => ctx.Resize(new ResizeOptions
                {
                    Size = new Size(800, 0),
                    Mode = ResizeMode.Max
                })))
                {
                    await medium.SaveAsync(mediumPath);
                }

                // Create large size (1600px width)
                using (var large = image.Clone(ctx => ctx.Resize(new ResizeOptions
                {
                    Size = new Size(1600, 0),
                    Mode = ResizeMode.Max
                })))
                {
                    await large.SaveAsync(largePath);
                }
            }
        }
        catch (Exception)
        {
            // If image processing fails, use original file for all sizes
            File.Copy(filePath, thumbnailPath, true);
            File.Copy(filePath, mediumPath, true);
            File.Copy(filePath, largePath, true);
        }

        // Return relative paths for storage in database
        var relativePath = $"/uploads/{(string.IsNullOrEmpty(subFolder) ? "" : subFolder + "/")}{fileName}";
        var relativeThumbnailPath = $"/uploads/{(string.IsNullOrEmpty(subFolder) ? "" : subFolder + "/")}{thumbnailFileName}";
        var relativeMediumPath = $"/uploads/{(string.IsNullOrEmpty(subFolder) ? "" : subFolder + "/")}{mediumFileName}";
        var relativeLargePath = $"/uploads/{(string.IsNullOrEmpty(subFolder) ? "" : subFolder + "/")}{largeFileName}";

        return (relativePath, relativeThumbnailPath, relativeMediumPath, relativeLargePath, width, height);
    }

    public async Task<(string FilePath, string ThumbnailPath, int? Duration)> ProcessVideoAsync(IFormFile file, string? subFolder = null)
    {
        // For now, just save the video file without thumbnail generation
        // In a real implementation, you would use FFmpeg or similar to generate thumbnails and get duration
        var filePath = await SaveMediaFileAsync(file, subFolder);

        // Use the same path for thumbnail for now
        // In a real implementation, you would generate a thumbnail from the video
        var thumbnailPath = filePath;

        // Duration would be extracted from the video metadata
        int? duration = null;

        return (filePath, thumbnailPath, duration);
    }

    public async Task<string> ProcessDocumentAsync(IFormFile file, string? subFolder = null)
    {
        // Just save the document file
        return await SaveMediaFileAsync(file, subFolder);
    }

    public Task DeleteMediaFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return Task.CompletedTask;
        }

        // Convert relative path to absolute path
        var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public async Task DeleteMediaFilesAsync(IEnumerable<string> filePaths)
    {
        foreach (var filePath in filePaths)
        {
            await DeleteMediaFileAsync(filePath);
        }
    }

    public MediaType DetermineMediaType(string mimeType, string extension)
    {
        // Try to determine by mime type first
        if (_mimeTypeMap.TryGetValue(mimeType.ToLower(), out var mediaType))
        {
            return mediaType;
        }

        // If mime type is not recognized, try to determine by extension
        extension = extension.ToLower().TrimStart('.');

        // Image extensions
        if (new[] { "jpg", "jpeg", "png", "gif", "webp", "svg" }.Contains(extension))
        {
            return MediaType.Image;
        }

        // Video extensions
        if (new[] { "mp4", "webm", "ogg", "mov", "avi", "wmv", "flv" }.Contains(extension))
        {
            return MediaType.Video;
        }

        // Audio extensions
        if (new[] { "mp3", "wav", "ogg", "m4a", "flac" }.Contains(extension))
        {
            return MediaType.Audio;
        }

        // Document extensions
        if (new[] { "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "rtf" }.Contains(extension))
        {
            return MediaType.Document;
        }

        // Archive extensions
        if (new[] { "zip", "rar", "7z", "tar", "gz" }.Contains(extension))
        {
            return MediaType.Archive;
        }

        // Default to other
        return MediaType.Other;
    }

    public string GetMimeType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            ".mp4" => "video/mp4",
            ".webm" => "video/webm",
            ".ogg" => "video/ogg",
            ".mov" => "video/quicktime",
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".txt" => "text/plain",
            ".zip" => "application/zip",
            ".rar" => "application/x-rar-compressed",
            ".7z" => "application/x-7z-compressed",
            _ => "application/octet-stream" // Default mime type
        };
    }

    public string GetFileExtension(string fileName)
    {
        return Path.GetExtension(fileName).ToLowerInvariant();
    }

    private string SanitizeFileName(string fileName)
    {
        // Remove invalid characters
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitizedFileName = new string(fileName.Where(c => !invalidChars.Contains(c)).ToArray());

        // Replace spaces with underscores
        sanitizedFileName = sanitizedFileName.Replace(" ", "_");

        // Remove any other potentially problematic characters
        sanitizedFileName = Regex.Replace(sanitizedFileName, @"[^\w\.-]", "");

        return sanitizedFileName;
    }
}

