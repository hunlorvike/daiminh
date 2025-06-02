using domain.Entities;
using shared.Enums;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IMediaService
{
    Task<IPagedList<MediaFileViewModel>> GetPagedMediaFilesAsync(MediaFilterViewModel filter, int pageNumber, int pageSize);
    Task<List<MediaFileViewModel>> GetFilesForModalAsync(MediaType? mediaTypeFilter = null, string? searchTerm = null, int limit = 50);
    Task<MediaFile?> GetFileEntityByIdAsync(int id);
    Task<MediaFileViewModel?> GetFileViewModelByIdAsync(int id);
    Task<OperationResult<MediaFileViewModel>> UploadFileAsync(IFormFile file, string? altText = null, string? description = null);
    Task<OperationResult> DeleteFileAsync(int id);
    Task<string> GetFilePresignedUrlAsync(MediaFile file);
    Task<OperationResult> UpdateMediaFileDetailsAsync(MediaFileViewModel viewModel);
}