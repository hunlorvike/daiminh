using shared.Enums;

namespace web.Areas.Admin.ViewModels.Product;

public class ProductListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? MainImageUrl { get; set; }
    public string ProductTypeName { get; set; } = string.Empty;
    public int CategoryCount { get; set; }
    public int TagCount { get; set; }
    public int ImageCount { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public PublishStatus Status { get; set; }
    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
