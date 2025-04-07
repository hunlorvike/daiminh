using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Product;
using web.Areas.Admin.ViewModels.ProductType;

namespace web.Areas.Admin.Mappers;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        // Product Type Mappings
        CreateMap<ProductType, ProductTypeViewModel>();
        CreateMap<ProductType, ProductTypeListItemViewModel>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src =>
                src.Products != null ? src.Products.Count : 0));
        CreateMap<ProductTypeViewModel, ProductType>()
            // Map only the fields that should be updated from the ViewModel
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            // Ignore fields that should not be updated from this ViewModel
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Product Mappings
        CreateMap<Product, ProductListItemViewModel>()
            .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType != null ? src.ProductType.Name : string.Empty))
            .ForMember(dest => dest.CategoryCount, opt => opt.MapFrom(src => src.ProductCategories != null ? src.ProductCategories.Count : 0))
            .ForMember(dest => dest.TagCount, opt => opt.MapFrom(src => src.ProductTags != null ? src.ProductTags.Count : 0))
            .ForMember(dest => dest.ImageCount, opt => opt.MapFrom(src => src.Images != null ? src.Images.Count : 0))
            // Get main image URL or first image URL
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any(i => i.IsMain) ?
                src.Images.First(i => i.IsMain).ImageUrl :
                src.Images != null && src.Images.Any() ? src.Images.First().ImageUrl : null));

        CreateMap<Product, ProductViewModel>()
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src =>
                src.ProductCategories != null ?
                src.ProductCategories.Select(pc => pc.CategoryId).ToList() :
                new List<int>()))
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src =>
                src.ProductTags != null ?
                src.ProductTags.Select(pt => pt.TagId).ToList() :
                new List<int>()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)) // Map existing images
            .ForMember(dest => dest.ImageFiles, opt => opt.Ignore()); // Handled by controller

        CreateMap<ProductViewModel, Product>()
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore()) // Handled separately
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore()) // Handled separately
            .ForMember(dest => dest.Images, opt => opt.Ignore()); // Handled separately

        CreateMap<ProductImage, ProductImageViewModel>().ReverseMap();
    }
}