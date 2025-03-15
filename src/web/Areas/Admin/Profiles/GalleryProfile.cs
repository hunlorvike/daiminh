using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.Gallery;

namespace web.Areas.Admin.Profiles
{
    public class GalleryProfile : Profile
    {
        public GalleryProfile()
        {
            CreateMap<Folder, FolderEditRequest>().ReverseMap();
        }
    }
}
