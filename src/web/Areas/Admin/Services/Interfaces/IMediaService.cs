using domain.Entities;
using shared.Enums;

namespace web.Areas.Admin.Services.Interfaces;

public interface IMediaService
{
    Task<List<MediaFile>> GetFilesAsync(MediaType? mediaTypeFilter = null);
    Task<MediaFile?> GetFileByIdAsync(int id);
    Task<MediaFile> UploadFileAsync(IFormFile file, string? altText = null, string? description = null);
    Task<bool> DeleteFileAsync(int id);
    string GetFilePublicUrl(MediaFile file);
}