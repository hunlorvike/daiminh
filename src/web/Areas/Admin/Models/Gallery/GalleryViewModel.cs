using domain.Entities;

namespace web.Areas.Admin.Models.Gallery;

public class GalleryViewModel
{
    public Folder? CurrentFolder { get; set; }
    public IEnumerable<Folder> Folders { get; set; } = new List<Folder>();
    public IEnumerable<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
    public IEnumerable<Folder> Breadcrumbs { get; set; } = new List<Folder>();

    public IEnumerable<Folder> AllFolders { get; set; } = new List<Folder>();
}