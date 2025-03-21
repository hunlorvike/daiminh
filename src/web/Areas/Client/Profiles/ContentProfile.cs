using AutoMapper;
using domain.Entities;
using web.Areas.Client.Models.Content;


namespace web.Areas.Client.Profiles;

public class ContentProfile : Profile
{
    public ContentProfile()
    {
        CreateMap<Content, ContentViewModel>()
            .ForMember(dest => dest.ContentTypeName, opt => opt.MapFrom(src => src.ContentType!.Name))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author!.Username));

    }
}