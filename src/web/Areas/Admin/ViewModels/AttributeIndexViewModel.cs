using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class AttributeIndexViewModel
{
    public IPagedList<AttributeListItemViewModel> Attributes { get; set; } = default!;
    public AttributeFilterViewModel Filter { get; set; } = new();
}
