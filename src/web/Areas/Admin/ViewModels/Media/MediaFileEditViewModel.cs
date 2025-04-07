using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.ViewModels.Media;

public class MediaFileEditViewModel
{
    [Required]
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tên tệp")]
    [StringLength(255, ErrorMessage = "Alt Text không được vượt quá 255 ký tự.")]
    public string? AltText { get; set; }

    [Display(Name = "Mô tả")]
    [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự.")]
    public string? Description { get; set; }

    // Display only properties (optional)
    public string FileName { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}