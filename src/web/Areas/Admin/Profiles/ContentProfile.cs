using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Models.Content;
using web.Areas.Admin.Requests.Content;

namespace web.Areas.Admin.Profiles;

public class ContentProfile : Profile
{
    public ContentProfile()
    {
        CreateMap<Content, ContentViewModel>()
            .ForMember(dest => dest.ContentTypeName, opt => opt.MapFrom(src => src.ContentType!.Name))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author!.Username));
        CreateMap<ContentCreateRequest, Content>();
        CreateMap<ContentUpdateRequest, Content>();
        CreateMap<Content, ContentUpdateRequest>();
        CreateMap<Content, ContentDeleteRequest>();
    }
}