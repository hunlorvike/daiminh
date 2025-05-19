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
             .ForMember(dest => dest.PublicUrl, opt => opt.Ignore());
    }
}