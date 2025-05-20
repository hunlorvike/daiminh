using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class RoleIndexViewModel
{
    public IPagedList<RoleListItemViewModel> Roles { get; set; } = default!;
    public string? SearchTerm { get; set; }
}
