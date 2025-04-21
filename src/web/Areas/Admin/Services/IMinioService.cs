namespace web.Areas.Admin.Services;

public interface IMinioService
{
    Task CreateBucketIfNotExistsAsync(string bucketName);
    Task<string> UploadFileAsync(string bucketName, string objectName, Stream data, string contentType);
    Task<bool> FileExistsAsync(string bucketName, string objectName);
    Task<Stream> GetFileAsync(string bucketName, string objectName);
    Task<bool> DeleteFileAsync(string bucketName, string objectName);
    string GetFilePublicUrl(string bucketName, string objectName);
    Task<string> GetFilePresignedUrlAsync(string bucketName, string objectName, int expiresInt = 60 * 5); // Default 5 mins
}
