namespace web.Areas.Admin.ViewModels;

public class ProductVariationListItemViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty; // For display context if needed
    public string AttributeValueCombination { get; set; } = string.Empty; // e.g., "Đỏ / 5 Lít"
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
