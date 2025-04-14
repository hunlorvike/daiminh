using shared.Enums;
namespace web.Areas.Admin.ViewModels.Article;
public class ArticleListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ThumbnailImage { get; set; }
    public string? AuthorName { get; set; }
    public string? CategoryNames { get; set; }
    public ArticleType Type { get; set; }
    public PublishStatus Status { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime? UpdatedAt { get; set; }
}