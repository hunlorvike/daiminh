using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class ArticleIndexViewModel
{
    public IPagedList<ArticleListItemViewModel> Articles { get; set; } = default!;
    public ArticleFilterViewModel Filter { get; set; } = new();
}