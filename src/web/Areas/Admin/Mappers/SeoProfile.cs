using AutoMapper;
using domain.Entities.Shared;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Mappers;

public class SeoProfile : Profile
{
    public SeoProfile()
    {
        CreateMap<SeoEntity<int>, SeoViewModel>()
            .ForMember(dest => dest.MetaTitle, opt => opt.MapFrom(src => src.MetaTitle))
            .ForMember(dest => dest.MetaDescription, opt => opt.MapFrom(src => src.MetaDescription))
            .ForMember(dest => dest.MetaKeywords, opt => opt.MapFrom(src => src.MetaKeywords))
            .ForMember(dest => dest.CanonicalUrl, opt => opt.MapFrom(src => src.CanonicalUrl))
            .ForMember(dest => dest.NoIndex, opt => opt.MapFrom(src => src.NoIndex))
            .ForMember(dest => dest.NoFollow, opt => opt.MapFrom(src => src.NoFollow))
            .ForMember(dest => dest.OgTitle, opt => opt.MapFrom(src => src.OgTitle))
            .ForMember(dest => dest.OgDescription, opt => opt.MapFrom(src => src.OgDescription))
            .ForMember(dest => dest.OgImage, opt => opt.MapFrom(src => src.OgImage))
            .ForMember(dest => dest.OgType, opt => opt.MapFrom(src => src.OgType))
            .ForMember(dest => dest.TwitterTitle, opt => opt.MapFrom(src => src.TwitterTitle))
            .ForMember(dest => dest.TwitterDescription, opt => opt.MapFrom(src => src.TwitterDescription))
            .ForMember(dest => dest.TwitterImage, opt => opt.MapFrom(src => src.TwitterImage))
            .ForMember(dest => dest.TwitterCard, opt => opt.MapFrom(src => src.TwitterCard))
            .ForMember(dest => dest.SchemaMarkup, opt => opt.MapFrom(src => src.SchemaMarkup))
            .ForMember(dest => dest.BreadcrumbJson, opt => opt.MapFrom(src => src.BreadcrumbJson))
            .ForMember(dest => dest.SitemapPriority, opt => opt.MapFrom(src => src.SitemapPriority))
            .ForMember(dest => dest.SitemapChangeFrequency, opt => opt.MapFrom(src => src.SitemapChangeFrequency))
            .ReverseMap();
    }
}
