using AutoMapper;
using domain.Entities;
using web.Areas.Client.Models.ContentType;


namespace web.Areas.Client.Profiles;

public class ContentTypeProfile : Profile
{
    public ContentTypeProfile()
    {
        CreateMap<ContentType, ContentTypeViewModel>();
    }
}