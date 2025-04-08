namespace web.Areas.Admin.ViewModels.Comment;


public class CommentListItemViewModel
{
    public int Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorEmail { get; set; }
    public string ContentExcerpt { get; set; } = string.Empty; // Short version of content
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ArticleId { get; set; }
    public string? ArticleTitle { get; set; } // Title of the article it belongs to
    public int ReplyCount { get; set; } // Number of replies
}