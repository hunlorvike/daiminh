namespace web.Areas.Admin.ViewModels.Shared;

public class KpiViewModel
{
    public int Count { get; set; }
    public double TrendPercentage { get; set; } // Phần trăm thay đổi so với kỳ trước
    public string TrendStatus { get; set; } = "neutral"; // "up", "down", "neutral"
}
