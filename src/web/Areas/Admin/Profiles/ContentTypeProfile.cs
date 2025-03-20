using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.ContentType;

namespace web.Areas.Admin.Profiles;

public class ContentTypeProfile : Profile
{
    public ContentTypeProfile()
    {
        CreateMap<ContentTypeCreateRequest, ContentType>();
        CreateMap<ContentType, ContentTypeUpdateRequest>().ReverseMap();
        CreateMap<ContentType, ContentTypeDeleteRequest>().ReverseMap();
    }
}