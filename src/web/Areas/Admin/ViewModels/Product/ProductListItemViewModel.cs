using shared.Enums;

namespace web.Areas.Admin.ViewModels.Product;

public class ProductListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? MainImageUrl { get; set; }
    public string? ProductTypeName { get; set; }
    public string? BrandName { get; set; }
    public PublishStatus Status { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public int ViewCount { get; set; }
    public DateTime? UpdatedAt { get; set; }
}