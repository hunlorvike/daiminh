using shared.Enums;

namespace web.Areas.Admin.ViewModels.Banner;

public class BannerListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? LinkUrl { get; set; }
    public BannerType Type { get; set; }
    public bool IsActive { get; set; }
    public int OrderIndex { get; set; }
    public DateTime? UpdatedAt { get; set; }
}