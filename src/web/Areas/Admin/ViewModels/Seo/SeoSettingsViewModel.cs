using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Seo;
public class SeoSettingsViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Khóa cài đặt không được để trống")]
    [Display(Name = "Khóa cài đặt")]
    public string Key { get; set; } = string.Empty;

    [Display(Name = "Giá trị")]
    public string? Value { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Ngày tạo")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Ngày cập nhật")]
    public DateTime? UpdatedAt { get; set; }
}
