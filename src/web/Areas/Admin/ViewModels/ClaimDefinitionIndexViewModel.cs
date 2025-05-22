using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class ClaimDefinitionIndexViewModel
{
    public IPagedList<ClaimDefinitionListItemViewModel> ClaimDefinitions { get; set; } = default!;
    public ClaimDefinitionFilterViewModel Filter { get; set; } = new();
}
