using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Comment;

namespace web.Areas.Admin.Mappers;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Comment, CommentListItemViewModel>()
            .ForMember(dest => dest.ContentExcerpt, opt => opt.MapFrom(src =>
                 src.Content.Length > 100 ? src.Content.Substring(0, 100) + "..." : src.Content))
            .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : null))
            .ForMember(dest => dest.ReplyCount, opt => opt.MapFrom(src => src.Replies != null ? src.Replies.Count : 0));

        // Entity -> ViewModel
        CreateMap<Comment, CommentViewModel>()
             .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : null));

        // ViewModel -> Entity (For Update)
        CreateMap<CommentViewModel, Comment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleId, opt => opt.Ignore()) // Don't change article
            .ForMember(dest => dest.ParentId, opt => opt.Ignore()) // Don't change parent
            .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
            .ForMember(dest => dest.AuthorAvatar, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Article, opt => opt.Ignore())
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Replies, opt => opt.Ignore());
    }
}