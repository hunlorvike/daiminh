// GalleryImageViewModel.cs
namespace web.Areas.Admin.ViewModels.Gallery;

public class GalleryImageViewModel
{
    public int Id { get; set; }
    public int GalleryId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? AltText { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int OrderIndex { get; set; }

    // For upload
    public IFormFile? ImageFile { get; set; }
}