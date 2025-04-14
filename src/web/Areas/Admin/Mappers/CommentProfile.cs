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
            .ForMember(dest => dest.ContentExcerpt, opt => opt.MapFrom(src => // Safer excerpt
                 src.Content != null && src.Content.Length > 100 ? src.Content.Substring(0, 100) + "..." : src.Content))
            .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : null))
            .ForMember(dest => dest.ReplyCount, opt => opt.MapFrom(src => src.Replies != null ? src.Replies.Count : 0))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt)); // Map UpdatedAt

        // Entity -> ViewModel
        CreateMap<Comment, CommentViewModel>()
             .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : null));

        // ViewModel -> Entity (For Update from Admin Edit)
        CreateMap<CommentViewModel, Comment>()
            // Map only fields editable by Admin
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(dest => dest.AuthorEmail, opt => opt.MapFrom(src => src.AuthorEmail))
            .ForMember(dest => dest.AuthorWebsite, opt => opt.MapFrom(src => src.AuthorWebsite))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
            // Ignore non-editable fields
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleId, opt => opt.Ignore())
            .ForMember(dest => dest.ParentId, opt => opt.Ignore())
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