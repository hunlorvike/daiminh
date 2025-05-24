using System.ComponentModel.DataAnnotations;
using shared.Enums;
using shared.Extensions;

namespace web.Areas.Admin.ViewModels;

public class MediaFileViewModel
{
    public int Id { get; set; }
    [Display(Name = "Tên file")]
    public string FileName { get; set; } = string.Empty;
    [Display(Name = "Tên gốc")]
    public string OriginalFileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    [Display(Name = "Mô tả")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Description { get; set; }
    [Display(Name = "Alt Text")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? AltText { get; set; }
    public long FileSize { get; set; }
    public string FormattedFileSize => BytesToString(FileSize);
    public int? Duration { get; set; }
    public MediaType MediaType { get; set; }
    public string MediaTypeDisplayName => MediaType.GetDisplayName();
    public DateTime CreatedAt { get; set; }
    public string? PublicUrl { get; set; }

    private static string BytesToString(long byteCount)
    {
        string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
        if (byteCount == 0)
            return "0" + suf[0];
        long bytes = Math.Abs(byteCount);
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        return (Math.Sign(byteCount) * num).ToString() + suf[place];
    }
}