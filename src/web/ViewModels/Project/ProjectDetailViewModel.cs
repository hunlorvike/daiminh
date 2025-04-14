using shared.Enums;

namespace web.ViewModels.Project;

public class ProjectDetailViewModel
{
    // Core Project Details
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty; // For canonical URL etc.
    public string Description { get; set; } = string.Empty; // Rendered HTML
    public string? Client { get; set; }
    public string? Location { get; set; }
    public decimal? Area { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public ProjectStatus Status { get; set; }
    public string? FeaturedImage { get; set; } // MinIO Path

    // Related Data
    public List<ProjectCategoryLinkViewModel> Categories { get; set; } = new();
    public List<ProjectTagLinkViewModel> Tags { get; set; } = new();
    public List<ProjectGalleryImageViewModel> GalleryImages { get; set; } = new(); // Images excluding Featured
    public List<ProjectProductLinkViewModel> RelatedProducts { get; set; } = new();

    // SEO Meta Data (Populated from Project entity)
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; } // URL or Path (needs resolving)
    public string? OgType { get; set; }
    // Add Twitter tags if needed
}