using AutoMapper;
using domain.Entities;
using web.Areas.Client.ViewModels.Article;

namespace web.Areas.Client.Mappers;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        // Entity -> ArticleItemViewModel (For listing)
        CreateMap<Article, ArticleItemViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.CategorySlug, opt => opt.MapFrom(src => src.Category != null ? src.Category.Slug : null))
            .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src => src.ThumbnailImage ?? src.FeaturedImage));

        // Entity -> ArticleDetailViewModel (For detail page)
        CreateMap<Article, ArticleDetailViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.CategorySlug, opt => opt.MapFrom(src => src.Category != null ? src.Category.Slug : null))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ArticleTags != null ? src.ArticleTags.Select(at => at.Tag) : new List<Tag>()))
            .ForMember(dest => dest.FeaturedImage, opt => opt.MapFrom(src => src.FeaturedImage)); // Ensure FeaturedImage is mapped

        // Entity Tag -> ArticleTagViewModel
        CreateMap<Tag, ArticleTagViewModel>();

        // Entity Product -> ArticleProductViewModel
        CreateMap<Product, ArticleProductViewModel>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any()
                    ? (src.Images.FirstOrDefault(i => i.IsMain) != null
                        ? (src.Images.FirstOrDefault(i => i.IsMain).ThumbnailUrl ?? src.Images.FirstOrDefault(i => i.IsMain).ImageUrl)
                        : (src.Images.FirstOrDefault() != null
                            ? (src.Images.FirstOrDefault().ThumbnailUrl ?? src.Images.FirstOrDefault().ImageUrl)
                            : null))
                    : null))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src =>
                src.Variations != null && src.Variations.Any(v => v.IsDefault)
                    ? src.Variations.First(v => v.IsDefault).Price
                    : (src.Variations != null && src.Variations.Any() ? src.Variations.First().Price : 0)))
            .ForMember(dest => dest.SalePrice, opt => opt.MapFrom(src =>
                src.Variations != null && src.Variations.Any(v => v.IsDefault)
                    ? src.Variations.First(v => v.IsDefault).SalePrice
                    : (src.Variations != null && src.Variations.Any() ? src.Variations.First().SalePrice : null)));


        // Entity Category -> CategorySidebarViewModel (For sidebar)
        CreateMap<Category, CategorySidebarViewModel>()
            .ForMember(dest => dest.ArticleCount, opt => opt.MapFrom(src => src.Articles != null ? src.Articles.Count(a => a.Status == shared.Enums.PublishStatus.Published) : 0));

        // Entity Tag -> TagSidebarViewModel (For sidebar)
        CreateMap<Tag, TagSidebarViewModel>()
            .ForMember(dest => dest.ArticleCount, opt => opt.MapFrom(src => src.ArticleTags != null ? src.ArticleTags.Count(at => at.Article != null && at.Article.Status == shared.Enums.PublishStatus.Published) : 0));
    }
}