using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Models.Setting;

public class SettingViewModel
{
    public int Id { get; set; }

    [Display(Name = "Key")] public string? Key { get; set; }

    [Display(Name = "Nhóm các Key")] public string? Group { get; set; }

    [Display(Name = "Giá trị")] public string? Value { get; set; }

    [Display(Name = "Ngày tạo")] public DateTime? CreatedAt { get; set; }

    [Display(Name = "Mô tả")] public string? Description { get; set; }

    [Display(Name = "Thứ tự hiển thị")] public int Order { get; set; }
}