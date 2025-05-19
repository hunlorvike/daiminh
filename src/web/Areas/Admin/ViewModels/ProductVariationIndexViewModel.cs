using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class ProductVariationIndexViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    public IPagedList<ProductVariationListItemViewModel> Variations { get; set; } = default!;

    public ProductVariationFilterViewModel Filter { get; set; } = new();
}