using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Product;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Product, ProductListItemViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
            .ForMember(dest => dest.ImageCount, opt => opt.MapFrom(src => src.Images != null ? src.Images.Count : 0))
            .ForMember(dest => dest.TagCount, opt => opt.MapFrom(src => src.ProductTags != null ? src.ProductTags.Count : 0))
            .ForMember(dest => dest.VariationCount, opt => opt.MapFrom(src => src.Variations != null ? src.Variations.Count : 0))
            .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews != null ? src.Reviews.Count : 0));

        // Entity -> SeoViewModel
        CreateMap<Product, SeoViewModel>();

        // SeoViewModel -> Entity (used in AfterMap)
        CreateMap<SeoViewModel, Product>()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.Name, opt => opt.Ignore())
             .ForMember(dest => dest.Slug, opt => opt.Ignore())
             .ForMember(dest => dest.Description, opt => opt.Ignore())
             .ForMember(dest => dest.ShortDescription, opt => opt.Ignore())
             .ForMember(dest => dest.Manufacturer, opt => opt.Ignore())
             .ForMember(dest => dest.Origin, opt => opt.Ignore())
             .ForMember(dest => dest.Specifications, opt => opt.Ignore())
             .ForMember(dest => dest.Usage, opt => opt.Ignore())
             .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
             .ForMember(dest => dest.IsFeatured, opt => opt.Ignore())
             .ForMember(dest => dest.IsActive, opt => opt.Ignore())
             .ForMember(dest => dest.Status, opt => opt.Ignore())
             .ForMember(dest => dest.BrandId, opt => opt.Ignore())
             .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
             .ForMember(dest => dest.Brand, opt => opt.Ignore())
             .ForMember(dest => dest.Category, opt => opt.Ignore())
             .ForMember(dest => dest.Images, opt => opt.Ignore())
             .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
             .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore())
             .ForMember(dest => dest.Variations, opt => opt.Ignore())
             .ForMember(dest => dest.ProductAttributes, opt => opt.Ignore())
             .ForMember(dest => dest.Reviews, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Entity -> ProductImageViewModel
        CreateMap<ProductImage, ProductImageViewModel>();

        // ProductImageViewModel -> Entity (used in AfterMap)
        CreateMap<ProductImageViewModel, ProductImage>()
            .ForMember(dest => dest.Product, opt => opt.Ignore()); // Ignore navigation property

        // Entity -> ViewModel (GET Edit)
        CreateMap<Product, ProductViewModel>()
             .ForMember(dest => dest.CategoryOptions, opt => opt.Ignore())
             .ForMember(dest => dest.BrandOptions, opt => opt.Ignore())
             .ForMember(dest => dest.StatusOptions, opt => opt.Ignore())
             .ForMember(dest => dest.AttributeOptions, opt => opt.Ignore())
             .ForMember(dest => dest.TagOptions, opt => opt.Ignore())
             .ForMember(dest => dest.ArticleOptions, opt => opt.Ignore())
             .ForMember(dest => dest.SelectedAttributeIds, opt => opt.MapFrom(src => src.ProductAttributes!.Select(pa => pa.AttributeId).ToList()))
             .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ProductTags!.Select(pt => pt.TagId).ToList()))
             .ForMember(dest => dest.SelectedArticleIds, opt => opt.MapFrom(src => src.ArticleProducts!.Select(ap => ap.ArticleId).ToList()))
             .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images!.OrderBy(img => img.OrderIndex).ToList())) // Map and order images
             .ForMember(dest => dest.Seo, opt => opt.MapFrom(src => src));


        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<ProductViewModel, Product>()
            .ForMember(dest => dest.Brand, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ProductAttributes, opt => opt.Ignore()) // Managed by helper
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore())     // Managed by helper
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore()) // Managed by helper
            .ForMember(dest => dest.Images, opt => opt.Ignore())         // Managed by helper
            .ForMember(dest => dest.Variations, opt => opt.Ignore())     // Managed separately
            .ForMember(dest => dest.Reviews, opt => opt.Ignore())         // Managed separately
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .AfterMap((src, dest, context) =>
            {
                context.Mapper.Map(src.Seo, dest);
                // Images will be handled in controller to manage add/update/delete logic
            });
    }
}