using shared.Models;

namespace web.Areas.Admin.Services;

public interface IMinioStorageService
{
    Task<MinioUploadResult?> UploadFileAsync(IFormFile file, string? subFolder = null, string? customFileName = null);
    Task DeleteFileAsync(string objectName);
    Task DeleteFolderAsync(string folderPath);
    string GetPublicUrl(string objectName);
    Task<string> GetPresignedUrlAsync(string objectName, int expiryInSeconds = 60 * 60);
    Task EnsureBucketExistsAsync();
}
