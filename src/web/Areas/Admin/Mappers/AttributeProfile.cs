using AutoMapper;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class AttributeProfile : Profile
{
    public AttributeProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<domain.Entities.Attribute, AttributeListItemViewModel>()
            .ForMember(dest => dest.ValueCount, opt => opt.MapFrom(src => src.Values != null ? src.Values.Count : 0));

        // Entity -> ViewModel (GET Edit)
        CreateMap<domain.Entities.Attribute, AttributeViewModel>();

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<AttributeViewModel, domain.Entities.Attribute>()
            .ForMember(dest => dest.Values, opt => opt.Ignore())
            .ForMember(dest => dest.ProductAttributes, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
