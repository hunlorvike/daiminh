using X.PagedList;

namespace web.Areas.Client.ViewModels;

public class ProductIndexViewModel
{
    public IPagedList<ProductCardViewModel> Products { get; set; } = default!;
    public ProductFilterViewModel Filter { get; set; } = new();
}