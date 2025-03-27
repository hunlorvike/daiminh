using shared.Enums;

namespace web.Areas.Admin.Services;

public interface IMediaService
{
    Task<string> SaveMediaFileAsync(IFormFile file, string? subFolder = null);
    Task<(string FilePath, string ThumbnailPath, string MediumPath, string LargePath, int? Width, int? Height)> ProcessImageAsync(IFormFile file, string? subFolder = null);
    Task<(string FilePath, string ThumbnailPath, int? Duration)> ProcessVideoAsync(IFormFile file, string? subFolder = null);
    Task<string> ProcessDocumentAsync(IFormFile file, string? subFolder = null);
    Task DeleteMediaFileAsync(string filePath);
    Task DeleteMediaFilesAsync(IEnumerable<string> filePaths);
    MediaType DetermineMediaType(string mimeType, string extension);
    string GetMimeType(string fileName);
    string GetFileExtension(string fileName);
}
