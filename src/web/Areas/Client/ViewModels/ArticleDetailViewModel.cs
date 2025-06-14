using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Client.ViewModels;

public class ArticleDetailViewModel : SeoViewModel
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime PublishedAt { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorAvatar { get; set; }
    public string? CategoryName { get; set; }
    public string? CategorySlug { get; set; }
    public List<string> Tags { get; set; } = new();

    // Thêm danh sách bài viết liên quan
    public List<ArticleCardViewModel> RelatedArticles { get; set; } = new();
}