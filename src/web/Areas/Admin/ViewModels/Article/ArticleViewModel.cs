using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Article;

public class ArticleViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Summernote editor
    public string? Summary { get; set; }
    public string? FeaturedImage { get; set; } // MinIO Path
    public string? ThumbnailImage { get; set; } // MinIO Path (Optional, can use Featured)
    public bool IsFeatured { get; set; } = false;
    public bool IsActive { get; set; }
    public int EstimatedReadingMinutes { get; set; }
    public DateTime? PublishedAt { get; set; } // DateTime picker
    public string? AuthorId { get; set; } // Optional, if linking to Users table
    public string? AuthorName { get; set; } // Display/Manual entry
    public string? AuthorAvatar { get; set; } // MinIO Path
    public ArticleType Type { get; set; } = ArticleType.Knowledge;
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    // --- Relationships ---
    public List<int> SelectedCategoryIds { get; set; } = new List<int>();
    public List<int> SelectedTagIds { get; set; } = new List<int>();
    public List<int> SelectedProductIds { get; set; } = new List<int>();

    // --- SEO Fields ---
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public bool NoIndex { get; set; } = false;
    public bool NoFollow { get; set; } = false;
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; } // MinIO Path or URL
    public string? OgType { get; set; } = "article";
    public string? TwitterTitle { get; set; }
    public string? TwitterDescription { get; set; }
    public string? TwitterImage { get; set; } // MinIO Path or URL
    public string? TwitterCard { get; set; } = "summary_large_image";
    public string? SchemaMarkup { get; set; } // JSON-LD
    public string? BreadcrumbJson { get; set; }
    public double SitemapPriority { get; set; } = 0.7;
    public string SitemapChangeFrequency { get; set; } = "weekly";

    // --- Dropdown Data ---
    public SelectList? CategoryList { get; set; }
    public SelectList? TagList { get; set; }
    public SelectList? ProductList { get; set; } // For related products
    public SelectList? StatusList { get; set; }
    public SelectList? TypeList { get; set; }
}