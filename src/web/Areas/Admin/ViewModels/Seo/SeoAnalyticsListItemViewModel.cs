using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Seo;

public class SeoAnalyticsListItemViewModel
{
    public int Id { get; set; }

    [Display(Name = "Loại đối tượng")]
    public string EntityType { get; set; } = string.Empty;

    [Display(Name = "ID đối tượng")]
    public int EntityId { get; set; }

    [Display(Name = "Tiêu đề")]
    public string? EntityTitle { get; set; }

    [Display(Name = "URL")]
    public string? EntityUrl { get; set; }

    [Display(Name = "Số lần hiển thị")]
    public int Impressions { get; set; }

    [Display(Name = "Số lần click")]
    public int Clicks { get; set; }

    [Display(Name = "Tỷ lệ click (CTR)")]
    public double CTR { get; set; }

    [Display(Name = "Vị trí trung bình")]
    public double AveragePosition { get; set; }

    [Display(Name = "Ngày")]
    public DateTime Date { get; set; }
}
