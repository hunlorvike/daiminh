using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Article;

namespace web.Areas.Admin.Mappers;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Article, ArticleListItemViewModel>()
            .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src => src.ThumbnailImage ?? src.FeaturedImage)) // Use Thumb, fallback to Featured
            .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src =>
                 src.ArticleCategories != null && src.ArticleCategories.Any()
                 ? string.Join(", ", src.ArticleCategories.Select(ac => ac.Category.Name).Where(name => name != null))
                 : null
            ));

        // Entity -> ViewModel (For Edit GET)
        CreateMap<Article, ArticleViewModel>()
            .ForMember(dest => dest.SelectedCategoryIds, opt => opt.MapFrom(src => src.ArticleCategories != null ? src.ArticleCategories.Select(ac => ac.CategoryId).ToList() : new List<int>()))
            .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ArticleTags != null ? src.ArticleTags.Select(at => at.TagId).ToList() : new List<int>()))
            .ForMember(dest => dest.SelectedProductIds, opt => opt.MapFrom(src => src.ArticleProducts != null ? src.ArticleProducts.Select(ap => ap.ProductId).ToList() : new List<int>()))
            // Ignore dropdowns
            .ForMember(dest => dest.CategoryList, opt => opt.Ignore())
            .ForMember(dest => dest.TagList, opt => opt.Ignore())
            .ForMember(dest => dest.ProductList, opt => opt.Ignore())
            .ForMember(dest => dest.StatusList, opt => opt.Ignore())
            .ForMember(dest => dest.TypeList, opt => opt.Ignore());

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<ArticleViewModel, Article>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.EstimatedReadingMinutes, opt => opt.Ignore()) // Calculate this maybe?
            .ForMember(dest => dest.ArticleCategories, opt => opt.Ignore()) // Manual handling
            .ForMember(dest => dest.ArticleTags, opt => opt.Ignore()) // Manual handling
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore()) // Manual handling
            .ForMember(dest => dest.Comments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}