using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Client.ViewModels;

public class ProductDetailViewModel : SeoViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? Specifications { get; set; }
    public string? Usage { get; set; }

    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? CategorySlug { get; set; }

    public List<ProductImageClientViewModel> Images { get; set; } = new();
}