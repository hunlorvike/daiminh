namespace web.Areas.Client.ViewModels.Article;

public class ArticleDetailViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? FeaturedImage { get; set; }
    public int ViewCount { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorAvatar { get; set; }
    public int EstimatedReadingMinutes { get; set; }
    public string? CategoryName { get; set; }
    public string? CategorySlug { get; set; }

    public List<ArticleTagViewModel> Tags { get; set; } = new();
    public List<ArticleProductViewModel> RelatedProducts { get; set; } = new();
    public List<ArticleItemViewModel> RelatedArticles { get; set; } = new();

    // SEO Properties
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public bool NoIndex { get; set; }
    public bool NoFollow { get; set; }
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string? OgType { get; set; }
    public string? TwitterTitle { get; set; }
    public string? TwitterDescription { get; set; }
    public string? TwitterImage { get; set; }
    public string? TwitterCard { get; set; }
    public string? SchemaMarkup { get; set; }
    public string? BreadcrumbJson { get; set; }
}

public class ArticleTagViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}

public class ArticleProductViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }
}