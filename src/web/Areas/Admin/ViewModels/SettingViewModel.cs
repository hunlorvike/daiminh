using System.ComponentModel.DataAnnotations;
using shared.Enums;

namespace web.Areas.Admin.ViewModels;

public class SettingViewModel
{
    public int Id { get; set; }

    [Display(Name = "Khóa cấu hình")]
    public string Key { get; set; } = string.Empty;

    [Display(Name = "Loại dữ liệu")]
    public FieldType Type { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Giá trị")]
    public string? Value { get; set; }
}