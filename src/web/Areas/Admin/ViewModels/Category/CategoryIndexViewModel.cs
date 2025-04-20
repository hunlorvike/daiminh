using X.PagedList;

namespace web.Areas.Admin.ViewModels.Category;

public class CategoryIndexViewModel
{
    public IPagedList<CategoryListItemViewModel> Categories { get; set; } = default!;
    public CategoryFilterViewModel Filter { get; set; } = new();
}
