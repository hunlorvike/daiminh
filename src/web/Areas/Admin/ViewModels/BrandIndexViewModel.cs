using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class BrandIndexViewModel
{
    public IPagedList<BrandListItemViewModel> Brands { get; set; } = default!;
    public BrandFilterViewModel Filter { get; set; } = new();
}