using System.Net.Mime;
using System.Text.RegularExpressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public partial class MediaService : IMediaService
{
    private readonly ApplicationDbContext _context;
    private readonly IMinioService _minioService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MediaService> _logger;
    private readonly IMapper _mapper;
    private readonly string _bucketName;

    public MediaService(
        ApplicationDbContext context,
        IMinioService minioService,
        IConfiguration configuration,
        ILogger<MediaService> logger,
        IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _minioService = minioService ?? throw new ArgumentNullException(nameof(minioService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _bucketName = _configuration["Minio:BucketName"] ?? throw new ArgumentNullException("Minio:BucketName configuration is missing");
    }

    [Obsolete]
    public async Task<IPagedList<MediaFileViewModel>> GetPagedMediaFilesAsync(MediaFilterViewModel filter, int pageNumber, int pageSize)
    {
        var query = _context.MediaFiles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(f => f.OriginalFileName.ToLower().Contains(lowerSearchTerm) ||
                                     (f.Description != null && f.Description.ToLower().Contains(lowerSearchTerm)) ||
                                     (f.AltText != null && f.AltText.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.MediaType.HasValue)
        {
            query = query.Where(f => f.MediaType == filter.MediaType.Value);
        }

        query = query.OrderByDescending(f => f.CreatedAt);

        var pagedEntities = await query.ToPagedListAsync(pageNumber, pageSize);

        var viewModels = new List<MediaFileViewModel>();
        foreach (var entity in pagedEntities)
        {
            var vm = _mapper.Map<MediaFileViewModel>(entity);
            vm.PresignedUrl = await GetFilePresignedUrlAsync(entity);
            viewModels.Add(vm);
        }
        return new StaticPagedList<MediaFileViewModel>(viewModels, pagedEntities.GetMetaData());
    }

    public async Task<List<MediaFileViewModel>> GetAllFilesForModalAsync()
    {
        IQueryable<MediaFile> query = _context.MediaFiles.AsNoTracking();

        query = query.OrderByDescending(f => f.CreatedAt);

        var viewModels = await query
            .ProjectTo<MediaFileViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        foreach (var vm in viewModels)
        {
            if (!string.IsNullOrEmpty(vm.FilePath))
            {
                vm.PresignedUrl = await _minioService.GetFilePresignedUrlAsync(_bucketName, vm.FilePath);
            }
            else
            {
                var entity = await _context.MediaFiles.AsNoTracking().Select(mf => new { mf.Id, mf.FilePath }).FirstOrDefaultAsync(mf => mf.Id == vm.Id);
                if (entity != null && !string.IsNullOrEmpty(entity.FilePath))
                {
                    vm.PresignedUrl = await _minioService.GetFilePresignedUrlAsync(_bucketName, entity.FilePath);
                }
            }
        }
        return viewModels;
    }

    public async Task<MediaFile?> GetFileEntityByIdAsync(int id)
    {
        return await _context.MediaFiles.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<MediaFileViewModel?> GetFileViewModelByIdAsync(int id)
    {
        var entity = await GetFileEntityByIdAsync(id);
        if (entity == null) return null;

        var vm = _mapper.Map<MediaFileViewModel>(entity);
        vm.PresignedUrl = await GetFilePresignedUrlAsync(entity);
        return vm;
    }

    public async Task<OperationResult<MediaFileViewModel>> UploadFileAsync(IFormFile file, string? altText = null, string? description = null)
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

            var mediaFileEntity = new MediaFile
            {
                FileName = Path.GetFileName(objectName), // Store just the filename for the DB, MinIO path is bucket/filename
                OriginalFileName = file.FileName,
                MimeType = contentType,
                FileExtension = fileExtension,
                FilePath = objectName, // This is the MinIO object name (key)
                FileSize = file.Length,
                AltText = altText?.Trim(),
                Description = description?.Trim(),
                MediaType = mediaType,
            };

            // Potentially get video duration here if needed
            // if (mediaFileEntity.MediaType == MediaType.Video) { /* ... get duration ... */ }

            _context.MediaFiles.Add(mediaFileEntity);
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<MediaFileViewModel>(mediaFileEntity);
            viewModel.PresignedUrl = await GetFilePresignedUrlAsync(mediaFileEntity);

            _logger.LogInformation("Uploaded file {OriginalFileName} as {ObjectName} to bucket {BucketName}. DB ID: {DbId}", file.FileName, objectName, _bucketName, mediaFileEntity.Id);
            return OperationResult<MediaFileViewModel>.SuccessResult(viewModel, "Tải lên tập tin thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file {FileName} to MinIO bucket {BucketName}", file.FileName, _bucketName);
            return OperationResult<MediaFileViewModel>.FailureResult("Lỗi khi tải lên tập tin: " + ex.Message);
        }
    }

    public async Task<OperationResult> DeleteFileAsync(int id)
    {
        var file = await _context.MediaFiles.FirstOrDefaultAsync(f => f.Id == id);
        if (file == null)
        {
            _logger.LogWarning("Attempted to delete non-existent media file with ID: {FileId}", id);
            return OperationResult.FailureResult("Không tìm thấy tập tin để xóa.");
        }

        try
        {
            bool minioDeleteSuccess = await _minioService.DeleteFileAsync(_bucketName, file.FilePath);
            if (!minioDeleteSuccess)
            {
                // Log this, but proceed to delete from DB if that's the desired behavior for orphaned DB entries.
                _logger.LogWarning("Failed to delete file {FilePath} from MinIO bucket {BucketName}, but will attempt to remove DB record.", file.FilePath, _bucketName);
            }

            _context.MediaFiles.Remove(file);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted media file: ID={FileId}, Path={FilePath}", id, file.FilePath);
            return OperationResult.SuccessResult("Xóa tập tin thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting media file ID {FileId}, Path {FilePath}", id, file.FilePath);
            return OperationResult.FailureResult("Lỗi khi xóa tập tin: " + ex.Message);
        }
    }

    public async Task<OperationResult> UpdateMediaFileDetailsAsync(MediaFileViewModel viewModel)
    {
        var mediaFile = await _context.MediaFiles.FindAsync(viewModel.Id);
        if (mediaFile == null)
        {
            return OperationResult.FailureResult("Không tìm thấy tập tin.");
        }

        mediaFile.AltText = viewModel.AltText?.Trim();
        mediaFile.Description = viewModel.Description?.Trim();
        // OriginalFileName, FilePath, etc. should not be changed here usually

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated media file details for ID: {FileId}", viewModel.Id);
            return OperationResult.SuccessResult("Cập nhật thông tin tập tin thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating media file details for ID: {FileId}", viewModel.Id);
            return OperationResult.FailureResult("Lỗi khi cập nhật thông tin tập tin: " + ex.Message);
        }
    }


    public async Task<string> GetFilePresignedUrlAsync(MediaFile file)
    {
        if (file == null || string.IsNullOrEmpty(file.FilePath)) return string.Empty;
        return await _minioService.GetFilePresignedUrlAsync(_bucketName, file.FilePath);
    }
}

// Helper methods (GenerateObjectName, GetMediaTypeFromFileExtension) remain in the partial class
public partial class MediaService
{
    private string GenerateObjectName(string originalFileName)
    {
        var fileExtension = Path.GetExtension(originalFileName).ToLowerInvariant();
        var baseName = Path.GetFileNameWithoutExtension(originalFileName);
        var slug = Regex.Replace(baseName, @"\s+", "-").ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9-.]", ""); // Allow dots in slug part if desired, or remove them.
        slug = slug.Trim('-', '.');

        var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
        // Store in year/month folders for better organization in MinIO
        var yearMonthPath = DateTime.UtcNow.ToString("yyyy/MM");
        var fileName = $"{slug}-{uniqueId}{fileExtension}";

        return Path.Combine(yearMonthPath, fileName).Replace("\\", "/");
    }

    private MediaType GetMediaTypeFromFileExtension(string extension, string mimeType)
    {
        extension = extension.TrimStart('.').ToLowerInvariant();
        mimeType = mimeType.ToLowerInvariant();

        if (new[] { "jpg", "jpeg", "png", "gif", "bmp", "webp", "svg", "ico" }.Contains(extension) || mimeType.StartsWith("image/"))
        {
            return MediaType.Image;
        }
        if (new[] { "mp4", "avi", "mov", "wmv", "flv", "webm", "mkv", "mpeg", "mpg" }.Contains(extension) || mimeType.StartsWith("video/"))
        {
            return MediaType.Video;
        }
        if (new[] { "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "rtf", "csv", "odt", "ods", "odp" }.Contains(extension) ||
            mimeType.StartsWith("application/pdf") ||
            mimeType.StartsWith("application/msword") ||
            mimeType.StartsWith("application/vnd.openxmlformats-officedocument.wordprocessingml.document") ||
            mimeType.StartsWith("application/vnd.ms-excel") ||
            mimeType.StartsWith("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") ||
            mimeType.StartsWith("application/vnd.ms-powerpoint") ||
            mimeType.StartsWith("application/vnd.openxmlformats-officedocument.presentationml.presentation") ||
            mimeType == MediaTypeNames.Text.Plain ||
            mimeType == "text/csv")
        {
            return MediaType.Document;
        }

        return MediaType.Other;
    }
}