using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Seo;

public class SeoSettingsListItemViewModel
{
    public int Id { get; set; }

    [Display(Name = "Khóa cài đặt")]
    public string Key { get; set; } = string.Empty;

    [Display(Name = "Giá trị")]
    public string? Value { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; }

    [Display(Name = "Ngày cập nhật")]
    public DateTime? UpdatedAt { get; set; }
}
