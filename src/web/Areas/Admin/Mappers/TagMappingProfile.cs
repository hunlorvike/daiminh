using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Tag;

namespace web.Areas.Admin.Mappers;

public class TagMappingProfile : Profile
{
    public TagMappingProfile()
    {
        // Tag Mappings (Generic)
        CreateMap<Tag, TagListItemViewModel>()
            // Calculate ItemCount based on all possible related collections
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src =>
                (src.ProductTags != null ? src.ProductTags.Count : 0) +
                (src.ArticleTags != null ? src.ArticleTags.Count : 0) +
                (src.ProjectTags != null ? src.ProjectTags.Count : 0) + // Assuming ProjectTags exists
                (src.GalleryTags != null ? src.GalleryTags.Count : 0)
            ));

        CreateMap<Tag, TagViewModel>().ReverseMap();
    }
}
