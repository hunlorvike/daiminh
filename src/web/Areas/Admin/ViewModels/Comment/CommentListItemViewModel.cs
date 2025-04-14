namespace web.Areas.Admin.ViewModels.Comment;
public class CommentListItemViewModel
{
    public int Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorEmail { get; set; }
    public string ContentExcerpt { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; } // Added
    public int ArticleId { get; set; }
    public string? ArticleTitle { get; set; }
    public int ReplyCount { get; set; }
}