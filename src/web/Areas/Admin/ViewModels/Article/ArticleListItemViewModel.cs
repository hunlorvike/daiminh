using shared.Enums;

namespace web.Areas.Admin.ViewModels.Article;

public class ArticleListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? ThumbnailImage { get; set; }
    public int ViewCount { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? AuthorName { get; set; }
    public int CommentCount { get; set; }
    public ArticleType Type { get; set; }
    public PublishStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // For display purposes
    public List<string> Categories { get; set; } = new List<string>();
    public List<string> Tags { get; set; } = new List<string>();
}
