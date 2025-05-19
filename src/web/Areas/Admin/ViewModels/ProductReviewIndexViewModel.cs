using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class ProductReviewIndexViewModel
{
    public IPagedList<ProductReviewListItemViewModel> Reviews { get; set; } = default!;
    public ProductReviewFilterViewModel Filter { get; set; } = new();
}
