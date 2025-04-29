using X.PagedList;

namespace web.Areas.Admin.ViewModels.Product;

public class ProductIndexViewModel
{
    public IPagedList<ProductListItemViewModel> Products { get; set; } = default!;
    public ProductFilterViewModel Filter { get; set; } = new();
}