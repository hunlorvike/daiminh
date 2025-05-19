using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class NewsletterIndexViewModel
{
    public IPagedList<NewsletterListItemViewModel> Newsletters { get; set; } = default!;
    public NewsletterFilterViewModel Filter { get; set; } = new();
}