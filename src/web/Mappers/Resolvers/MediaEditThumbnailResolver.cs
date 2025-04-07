using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Services;
using web.Areas.Admin.ViewModels.Media;

namespace web.Mappers.Resolvers;

public class MediaEditThumbnailResolver : IValueResolver<MediaFile, MediaFileEditViewModel, string>
{
    private readonly IMinioStorageService _minioService;
    public MediaEditThumbnailResolver(IMinioStorageService minioService) { _minioService = minioService; }

    public string Resolve(MediaFile source, MediaFileEditViewModel destination, string destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.ThumbnailPath ?? source.FilePath)) return "";
        return _minioService.GetPublicUrl(source.ThumbnailPath ?? source.FilePath!);
    }
}