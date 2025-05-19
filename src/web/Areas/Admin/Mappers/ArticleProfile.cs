using AutoMapper;
using domain.Entities;
using domain.Entities.Shared;
using web.Areas.Admin.ViewModels;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Mappers;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Article, ArticleListItemViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.TagCount, opt => opt.MapFrom(src => src.ArticleTags != null ? src.ArticleTags.Count : 0))
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.ArticleProducts != null ? src.ArticleProducts.Count : 0));

        // Entity -> ViewModel (GET Edit)
        CreateMap<Article, ArticleViewModel>()
             .ForMember(dest => dest.CategoryOptions, opt => opt.Ignore())
             .ForMember(dest => dest.StatusOptions, opt => opt.Ignore())
             .ForMember(dest => dest.TagOptions, opt => opt.Ignore())
             .ForMember(dest => dest.ProductOptions, opt => opt.Ignore())
             .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ArticleTags!.Select(at => at.TagId).ToList()))
             .ForMember(dest => dest.SelectedProductIds, opt => opt.MapFrom(src => src.ArticleProducts!.Select(ap => ap.ProductId).ToList()));

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<ArticleViewModel, Article>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleTags, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore())
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .IncludeBase<SeoViewModel, SeoEntity<int>>();
    }
}