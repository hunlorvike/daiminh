using X.PagedList;

namespace web.Areas.Admin.ViewModels;

public class MediaIndexViewModel
{
    public IPagedList<MediaFileViewModel> MediaFiles { get; set; } = default!;
    public MediaFilterViewModel Filter { get; set; } = new();
}