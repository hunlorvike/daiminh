// GalleryViewModel.cs
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Gallery;

public class GalleryViewModel
{
    public int Id { get; set; }

    // Basic information
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Cover image
    public string? CoverImage { get; set; }
    public IFormFile? CoverImageFile { get; set; }

    // Metadata
    public int ViewCount { get; set; }
    public bool IsFeatured { get; set; }
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    // SEO fields
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public bool NoIndex { get; set; }
    public bool NoFollow { get; set; }

    // Relationships
    public List<int> CategoryIds { get; set; } = new List<int>();
    public List<int> TagIds { get; set; } = new List<int>();

    // For display purposes
    public List<SelectItemViewModel> AvailableCategories { get; set; } = new List<SelectItemViewModel>();
    public List<SelectItemViewModel> AvailableTags { get; set; } = new List<SelectItemViewModel>();
    public List<GalleryImageViewModel> Images { get; set; } = new List<GalleryImageViewModel>();
}

public class SelectItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Selected { get; set; }
}