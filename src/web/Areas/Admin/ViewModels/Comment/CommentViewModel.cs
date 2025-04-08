namespace web.Areas.Admin.ViewModels.Comment;

public class CommentViewModel
{
    public int Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorEmail { get; set; }
    public string? AuthorWebsite { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; } // Display Only
    public string? ArticleTitle { get; set; } // Display Only
    public int ArticleId { get; set; } // Hidden/Display Only
}