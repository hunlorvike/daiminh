namespace web.Areas.Admin.ViewModels.Product;

public class ProductImageViewModel
{
    public int Id { get; set; } // 0 for new images
    public string ImageUrl { get; set; } = string.Empty; // MinIO Path (ObjectName)
    public string? ThumbnailUrl { get; set; } // Optional: Path to a specific thumbnail size in MinIO
    public string? AltText { get; set; }
    public string? Title { get; set; }
    public int OrderIndex { get; set; } = 0;
    public bool IsMain { get; set; } = false;
    public bool IsDeleted { get; set; } = false; // Flag for marking deletion on Edit
}
