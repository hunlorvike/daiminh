namespace web.Areas.Admin.ViewModels.Product;

public class ProductVariantViewModel
{
    public int Id { get; set; } // 0 for new variants
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
    public int StockQuantity { get; set; } = 0;
    public string? Color { get; set; }
    public string? Size { get; set; }
    public string? Packaging { get; set; }
    public string? ImageUrl { get; set; } // Optional: MinIO Path for variant-specific image
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false; // Flag for marking deletion on Edit
}