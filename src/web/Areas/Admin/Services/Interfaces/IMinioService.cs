namespace web.Areas.Admin.Services.Interfaces;

public interface IMinioService
{
    Task CreateBucketIfNotExistsAsync(string bucketName);
    Task<string> UploadFileAsync(string bucketName, string objectName, Stream data, string contentType);
    Task<bool> FileExistsAsync(string bucketName, string objectName);
    Task<Stream> GetFileAsync(string bucketName, string objectName);
    Task<bool> DeleteFileAsync(string bucketName, string objectName);
    Task<string> GetFilePresignedUrlAsync(string bucketName, string objectName, int expiresInt = 60 * 60 * 1); // 1 hour default
}