using X.PagedList;

namespace web.Areas.Admin.ViewModels.Page;

public class PageIndexViewModel
{
    public IPagedList<PageListItemViewModel> Pages { get; set; } = default!;
    public PageFilterViewModel Filter { get; set; } = new();
}