using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Comment;

namespace web.Areas.Admin.Mappers;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        // Comment Mappings
        CreateMap<Comment, CommentListItemViewModel>()
            .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : ""))
            .ForMember(dest => dest.ArticleSlug, opt => opt.MapFrom(src => src.Article != null ? src.Article.Slug : ""))
            // Calculate ReplyCount in the controller/service where needed using a query
            .ForMember(dest => dest.ReplyCount, opt => opt.Ignore());

        CreateMap<Comment, CommentViewModel>()
            .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : null)) // Include ArticleTitle
            .ForMember(dest => dest.ParentAuthorName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.AuthorName : null)); // Map parent author name

        CreateMap<CommentViewModel, Comment>(); // Simple reverse map
    }
}