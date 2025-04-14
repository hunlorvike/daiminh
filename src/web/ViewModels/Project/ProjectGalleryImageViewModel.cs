namespace web.ViewModels.Project;

public class ProjectGalleryImageViewModel
{
    public string ImageUrl { get; set; } = string.Empty; // MinIO Path
    public string? ThumbnailUrl { get; set; } // MinIO Path
    public string? AltText { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}