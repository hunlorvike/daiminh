using shared.Enums;

namespace web.ViewModels.Project;

public class ProjectListItemViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ThumbnailOrFeaturedImageUrl { get; set; } // MinIO Path
    public string? ShortDescription { get; set; }
    public string? Location { get; set; }
    public string? PrimaryCategoryName { get; set; } // Optional: Display one main category
    public string? PrimaryCategorySlug { get; set; } // Optional: For category link
    public DateTime? CompletionDate { get; set; } // To display year or status
    public ProjectStatus Status { get; set; } // For display (e.g., "Completed")
}