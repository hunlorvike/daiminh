using domain.Entities;
using shared.Enums;

namespace web.Areas.Admin.Services;

public interface IMediaService
{
    Task<List<MediaFolder>> GetRootFoldersAsync();
    Task<List<MediaFolder>> GetChildFoldersAsync(int parentId);
    Task<MediaFolder?> GetFolderByIdAsync(int id);
    Task<MediaFolder> CreateFolderAsync(int? parentId, string name);
    Task<bool> RenameFolderAsync(int id, string newName);
    Task<bool> DeleteFolderAsync(int id);
    Task<List<MediaFile>> GetFilesByFolderAsync(int? folderId, MediaType? mediaTypeFilter = null);
    Task<MediaFile?> GetFileByIdAsync(int id);
    Task<MediaFile> UploadFileAsync(int? folderId, IFormFile file, string? altText = null, string? description = null);
    Task<bool> DeleteFileAsync(int id);
    string GetFilePublicUrl(MediaFile file);
    Task<List<MediaFolder>> GetFolderBreadcrumbsAsync(int? folderId);
}
