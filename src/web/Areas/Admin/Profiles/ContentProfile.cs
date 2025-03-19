using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Models.Content;
using web.Areas.Admin.Requests.Content;

namespace web.Areas.Admin.Profiles;

public class ContentProfile : Profile
{
    public ContentProfile()
    {
        CreateMap<Content, ContentViewModel>()
            .ForMember(dest => dest.ContentTypeName, opt => opt.MapFrom(src => src.ContentType!.Name))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author!.Username))
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.ContentCategories != null ? src.ContentCategories.Select(cc => cc.CategoryId).ToList() : new List<int>()))
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.ContentTags != null ? src.ContentTags.Select(ct => ct.TagId).ToList() : new List<int>()));

        CreateMap<ContentCreateRequest, Content>()
            .ForMember(dest => dest.FieldValues, opt => opt.Ignore())
            .ForMember(dest => dest.ContentCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ContentTags, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore());

        CreateMap<ContentUpdateRequest, Content>()
            .ForMember(dest => dest.FieldValues, opt => opt.Ignore())
            .ForMember(dest => dest.ContentCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ContentTags, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.Ignore());

        CreateMap<Content, ContentUpdateRequest>()
            .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src =>
                src.ContentCategories != null ? src.ContentCategories.Select(cc => cc.CategoryId).ToList() : null))
            .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src =>
                src.ContentTags != null ? src.ContentTags.Select(ct => ct.TagId).ToList() : null))
            .ForMember(dest => dest.FieldValues, opt => opt.MapFrom(src =>
                src.FieldValues != null ? src.FieldValues.ToDictionary(fv => fv.FieldId, fv => fv.Value) : new Dictionary<int, string>()));

        CreateMap<Content, ContentDeleteRequest>();
    }
}