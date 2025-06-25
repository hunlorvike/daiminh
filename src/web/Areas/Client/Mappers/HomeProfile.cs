using AutoMapper;
using domain.Entities;
using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Mappers;

public class HomeProfile : Profile
{
    public HomeProfile()
    {
        CreateMap<Banner, BannerViewModel>();

        CreateMap<Product, ProductCardViewModel>()
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src =>
                src.Images!.OrderByDescending(i => i.IsMain).ThenBy(i => i.OrderIndex).FirstOrDefault()!.ImageUrl ?? "/img/placeholder.svg"));

        CreateMap<Article, ArticleCardViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Chưa phân loại"))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ThumbnailImage) ? src.ThumbnailImage : "/img/placeholder.svg"))
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.PublishedAt ?? src.CreatedAt));
    }
}