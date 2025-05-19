using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class BannerIndexViewModel
{
    public IPagedList<BannerListItemViewModel> Banners { get; set; } = default!;
    public BannerFilterViewModel Filter { get; set; } = new();
}