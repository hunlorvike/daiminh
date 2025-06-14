using X.PagedList;

namespace web.Areas.Client.ViewModels;

public class ArticleIndexViewModel
{
    public IPagedList<ArticleCardViewModel> Articles { get; set; } = default!;
    // Có thể thêm các thuộc tính khác như danh sách Category để lọc sau này
}