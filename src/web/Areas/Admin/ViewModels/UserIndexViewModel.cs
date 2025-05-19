using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class UserIndexViewModel
{
    public IPagedList<UserListItemViewModel> Users { get; set; } = default!;
    public UserFilterViewModel Filter { get; set; } = new();
}