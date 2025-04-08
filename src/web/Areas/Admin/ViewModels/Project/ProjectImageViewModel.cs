namespace web.Areas.Admin.ViewModels.Project;

public class ProjectImageViewModel
{
    public int Id { get; set; } = 0;
    public string ImageUrl { get; set; } = string.Empty; // MinIO Path
    public string? ThumbnailUrl { get; set; } // Optional MinIO Thumb Path
    public string? AltText { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; } // Description for the image itself
    public int OrderIndex { get; set; } = 0;
    public bool IsMain { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
}
