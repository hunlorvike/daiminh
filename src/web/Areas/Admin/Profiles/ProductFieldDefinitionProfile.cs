using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Models.ProductFieldDefinition;
using web.Areas.Admin.Requests.ProductFieldDefinition;

namespace web.Areas.Admin.Profiles;

public class ProductFieldDefinitionProfile : Profile
{
    public ProductFieldDefinitionProfile()
    {
        CreateMap<ProductFieldDefinition, ProductFieldDefinitionViewModel>();
        CreateMap<ProductFieldDefinitionCreateRequest, ProductFieldDefinition>();
        CreateMap<ProductFieldDefinition, ProductFieldDefinitionUpdateRequest>().ReverseMap();
        CreateMap<ProductFieldDefinition, ProductFieldDefinitionDeleteRequest>().ReverseMap();
    }
}