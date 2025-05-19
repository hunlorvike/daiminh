using shared.Enums;

namespace web.Areas.Admin.ViewModels;

public class ArticleListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public string? AuthorName { get; set; }
    public DateTime? PublishedAt { get; set; }
    public PublishStatus Status { get; set; }
    public bool IsFeatured { get; set; }
    public int ViewCount { get; set; }
    public int TagCount { get; set; }
    public int ProductCount { get; set; }
    public DateTime? UpdatedAt { get; set; }
}