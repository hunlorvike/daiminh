using shared.Enums;

namespace web.Areas.Admin.ViewModels;

public class ProductListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? MainImageUrl { get; set; }
    public string? CategoryName { get; set; }
    public string? BrandName { get; set; }
    public PublishStatus Status { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public int ViewCount { get; set; }
    public int ImageCount { get; set; }
    public DateTime? UpdatedAt { get; set; }
}