using X.PagedList;

namespace web.Areas.Admin.ViewModels.AttributeValue;

public class AttributeValueIndexViewModel
{
    public IPagedList<AttributeValueListItemViewModel> AttributeValues { get; set; } = default!;
    public AttributeValueFilterViewModel Filter { get; set; } = new();
}
