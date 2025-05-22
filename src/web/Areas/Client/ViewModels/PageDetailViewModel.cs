namespace web.Areas.Client.ViewModels;

public class PageDetailViewModel
{
    // Thông tin chính của trang
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime? PublishedAt { get; set; }

    // Thông tin seo
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public bool NoIndex { get; set; } = false;
    public bool NoFollow { get; set; } = false;
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
    public double? SitemapPriority { get; set; }
    public string? SitemapChangeFrequency { get; set; }
}