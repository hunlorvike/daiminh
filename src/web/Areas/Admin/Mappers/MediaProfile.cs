using AutoMapper;
using domain.Entities;
using shared.Extensions;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class MediaProfile : Profile
{
    public MediaProfile()
    {
        // Entity -> ViewModel for Files
        CreateMap<MediaFile, MediaFileViewModel>()
             .ForMember(dest => dest.MediaTypeDisplayName, opt => opt.MapFrom(src => src.MediaType.GetDisplayName()))
             .ForMember(dest => dest.PresignedUrl, opt => opt.Ignore());

        // ViewModel -> Entity (For updating metadata if needed)
        // Only map editable fields. FileName, FilePath, etc. should not be changed from VM for existing files.
        CreateMap<MediaFileViewModel, MediaFile>()
            .ForMember(dest => dest.FileName, opt => opt.Ignore())
            .ForMember(dest => dest.OriginalFileName, opt => opt.Ignore())
            .ForMember(dest => dest.MimeType, opt => opt.Ignore())
            .ForMember(dest => dest.FileExtension, opt => opt.Ignore())
            .ForMember(dest => dest.FilePath, opt => opt.Ignore())
            .ForMember(dest => dest.FileSize, opt => opt.Ignore())
            .ForMember(dest => dest.MediaType, opt => opt.Ignore())
            .ForMember(dest => dest.Duration, opt => opt.Ignore()) // Assuming duration is not editable via this VM
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}