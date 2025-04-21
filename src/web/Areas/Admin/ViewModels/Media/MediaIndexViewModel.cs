using X.PagedList;

namespace web.Areas.Admin.ViewModels.Media;

public class MediaIndexViewModel
{
    public int? CurrentFolderId { get; set; }
    public List<MediaFolderViewModel> Breadcrumbs { get; set; } = new();
    public List<MediaFolderViewModel> Folders { get; set; } = new();
    public List<MediaFileViewModel> Files { get; set; } = new();
    public MediaFilterViewModel Filter { get; set; } = new();
    public IPagedList<MediaFileViewModel> PagedFiles { get; set; } = default!;
}
