using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class TagProfile : Profile
{
    public TagProfile()
    {
        // Tag Mappings
        CreateMap<Tag, TagListItemViewModel>()
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src =>
                (src.ProductTags != null ? src.ProductTags.Count : 0) +
                (src.ArticleTags != null ? src.ArticleTags.Count : 0)
            ));

        // Entity -> ViewModel (For Edit GET)
        CreateMap<Tag, TagViewModel>();

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<TagViewModel, Tag>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleTags, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}