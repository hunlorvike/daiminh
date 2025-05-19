using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class FAQIndexViewModel
{
    public IPagedList<FAQListItemViewModel> FAQs { get; set; } = default!;
    public FAQFilterViewModel Filter { get; set; } = new();
}