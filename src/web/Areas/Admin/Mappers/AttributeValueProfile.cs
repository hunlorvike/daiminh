using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.AttributeValue;

namespace web.Areas.Admin.Mappers;

public class AttributeValueProfile : Profile
{
    public AttributeValueProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<AttributeValue, AttributeValueListItemViewModel>()
             .ForMember(dest => dest.AttributeName, opt => opt.MapFrom(src => src.Attribute != null ? src.Attribute.Name : "N/A"));

        // Entity -> ViewModel (GET Edit)
        CreateMap<AttributeValue, AttributeValueViewModel>()
            .ForMember(dest => dest.AttributeOptions, opt => opt.Ignore()); // Populated in controller

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<AttributeValueViewModel, AttributeValue>()
            .ForMember(dest => dest.Attribute, opt => opt.Ignore()) // Handled by FK
            .ForMember(dest => dest.ProductVariationAttributeValues, opt => opt.Ignore()) // Managed separately
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}