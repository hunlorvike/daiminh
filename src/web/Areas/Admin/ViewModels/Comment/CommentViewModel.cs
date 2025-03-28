namespace web.Areas.Admin.ViewModels.Comment;

public class CommentViewModel
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorEmail { get; set; }
    public string? AuthorAvatar { get; set; }
    public string? AuthorWebsite { get; set; }
    public bool IsApproved { get; set; }
    public int? ParentId { get; set; }
    public int ArticleId { get; set; }

    // For display purposes
    public string? ParentAuthorName { get; set; }
    public string ArticleTitle { get; set; } = string.Empty;
}
