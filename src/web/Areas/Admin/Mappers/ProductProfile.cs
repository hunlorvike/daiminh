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
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault(img => img.IsMain) != null ? src.Images.FirstOrDefault(img => img.IsMain)!.ImageUrl : (src.Images.FirstOrDefault() != null ? src.Images.FirstOrDefault().ImageUrl : null))) // Get main image or first image
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Không phân loại"))
            .ForMember(dest => dest.ImageCount, opt => opt.MapFrom(src => src.Images != null ? src.Images.Count : 0))
            .ForMember(dest => dest.TagCount, opt => opt.MapFrom(src => src.ProductTags != null ? src.ProductTags.Count : 0))
            .ForMember(dest => dest.ArticleCount, opt => opt.MapFrom(src => src.ArticleProducts != null ? src.ArticleProducts.Count : 0));


        // --- Mapping for SEO part ---
        CreateMap<Product, SeoViewModel>();
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
             .ForMember(dest => dest.Features, opt => opt.Ignore())
             .ForMember(dest => dest.PackagingInfo, opt => opt.Ignore())
             .ForMember(dest => dest.StorageInstructions, opt => opt.Ignore())
             .ForMember(dest => dest.SafetyInfo, opt => opt.Ignore())
             .ForMember(dest => dest.ApplicationAreas, opt => opt.Ignore())
             .ForMember(dest => dest.TechnicalDocuments, opt => opt.Ignore())
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
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Entity -> ViewModel (GET Edit)
        CreateMap<Product, ProductViewModel>()
             .ForMember(dest => dest.CategoryOptions, opt => opt.Ignore()) 
             .ForMember(dest => dest.BrandOptions, opt => opt.Ignore())    
             .ForMember(dest => dest.StatusOptions, opt => opt.Ignore())   
             .ForMember(dest => dest.AvailableTags, opt => opt.Ignore())   
             .ForMember(dest => dest.AvailableArticles, opt => opt.Ignore()) 
             .ForMember(dest => dest.Seo, opt => opt.MapFrom(src => src)) 
             .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.OrderBy(img => img.OrderIndex).ToList())) 
             .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.TagId).ToList()))
             .ForMember(dest => dest.SelectedArticleIds, opt => opt.MapFrom(src => src.ArticleProducts.Select(ap => ap.ArticleId).ToList()));

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<ProductViewModel, Product>()
            .ForMember(dest => dest.Brand, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.Images, opt => opt.Ignore())
            .AfterMap((src, dest, context) =>
            {
                context.Mapper.Map(src.Seo, dest);
            });


        // ProductImage Entity <-> ProductImageViewModel
        CreateMap<ProductImage, ProductImageViewModel>();
        CreateMap<ProductImageViewModel, ProductImage>()
             .ForMember(dest => dest.Product, opt => opt.Ignore()); 
    }
}