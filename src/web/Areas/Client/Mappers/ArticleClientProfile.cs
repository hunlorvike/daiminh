using AutoMapper;
using domain.Entities;
using domain.Entities.Shared;
using web.Areas.Admin.ViewModels.Shared;
using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Mappers;

public class ArticleClientProfile : Profile
{
    public ArticleClientProfile()
    {
        // Mapping từ SeoEntity sang SeoViewModel (để kế thừa)
        CreateMap<SeoEntity<int>, SeoViewModel>();

        // Mapping cho Article Card (dùng ở trang Index và các nơi khác)
        CreateMap<Article, ArticleCardViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Chưa phân loại"))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ThumbnailImage) ? src.ThumbnailImage : "/img/placeholder.svg"))
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.PublishedAt ?? src.CreatedAt));

        // Mapping cho trang chi tiết bài viết
        CreateMap<Article, ArticleDetailViewModel>()
            .IncludeBase<SeoEntity<int>, SeoViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.CategorySlug, opt => opt.MapFrom(src => src.Category != null ? src.Category.Slug : null))
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.PublishedAt ?? src.CreatedAt))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ArticleTags!.Select(at => at.Tag!.Name).ToList()));
    }
}