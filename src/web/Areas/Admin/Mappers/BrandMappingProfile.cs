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

        // Entity -> ViewModel (For Edit GET)
        CreateMap<Brand, BrandViewModel>(); // Map thẳng các trường

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<BrandViewModel, Brand>()
            // Ignore Id, collections, and base audit fields
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}