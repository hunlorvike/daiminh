namespace web.Areas.Admin.Services;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string folder);
    Task DeleteFileAsync(string filePath);
}