using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Setting;

public class SettingUpdateViewModel
{
    [Required]
    public List<SettingViewModel> Settings { get; set; } = new List<SettingViewModel>();

    public ILookup<string, SettingViewModel> SettingsByCategory =>
        Settings.OrderBy(s => GetCategoryOrder(s.Category))
                .ThenBy(s => s.Key)
                .ToLookup(s => s.Category);

    private static int GetCategoryOrder(string category)
    {
        return category switch
        {
            "General" => 1,
            "Contact" => 2,
            "Social Media" => 3,
            "Email" => 4,
            "SEO" => 5,
            "Theme" => 6,
            // Add other categories...
            _ => 99, // Default order for unknown categories
        };
    }
}