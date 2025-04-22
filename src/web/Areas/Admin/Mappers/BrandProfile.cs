using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Brand;

namespace web.Areas.Admin.Mappers;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Brand, BrandListItemViewModel>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0));

        // Entity -> ViewModel (GET Edit)
        CreateMap<Brand, BrandViewModel>();

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<BrandViewModel, Brand>()
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}