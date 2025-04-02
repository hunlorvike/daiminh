using shared.Enums;

namespace web.Areas.Admin.ViewModels.Media;

public class MediaFileViewModel
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public string MediumSizePath { get; set; } = string.Empty;
    public string LargeSizePath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string AltText { get; set; } = string.Empty;
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? Duration { get; set; }
    public MediaType MediaType { get; set; }
    public int? FolderId { get; set; }

    // For file upload
    public IFormFile? FileUpload { get; set; }

    // For display purposes
    public IEnumerable<MediaFolderSelectViewModel>? AvailableFolders { get; set; }
}