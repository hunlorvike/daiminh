using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.ProductType;

namespace web.Areas.Admin.Mappers;

public class ProductTypeProfile : Profile
{
    public ProductTypeProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<ProductType, ProductTypeListItemViewModel>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0));

        // Entity -> ViewModel (For Edit GET)
        CreateMap<ProductType, ProductTypeViewModel>();

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<ProductTypeViewModel, ProductType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore());
    }
}