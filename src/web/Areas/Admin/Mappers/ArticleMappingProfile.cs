using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Article;

namespace web.Areas.Admin.Mappers;

public class ArticleMappingProfile : Profile
{
    public ArticleMappingProfile()
    {
        // Article Mappings
        CreateMap<Article, ArticleListItemViewModel>()
            // Map Categories and Tags to lists of names for display
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src =>
                src.ArticleCategories != null ?
                src.ArticleCategories.Select(ac => ac.Category.Name).ToList() :
                new List<string>()))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                src.ArticleTags != null ?
                src.ArticleTags.Select(at => at.Tag.Name).ToList() :
                new List<string>()))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src =>
                 src.Comments != null ? src.Comments.Count(c => c.IsApproved) : 0)); // Count approved comments

        CreateMap<Article, ArticleViewModel>()
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src =>
                src.ArticleCategories != null ?
                src.ArticleCategories.Select(ac => ac.CategoryId).ToList() :
                new List<int>()))
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src =>
                src.ArticleTags != null ?
                src.ArticleTags.Select(at => at.TagId).ToList() :
                new List<int>()))
            .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src =>
                src.ArticleProducts != null ?
                src.ArticleProducts.Select(ap => ap.ProductId).ToList() :
                new List<int>()))
             .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src =>
                 src.Comments != null ? src.Comments.Count : 0)) // Total comments for info
            .ForMember(dest => dest.AvailableCategories, opt => opt.Ignore()) // Populated separately
            .ForMember(dest => dest.AvailableTags, opt => opt.Ignore()) // Populated separately
            .ForMember(dest => dest.AvailableProducts, opt => opt.Ignore()) // Populated separately
            .ForMember(dest => dest.FeaturedImageFile, opt => opt.Ignore()) // Handled by controller
            .ForMember(dest => dest.ThumbnailImageFile, opt => opt.Ignore()); // Handled by controller

        CreateMap<ArticleViewModel, Article>()
            .ForMember(dest => dest.ArticleCategories, opt => opt.Ignore()) // Handled separately based on CategoryIds
            .ForMember(dest => dest.ArticleTags, opt => opt.Ignore()) // Handled separately based on TagIds
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore()) // Handled separately based on ProductIds
            .ForMember(dest => dest.Comments, opt => opt.Ignore()) // Comments are not managed via ArticleViewModel
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Managed by DB/BaseEntity
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()); // Managed by DB/BaseEntity
    }
}