namespace web.Areas.Admin.ViewModels.Media;

public class MediaBrowserViewModel
{
    public int? CurrentFolderId { get; set; }
    public string? CurrentFolderName { get; set; }
    public int? ParentFolderId { get; set; }
    public List<MediaFolderListItemViewModel> Folders { get; set; } = new List<MediaFolderListItemViewModel>();
    public List<MediaFileListItemViewModel> Files { get; set; } = new List<MediaFileListItemViewModel>();
    public string? SearchTerm { get; set; }
    public string? FileType { get; set; } // image, video, document, etc.
    public int TotalItems => Folders.Count + Files.Count;

    // Breadcrumb navigation
    public List<(int Id, string Name)> Breadcrumbs { get; set; } = new List<(int Id, string Name)>();
}