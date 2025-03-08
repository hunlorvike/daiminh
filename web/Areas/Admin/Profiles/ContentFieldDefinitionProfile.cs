using AutoMapper;
using core.Entities;
using web.Areas.Admin.Models.ContentFieldDefinition;
using web.Areas.Admin.Requests.ContentFieldDefinition;

namespace web.Areas.Admin.Profiles;

public class ContentFieldDefinitionProfile : Profile
{
    public ContentFieldDefinitionProfile()
    {
        CreateMap<ContentFieldDefinition, ContentFieldDefinitionViewModel>();
        CreateMap<ContentFieldDefinitionCreateRequest, ContentFieldDefinition>();
        CreateMap<ContentFieldDefinition, ContentFieldDefinitionUpdateRequest>().ReverseMap();
        CreateMap<ContentFieldDefinition, ContentFieldDefinitionDeleteRequest>().ReverseMap();
    }
}