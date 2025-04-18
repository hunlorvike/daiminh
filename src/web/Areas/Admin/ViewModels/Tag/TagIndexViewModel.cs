using X.PagedList;

namespace web.Areas.Admin.ViewModels.Tag;

public class TagIndexViewModel
{
    public IPagedList<TagListItemViewModel> Tags { get; set; } = default!;
    public TagFilterViewModel Filter { get; set; } = new();
}
