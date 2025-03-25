using shared.Enums;

namespace web.Areas.Client.Models.Product;

public class ProductViewModel
{
    // Products
    public List<domain.Entities.Product> Products { get; set; } = new();

    // Pagination properties
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages { get; set; }
    public int TotalProducts { get; set; }

    // Filter properties
    public string Search { get; set; } = string.Empty;
    public List<int> SelectedCategoryIds { get; set; } = new();
    public List<int> SelectedTagIds { get; set; } = new();
    public List<int> SelectedProductTypeIds { get; set; } = new();
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string SortBy { get; set; } = "newest";
    public PublishStatus? Status { get; set; }
    public Dictionary<int, List<string>> FieldValues { get; set; } = new();

    // Filter options
    public List<domain.Entities.Category> Categories { get; set; } = new();
    public List<domain.Entities.Category> ParentCategories { get; set; } = new();
    public List<domain.Entities.Category> ChildCategories { get; set; } = new();
    public List<domain.Entities.Tag> Tags { get; set; } = new();
    public List<domain.Entities.ProductType> ProductTypes { get; set; } = new();
    public List<domain.Entities.ProductFieldDefinition> FieldDefinitions { get; set; } = new();
    public Dictionary<int, List<string>> AvailableFieldValues { get; set; } = new();

    // Price range
    public decimal MinAvailablePrice { get; set; }
    public decimal MaxAvailablePrice { get; set; }

    // Helper properties for UI
    public bool HasFilters => !string.IsNullOrEmpty(Search) ||
                             SelectedCategoryIds.Any() ||
                             SelectedTagIds.Any() ||
                             SelectedProductTypeIds.Any() ||
                             MinPrice.HasValue ||
                             MaxPrice.HasValue ||
                             Status.HasValue ||
                             FieldValues.Any();

    public int StartItem => (CurrentPage - 1) * PageSize + 1;
    public int EndItem => Math.Min(StartItem + PageSize - 1, TotalProducts);
}