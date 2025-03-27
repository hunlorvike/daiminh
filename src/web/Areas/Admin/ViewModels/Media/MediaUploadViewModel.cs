namespace web.Areas.Admin.ViewModels.Media;

public class MediaUploadViewModel
{
    public int? FolderId { get; set; }
    public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    public string Description { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;

    // For display purposes
    public IEnumerable<MediaFolderSelectViewModel>? AvailableFolders { get; set; }
}
