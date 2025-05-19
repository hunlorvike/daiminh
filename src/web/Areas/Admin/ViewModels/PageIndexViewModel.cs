using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class PageIndexViewModel
{
    public IPagedList<PageListItemViewModel> Pages { get; set; } = default!;
    public PageFilterViewModel Filter { get; set; } = new();
}