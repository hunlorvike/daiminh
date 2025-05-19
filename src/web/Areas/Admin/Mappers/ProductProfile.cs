using AutoMapper;
using domain.Entities;
using domain.Entities.Shared;
using web.Areas.Admin.ViewModels.Shared;
namespace web.Areas.Admin.Mappers;
using web.Areas.Admin.ViewModels;

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
             .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images!.OrderBy(img => img.OrderIndex).ToList()));


        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<ProductViewModel, Product>()
            .ForMember(dest => dest.Brand, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ProductAttributes, opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Variations, opt => opt.Ignore())
            .ForMember(dest => dest.Reviews, opt => opt.Ignore())
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .IncludeBase<SeoViewModel, SeoEntity<int>>();
    }
}