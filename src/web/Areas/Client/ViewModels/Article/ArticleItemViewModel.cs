namespace web.Areas.Client.ViewModels.Article;

public class ArticleItemViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? ThumbnailImage { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? AuthorName { get; set; }
    public int ViewCount { get; set; }
    public string? CategoryName { get; set; }
    public string? CategorySlug { get; set; }
    public int EstimatedReadingMinutes { get; set; }
}