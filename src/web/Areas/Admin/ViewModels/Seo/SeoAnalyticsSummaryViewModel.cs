using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Seo;

public class SeoAnalyticsSummaryViewModel
{
    [Display(Name = "Tổng số lần hiển thị")]
    public int TotalImpressions { get; set; }

    [Display(Name = "Tổng số lần click")]
    public int TotalClicks { get; set; }

    [Display(Name = "Tỷ lệ click trung bình (CTR)")]
    public double AverageCTR { get; set; }

    [Display(Name = "Vị trí trung bình")]
    public double AveragePosition { get; set; }

    [Display(Name = "Từ khóa hàng đầu")]
    public List<KeywordViewModel> TopKeywords { get; set; } = new List<KeywordViewModel>();

    [Display(Name = "Trang hiệu suất cao nhất")]
    public List<TopPageViewModel> TopPages { get; set; } = new List<TopPageViewModel>();

    [Display(Name = "Dữ liệu theo ngày")]
    public List<DailyAnalyticsViewModel> DailyData { get; set; } = new List<DailyAnalyticsViewModel>();
}

public class KeywordViewModel
{
    public string Keyword { get; set; } = string.Empty;
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public double CTR { get; set; }
    public double AveragePosition { get; set; }
}

public class TopPageViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public double CTR { get; set; }
    public double AveragePosition { get; set; }
}

public class DailyAnalyticsViewModel
{
    public DateTime Date { get; set; }
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public double CTR { get; set; }
    public double AveragePosition { get; set; }
}