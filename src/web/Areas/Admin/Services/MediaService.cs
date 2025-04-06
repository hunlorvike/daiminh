using System.Text.RegularExpressions;
using shared.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Minio;
using Microsoft.Extensions.Options;
using Minio.DataModel.Args;

namespace web.Areas.Admin.Services;


public partial class MediaService
{
    private readonly IWebHostEnvironment _environment;
    private readonly MinioClient _minioClient;
    private readonly string _bucketName;
    private readonly Dictionary<string, MediaType> _mimeTypeMap;

    public MediaService(IWebHostEnvironment environment, IOptions<MinioConfiguration> minioConfig)
    {
        _environment = environment;

        _minioClient = (MinioClient)new MinioClient()
            .WithEndpoint(minioConfig.Value.Endpoint)
            .WithCredentials(minioConfig.Value.AccessKey, minioConfig.Value.SecretKey)
            .WithSSL(minioConfig.Value.UseSSL)
            .Build();

        _bucketName = minioConfig.Value.BucketName;

        __EnsureBucketExistsAsync().GetAwaiter().GetResult();

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

    private async Task __EnsureBucketExistsAsync()
    {
        try
        {
            bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!found)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));

                var policy = $@"{{
                    ""Version"": ""2012-10-17"",
                    ""Statement"": [
                        {{
                            ""Effect"": ""Allow"",
                            ""Principal"": {{""AWS"": [""*""]}},
                            ""Action"": [""s3:GetObject""],
                            ""Resource"": [""arn:aws:s3:::{_bucketName}/*""]
                        }}
                    ]
                }}";

                await _minioClient.SetPolicyAsync(new SetPolicyArgs().WithBucket(_bucketName).WithPolicy(policy));
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to ensure MinIO bucket exists: {ex.Message}", ex);
        }
    }
}

public partial class MediaService : IMediaService
{

    public async Task<string> SaveMediaFileAsync(IFormFile file, string? subFolder = null)
    {
        if (file == null || file.Length == 0)
        {
            return string.Empty;
        }

        string objectName = string.IsNullOrEmpty(subFolder)
            ? $"{Guid.NewGuid()}_{__SanitizeFileName(file.FileName)}"
            : $"{subFolder}/{Guid.NewGuid()}_{__SanitizeFileName(file.FileName)}";

        using (var stream = file.OpenReadStream())
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType);

            await _minioClient.PutObjectAsync(putObjectArgs);
        }

        return objectName;
    }

    public async Task<(string FilePath, string FileUrl, string ThumbnailPath, string ThumbnailUrl, string MediumPath, string MediumUrl, string LargePath, string LargeUrl, int? Width, int? Height)> ProcessImageAsync(IFormFile file, string? subFolder = null)
    {
        if (file == null || file.Length == 0)
        {
            return (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null);
        }

        string fileExt = Path.GetExtension(file.FileName);
        string baseFileName = $"{Guid.NewGuid()}_{__SanitizeFileName(Path.GetFileNameWithoutExtension(file.FileName))}{fileExt}";

        string objectName = string.IsNullOrEmpty(subFolder) ? baseFileName : $"{subFolder}/{baseFileName}";
        string thumbnailObjectName = string.IsNullOrEmpty(subFolder) ? $"thumb_{baseFileName}" : $"{subFolder}/thumb_{baseFileName}";
        string mediumObjectName = string.IsNullOrEmpty(subFolder) ? $"medium_{baseFileName}" : $"{subFolder}/medium_{baseFileName}";
        string largeObjectName = string.IsNullOrEmpty(subFolder) ? $"large_{baseFileName}" : $"{subFolder}/large_{baseFileName}";

        int? width = null;
        int? height = null;

        // Create a temporary file for processing
        string tempFilePath = Path.GetTempFileName();
        try
        {
            // Save the uploaded file to temp location
            using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Upload original file to MinIO
            await __UploadFileToMinioAsync(tempFilePath, objectName, file.ContentType);

            // Process image and create thumbnails
            using (var image = await Image.LoadAsync(tempFilePath))
            {
                // Get original dimensions
                width = image.Width;
                height = image.Height;

                // Create thumbnail (150x150)
                string thumbTempPath = Path.GetTempFileName();
                try
                {
                    using (var thumbnail = image.Clone(ctx => ctx.Resize(new ResizeOptions
                    {
                        Size = new Size(150, 150),
                        Mode = ResizeMode.Max
                    })))
                    {
                        await thumbnail.SaveAsync(thumbTempPath);
                    }

                    await __UploadFileToMinioAsync(thumbTempPath, thumbnailObjectName, file.ContentType);
                }
                finally
                {
                    if (File.Exists(thumbTempPath))
                    {
                        File.Delete(thumbTempPath);
                    }
                }

                // Create medium size (800px width)
                string mediumTempPath = Path.GetTempFileName();
                try
                {
                    using (var medium = image.Clone(ctx => ctx.Resize(new ResizeOptions
                    {
                        Size = new Size(800, 0),
                        Mode = ResizeMode.Max
                    })))
                    {
                        await medium.SaveAsync(mediumTempPath);
                    }

                    await __UploadFileToMinioAsync(mediumTempPath, mediumObjectName, file.ContentType);
                }
                finally
                {
                    if (File.Exists(mediumTempPath))
                    {
                        File.Delete(mediumTempPath);
                    }
                }

                // Create large size (1600px width)
                string largeTempPath = Path.GetTempFileName();
                try
                {
                    using (var large = image.Clone(ctx => ctx.Resize(new ResizeOptions
                    {
                        Size = new Size(1600, 0),
                        Mode = ResizeMode.Max
                    })))
                    {
                        await large.SaveAsync(largeTempPath);
                    }

                    await __UploadFileToMinioAsync(largeTempPath, largeObjectName, file.ContentType);
                }
                finally
                {
                    if (File.Exists(largeTempPath))
                    {
                        File.Delete(largeTempPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // If image processing fails, use original file for all sizes
            await __UploadFileToMinioAsync(tempFilePath, thumbnailObjectName, file.ContentType);
            await __UploadFileToMinioAsync(tempFilePath, mediumObjectName, file.ContentType);
            await __UploadFileToMinioAsync(tempFilePath, largeObjectName, file.ContentType);
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }

        // Generate URLs
        string fileUrl = GetFileUrl(objectName);
        string thumbnailUrl = GetFileUrl(thumbnailObjectName);
        string mediumUrl = GetFileUrl(mediumObjectName);
        string largeUrl = GetFileUrl(largeObjectName);

        return (objectName, fileUrl, thumbnailObjectName, thumbnailUrl, mediumObjectName, mediumUrl, largeObjectName, largeUrl, width, height);
    }

    private async Task __UploadFileToMinioAsync(string filePath, string objectName, string contentType)
    {
        using var fileStream = new FileStream(filePath, FileMode.Open);
        var fileInfo = new FileInfo(filePath);
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileInfo.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs);
    }

    public async Task<(string FilePath, string FileUrl, string ThumbnailPath, string ThumbnailUrl, int? Duration)> ProcessVideoAsync(IFormFile file, string? subFolder = null)
    {
        string objectName = await SaveMediaFileAsync(file, subFolder);

        // For now, just use the same path for thumbnail
        // In a real implementation, you would generate a thumbnail from the video
        string thumbnailObjectName = objectName;

        // Duration would be extracted from the video metadata
        int? duration = null;

        // Generate URLs
        string fileUrl = GetFileUrl(objectName);
        string thumbnailUrl = GetFileUrl(thumbnailObjectName);

        return (objectName, fileUrl, thumbnailObjectName, thumbnailUrl, duration);
    }
    public async Task<string> ProcessDocumentAsync(IFormFile file, string? subFolder = null)
    {
        // Just save the document file to MinIO
        return await SaveMediaFileAsync(file, subFolder);
    }

    public async Task DeleteMediaFileAsync(string objectPath)
    {
        if (string.IsNullOrEmpty(objectPath))
        {
            return;
        }

        try
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectPath));
        }
        catch (Exception)
        {
            // Log the exception if needed
        }
    }

    public async Task DeleteMediaFilesAsync(IEnumerable<string> objectPaths)
    {
        foreach (var objectPath in objectPaths)
        {
            await DeleteMediaFileAsync(objectPath);
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

    private string __SanitizeFileName(string fileName)
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

    // Helper method to get a public URL for a file
    public string GetFileUrl(string objectName)
    {
        if (string.IsNullOrEmpty(objectName))
        {
            return string.Empty;
        }

        try
        {
            // Get presigned URL (optional, for private buckets)
            // var presignedUrl = await _minioClient.PresignedGetObjectAsync(
            //     new PresignedGetObjectArgs()
            //         .WithBucket(_bucketName)
            //         .WithObject(objectName)
            //         .WithExpiry(60 * 60 * 24)); // 24 hours

            // For public buckets, you can just construct the URL
            return $"{_minioClient.Config.Endpoint}/{_bucketName}/{objectName}";
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}


