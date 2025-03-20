using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.ContentFieldDefinition;

namespace web.Areas.Admin.Profiles;

public class ContentFieldDefinitionProfile : Profile
{
    public ContentFieldDefinitionProfile()
    {
        CreateMap<ContentFieldDefinitionCreateRequest, ContentFieldDefinition>();
        CreateMap<ContentFieldDefinition, ContentFieldDefinitionUpdateRequest>().ReverseMap();
        CreateMap<ContentFieldDefinition, ContentFieldDefinitionDeleteRequest>().ReverseMap();
    }
}