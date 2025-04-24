using X.PagedList;

namespace web.Areas.Admin.ViewModels.Newsletter;

public class NewsletterIndexViewModel
{
    public IPagedList<NewsletterListItemViewModel> Newsletters { get; set; } = default!;
    public NewsletterFilterViewModel Filter { get; set; } = new();
}