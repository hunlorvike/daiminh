using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.Gallery;

namespace web.Areas.Admin.Profiles;

public class GalleryProfile : Profile
{
    public GalleryProfile()
    {
        CreateMap<Folder, FolderEditRequest>().ReverseMap();
        CreateMap<MediaFile, FileEditRequest>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => Path.GetFileNameWithoutExtension(src.Name)));

        CreateMap<Folder, FolderDeleteRequest>().ReverseMap();
        CreateMap<MediaFile, FileDeleteRequest>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => Path.GetFileNameWithoutExtension(src.Name)));
    }
}