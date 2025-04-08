using shared.Enums;

namespace web.Areas.Admin.ViewModels.Project;

public class ProjectListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ThumbnailImage { get; set; } // MinIO Path
    public string? Location { get; set; }
    public string? Client { get; set; }
    public ProjectStatus Status { get; set; }
    public PublishStatus PublishStatus { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime UpdatedAt { get; set; }
}
