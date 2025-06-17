using AutoMapper;
using domain.Entities;
using domain.Entities.Shared;
using web.Areas.Admin.ViewModels;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // ProductImage to ProductImageViewModel and vice-versa
        CreateMap<ProductImage, ProductImageViewModel>().ReverseMap();

        // Product to ProductListItemViewModel
        CreateMap<Product, ProductListItemViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
            .ForMember(dest => dest.ImageCount, opt => opt.MapFrom(src => src.Images != null ? src.Images.Count : 0))
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any(i => i.IsMain)
                ? src.Images.First(i => i.IsMain).ImageUrl
                : (src.Images != null && src.Images.Any() ? src.Images.First().ImageUrl : null)
            ));

        // Product to ProductViewModel (for Edit/Create form)
        CreateMap<Product, ProductViewModel>()
            .IncludeBase<SeoEntity<int>, SeoViewModel>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ProductTags != null ? src.ProductTags.Select(pt => pt.TagId).ToList() : new List<int>()))
            .ForMember(dest => dest.BrandOptions, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryOptions, opt => opt.Ignore())
            .ForMember(dest => dest.StatusOptions, opt => opt.Ignore())
            .ForMember(dest => dest.TagOptions, opt => opt.Ignore());

        // ProductViewModel to Product
        CreateMap<ProductViewModel, Product>()
            .IncludeBase<SeoViewModel, SeoEntity<int>>()
            .ForMember(dest => dest.Brand, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}