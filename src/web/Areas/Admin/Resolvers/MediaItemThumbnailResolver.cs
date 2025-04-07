
using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Services;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Resolvers;

public class MediaItemThumbnailResolver : IValueResolver<MediaFile, MediaItemViewModel, string?>
{
    private readonly IMinioStorageService _minioService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MediaItemThumbnailResolver(IMinioStorageService minioService, IHttpContextAccessor httpContextAccessor)
    {
        _minioService = minioService;
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Resolve(MediaFile source, MediaItemViewModel destination, string? destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.ThumbnailPath))
        {
            if (string.IsNullOrEmpty(source.FilePath)) return null;
            return _minioService.GetPublicUrl(source.FilePath);
        }
        return _minioService.GetPublicUrl(source.ThumbnailPath);
    }
}
