using domain.Entities;

namespace web.Areas.Admin.Models.Gallery;

public class GalleryViewModel
{
    public Folder? CurrentFolder { get; set; }
    public List<Folder> Folders { get; set; } = new();
    public List<MediaFile> MediaFiles { get; set; } = new();
    public List<Folder> Breadcrumbs { get; set; } = new();
}