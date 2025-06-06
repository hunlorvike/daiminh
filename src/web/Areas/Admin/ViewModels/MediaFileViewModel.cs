using shared.Enums;
using shared.Extensions;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class MediaFileViewModel
{
    public int Id { get; set; }
    [Display(Name = "Tên file trên hệ thống")]
    public string FileName { get; set; } = string.Empty;

    [Display(Name = "Tên file gốc")]
    public string OriginalFileName { get; set; } = string.Empty;

    [Display(Name = "Loại MIME")]
    public string MimeType { get; set; } = string.Empty;

    [Display(Name = "Đuôi file")]
    public string FileExtension { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả cho tập tin này")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Description { get; set; }

    [Display(Name = "Văn bản thay thế (Alt Text)", Prompt = "Nhập alt text cho hình ảnh")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? AltText { get; set; }

    [Display(Name = "Kích thước")]
    public long FileSize { get; set; }

    public string FormattedFileSize => BytesToString(FileSize);

    [Display(Name = "Thời lượng (giây)")]
    public int? Duration { get; set; }

    [Display(Name = "Loại Media")]
    public MediaType MediaType { get; set; }

    public string MediaTypeDisplayName => MediaType.GetDisplayName();

    [Display(Name = "Ngày tải lên")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "URL truy cập")]
    public string? PresignedUrl { get; set; }

    private static string BytesToString(long byteCount)
    {
        string[] suf = ["B", "KB", "MB", "GB", "TB", "PB", "EB"];
        if (byteCount == 0)
            return "0" + suf[0];
        long bytes = Math.Abs(byteCount);
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        return (Math.Sign(byteCount) * num).ToString() + suf[place];
    }
}