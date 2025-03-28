namespace web.Areas.Admin.ViewModels.Product;

public class ProductImageViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? AltText { get; set; }
    public string? Title { get; set; }
    public int OrderIndex { get; set; }
    public bool IsMain { get; set; }
}
