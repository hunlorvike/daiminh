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
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt));

        // Entity -> ViewModel (For Edit GET)
        // AutoMapper will map matching property names including all SEO fields
        CreateMap<Brand, BrandViewModel>();

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<BrandViewModel, Brand>()
            // Ignore Id, collections, and base audit fields handled by DbContext/Base classes
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
            // SEO fields from BrandViewModel will automatically map to Brand (SeoEntity)
    }
}