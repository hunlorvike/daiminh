
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Media;

public class MediaItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsFolder { get; set; }
    public int? ParentId { get; set; }

    public string? ThumbnailUrl { get; set; } // URL to display (could be MinIO public URL or placeholder)
    public string? FilePath { get; set; } // Relative path in MinIO (ObjectName)
    public string? MimeType { get; set; }
    public long? FileSize { get; set; }
    public string? AltText { get; set; }
    public MediaType? MediaType { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Helper for UI
    public string DisplayIconClass => GetIconClass();

    private string GetIconClass()
    {
        if (IsFolder) return "ti ti-folder";
        return MediaType switch
        {
            shared.Enums.MediaType.Image => "ti ti-photo",
            shared.Enums.MediaType.Video => "ti ti-movie",
            shared.Enums.MediaType.Document => "ti ti-file-text",
            _ => "ti ti-file",
        };
    }
    public string FormattedFileSize => FormatFileSize(FileSize ?? 0);

    private static string FormatFileSize(long bytes)
    {
        if (bytes < 0) return "N/A";
        string[] suffix = { "B", "KB", "MB", "GB", "TB" };
        int i = 0;
        double dblSByte = bytes;
        if (bytes > 1024)
        {
            for (i = 0; (bytes / 1024) > 0; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }
        }
        return $"{dblSByte:0.##} {suffix[i]}";
    }
}
