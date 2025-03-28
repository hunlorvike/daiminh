using shared.Enums;

namespace web.Areas.Admin.ViewModels.Article;

public class ArticleViewModel
{
    public int Id { get; set; }

    // Basic information
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }

    // Images
    public string? FeaturedImage { get; set; }
    public string? ThumbnailImage { get; set; }
    public IFormFile? FeaturedImageFile { get; set; }
    public IFormFile? ThumbnailImageFile { get; set; }

    // Metadata
    public int ViewCount { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorAvatar { get; set; }
    public int EstimatedReadingMinutes { get; set; }
    public ArticleType Type { get; set; } = ArticleType.Knowledge;
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    // SEO fields
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public bool NoIndex { get; set; }
    public bool NoFollow { get; set; }

    // Relationships
    public List<int> CategoryIds { get; set; } = new List<int>();
    public List<int> TagIds { get; set; } = new List<int>();
    public List<int> ProductIds { get; set; } = new List<int>();

    // For display purposes
    public List<SelectItemViewModel> AvailableCategories { get; set; } = new List<SelectItemViewModel>();
    public List<SelectItemViewModel> AvailableTags { get; set; } = new List<SelectItemViewModel>();
    public List<SelectItemViewModel> AvailableProducts { get; set; } = new List<SelectItemViewModel>();
    public int CommentCount { get; set; }
}

public class SelectItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Selected { get; set; }
}