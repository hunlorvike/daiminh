using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.ProductFieldDefinition;

namespace web.Areas.Admin.Profiles;

public class ProductFieldDefinitionProfile : Profile
{
    public ProductFieldDefinitionProfile()
    {
        CreateMap<ProductFieldDefinitionCreateRequest, ProductFieldDefinition>();
        CreateMap<ProductFieldDefinition, ProductFieldDefinitionUpdateRequest>().ReverseMap();
        CreateMap<ProductFieldDefinition, ProductFieldDefinitionDeleteRequest>().ReverseMap();
    }
}