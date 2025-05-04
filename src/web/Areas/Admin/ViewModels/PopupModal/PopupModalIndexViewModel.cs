using X.PagedList;

namespace web.Areas.Admin.ViewModels.PopupModal;

public class PopupModalIndexViewModel
{
    public IPagedList<PopupModalListItemViewModel> PopupModals { get; set; } = default!;
    public PopupModalFilterViewModel Filter { get; set; } = new();
}