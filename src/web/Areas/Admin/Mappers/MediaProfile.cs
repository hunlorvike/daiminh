using AutoMapper;
using domain.Entities;
using shared.Extensions;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Mappers;

public class MediaProfile : Profile
{
    public MediaProfile()
    {
        // Entity -> ViewModel for Folders
        CreateMap<MediaFolder, MediaFolderViewModel>()
             .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
             .ForMember(dest => dest.FileCount, opt => opt.MapFrom(src => src.Files != null ? src.Files.Count : 0)) // Needs Include(f => f.Files)
             .ForMember(dest => dest.FolderCount, opt => opt.MapFrom(src => src.Children != null ? src.Children.Count : 0)); // Needs Include(f => f.Children)


        // Entity -> ViewModel for Files
        CreateMap<MediaFile, MediaFileViewModel>()
             .ForMember(dest => dest.FolderName, opt => opt.MapFrom(src => src.MediaFolder != null ? src.MediaFolder.Name : "Root")) // Needs Include(f => f.MediaFolder)
             .ForMember(dest => dest.MediaTypeDisplayName, opt => opt.MapFrom(src => src.MediaType.GetDisplayName()))
             // PublicUrl will be populated in the service/controller based on FilePath
             .ForMember(dest => dest.PublicUrl, opt => opt.Ignore());

        // ViewModel -> Entity (for Folder Create/Rename - simple mapping)
        CreateMap<MediaFolderViewModel, MediaFolder>();
        // Note: We don't map ViewModel to File Entity directly for upload,
        // the Service creates the Entity instance manually after MinIO upload.
    }
}