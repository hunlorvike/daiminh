using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class SettingsIndexViewModel
{
    public Dictionary<string, List<SettingViewModel>> SettingGroups { get; set; } = new();

    [Display(Name = "Tìm kiếm theo khóa hoặc mô tả")]
    public string? SearchTerm { get; set; }
}