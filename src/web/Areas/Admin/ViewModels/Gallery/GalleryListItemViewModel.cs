using shared.Enums;

namespace web.Areas.Admin.ViewModels.Gallery;

public class GalleryListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public int ViewCount { get; set; }
    public bool IsFeatured { get; set; }
    public PublishStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // For display purposes
    public int ImageCount { get; set; }
    public List<string> Categories { get; set; } = new List<string>();
    public List<string> Tags { get; set; } = new List<string>();
}