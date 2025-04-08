using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Project;

public class ProjectViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; // Summernote
    public string? ShortDescription { get; set; }
    public string? Client { get; set; }
    public string? Location { get; set; }
    public decimal? Area { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string? FeaturedImage { get; set; } // MinIO Path
    public string? ThumbnailImage { get; set; } // MinIO Path
    public bool IsFeatured { get; set; } = false;
    public ProjectStatus Status { get; set; } = ProjectStatus.InProgress;
    public PublishStatus PublishStatus { get; set; } = PublishStatus.Draft;

    // --- Relationships ---
    public List<int> SelectedCategoryIds { get; set; } = new List<int>();
    public List<int> SelectedTagIds { get; set; } = new List<int>();
    // Products are handled differently due to the 'Usage' field
    public List<ProjectProductViewModel> ProjectProducts { get; set; } = new List<ProjectProductViewModel>();
    public List<ProjectImageViewModel> Images { get; set; } = new List<ProjectImageViewModel>();

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
    public string? OgType { get; set; } = "object"; // Or a more specific type if applicable
    public string? TwitterTitle { get; set; }
    public string? TwitterDescription { get; set; }
    public string? TwitterImage { get; set; } // MinIO Path or URL
    public string? TwitterCard { get; set; } = "summary_large_image";
    public string? SchemaMarkup { get; set; } // JSON-LD
    public string? BreadcrumbJson { get; set; }
    public double SitemapPriority { get; set; } = 0.6;
    public string SitemapChangeFrequency { get; set; } = "monthly";

    // --- Dropdown Data ---
    public SelectList? CategoryList { get; set; }
    public SelectList? TagList { get; set; }
    public SelectList? ProductList { get; set; } // For selecting products to add
    public SelectList? StatusList { get; set; }
    public SelectList? PublishStatusList { get; set; }
}
