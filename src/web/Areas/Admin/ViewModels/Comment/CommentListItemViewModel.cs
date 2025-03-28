namespace web.Areas.Admin.ViewModels.Comment;

public class CommentListItemViewModel
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorEmail { get; set; }
    public string? AuthorAvatar { get; set; }
    public bool IsApproved { get; set; }
    public int? ParentId { get; set; }
    public int ArticleId { get; set; }
    public string ArticleTitle { get; set; } = string.Empty;
    public string ArticleSlug { get; set; } = string.Empty;
    public int ReplyCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}