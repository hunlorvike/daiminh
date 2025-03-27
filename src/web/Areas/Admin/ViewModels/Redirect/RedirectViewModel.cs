using System.ComponentModel.DataAnnotations;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Redirect;

public class RedirectViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập URL nguồn")]
    [Display(Name = "URL nguồn")]
    public string SourceUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập URL đích")]
    [Display(Name = "URL đích")]
    public string TargetUrl { get; set; } = string.Empty;

    [Display(Name = "Loại chuyển hướng")]
    public RedirectType Type { get; set; } = RedirectType.Permanent;

    [Display(Name = "Sử dụng Regex")]
    public bool IsRegex { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Số lần truy cập")]
    public int HitCount { get; set; }

    [Display(Name = "Ghi chú")]
    [MaxLength(255, ErrorMessage = "Ghi chú không được vượt quá 255 ký tự")]
    public string? Notes { get; set; }
}

