using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Article;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Mappers;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Article, ArticleListItemViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.TagCount, opt => opt.MapFrom(src => src.ArticleTags != null ? src.ArticleTags.Count : 0))
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.ArticleProducts != null ? src.ArticleProducts.Count : 0));

        // --- Mapping for SEO part (already exists in CategoryProfile, ensure it's general or copy) ---
        // Assuming SeoViewModel mapping exists and is applicable to all SeoEntity<TId>
        // CreateMap<Article, SeoViewModel>(); // For GET
        // CreateMap<SeoViewModel, Article>(); // For POST/PUT (need to ignore non-SEO fields here too)
        // Re-using the SeoViewModel mapping from CategoryProfile is ideal if it's in a shared profile or base profile.
        // For demonstration, assuming it's defined here or available via other profiles.
        CreateMap<Article, SeoViewModel>(); // Map Article properties to SeoViewModel
        CreateMap<SeoViewModel, Article>() // Map SeoViewModel to Article properties
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.Title, opt => opt.Ignore())
             .ForMember(dest => dest.Slug, opt => opt.Ignore())
             .ForMember(dest => dest.Content, opt => opt.Ignore())
             .ForMember(dest => dest.Summary, opt => opt.Ignore())
             .ForMember(dest => dest.FeaturedImage, opt => opt.Ignore())
             .ForMember(dest => dest.ThumbnailImage, opt => opt.Ignore())
             .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
             .ForMember(dest => dest.IsFeatured, opt => opt.Ignore())
             .ForMember(dest => dest.PublishedAt, opt => opt.Ignore())
             .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
             .ForMember(dest => dest.AuthorName, opt => opt.Ignore())
             .ForMember(dest => dest.AuthorAvatar, opt => opt.Ignore())
             .ForMember(dest => dest.EstimatedReadingMinutes, opt => opt.Ignore())
             .ForMember(dest => dest.Status, opt => opt.Ignore())
             .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
             .ForMember(dest => dest.Category, opt => opt.Ignore())
             .ForMember(dest => dest.ArticleTags, opt => opt.Ignore())
             .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());


        // Entity -> ViewModel (GET Edit)
        CreateMap<Article, ArticleViewModel>()
             .ForMember(dest => dest.CategoryOptions, opt => opt.Ignore())   // Dropdown data loaded in controller
             .ForMember(dest => dest.StatusOptions, opt => opt.Ignore())    // Dropdown data loaded in controller
             .ForMember(dest => dest.TagOptions, opt => opt.Ignore())      // Dropdown data loaded in controller
             .ForMember(dest => dest.ProductOptions, opt => opt.Ignore())  // Dropdown data loaded in controller
             .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ArticleTags.Select(at => at.TagId).ToList())) // Map existing tag IDs
             .ForMember(dest => dest.SelectedProductIds, opt => opt.MapFrom(src => src.ArticleProducts.Select(ap => ap.ProductId).ToList())) // Map existing product IDs
             .ForMember(dest => dest.Seo, opt => opt.MapFrom(src => src)); // Map SEO part

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<ArticleViewModel, Article>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleTags, opt => opt.Ignore()) // Manual handling in controller
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore()) // Manual handling in controller
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore()) // ViewCount is not set from ViewModel
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Set by DB/framework
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // Set by DB/framework
            .ForMember(dest => dest.AuthorId, opt => opt.Ignore()) // Assuming AuthorId is tied to current user, not set from ViewModel
                                                                   // Map nested SeoViewModel onto Article using the specific map defined above
            .AfterMap((src, dest, context) =>
            {
                context.Mapper.Map(src.Seo, dest);
            });
    }
}