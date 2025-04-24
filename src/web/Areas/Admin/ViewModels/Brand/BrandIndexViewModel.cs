using X.PagedList;

namespace web.Areas.Admin.ViewModels.Brand;

public class BrandIndexViewModel
{
    public IPagedList<BrandListItemViewModel> Brands { get; set; } = default!;
    public BrandFilterViewModel Filter { get; set; } = new();
}