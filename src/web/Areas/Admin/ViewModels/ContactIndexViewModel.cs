using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class ContactIndexViewModel
{
    public IPagedList<ContactListItemViewModel> Contacts { get; set; } = default!;
    public ContactFilterViewModel Filter { get; set; } = new();
}