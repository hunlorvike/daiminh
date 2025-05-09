using X.PagedList;

namespace web.Areas.Client.ViewModels.Article;

public class ArticleIndexViewModel
{
    public IPagedList<ArticleItemViewModel> Articles { get; set; } = default!;
    public ArticleFilterViewModel Filter { get; set; } = new();
    public ArticleSideBarViewModel SideBar { get; set; } = new();
}

