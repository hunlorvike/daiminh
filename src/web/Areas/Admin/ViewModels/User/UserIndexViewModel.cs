using X.PagedList;

namespace web.Areas.Admin.ViewModels.User;

public class UserIndexViewModel
{
    public IPagedList<UserListItemViewModel> Users { get; set; } = default!;
    public UserFilterViewModel Filter { get; set; } = new();
}
