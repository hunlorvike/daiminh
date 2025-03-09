using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Models.Tag;
using web.Areas.Admin.Requests.Tag;

namespace web.Areas.Admin.Profiles;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<Tag, TagViewModel>();
        CreateMap<TagCreateRequest, Tag>();
        CreateMap<Tag, TagUpdateRequest>().ReverseMap();
        CreateMap<Tag, TagDeleteRequest>().ReverseMap();
    }
}