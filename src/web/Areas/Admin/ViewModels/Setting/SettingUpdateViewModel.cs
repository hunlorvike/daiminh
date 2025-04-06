using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Setting;

public class SettingUpdateViewModel
{
    // Use List for model binding compatibility
    [Required] // Ensure the list itself is submitted
    public List<SettingViewModel> Settings { get; set; } = new List<SettingViewModel>();

    // Helper property for grouping in the View
    public ILookup<string, SettingViewModel> SettingsByCategory =>
        Settings.OrderBy(s => s.Key).ToLookup(s => s.Category);
}
