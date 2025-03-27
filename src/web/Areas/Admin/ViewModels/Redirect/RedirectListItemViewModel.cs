using shared.Enums;

namespace web.Areas.Admin.ViewModels.Redirect;

public class RedirectListItemViewModel
{
    public int Id { get; set; }
    public string SourceUrl { get; set; } = string.Empty;
    public string TargetUrl { get; set; } = string.Empty;
    public RedirectType Type { get; set; }
    public bool IsRegex { get; set; }
    public bool IsActive { get; set; }
    public int HitCount { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Helper properties
    public string TypeName => Type == RedirectType.Permanent ? "301 (Permanent)" : "302 (Temporary)";
    public string StatusName => IsActive ? "Hoạt động" : "Không hoạt động";
}