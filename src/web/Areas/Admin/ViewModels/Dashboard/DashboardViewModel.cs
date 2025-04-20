// File: web/Areas/Admin/ViewModels/Dashboard/DashboardViewModel.cs
using Newtonsoft.Json; // Import Newtonsoft.Json

namespace web.Areas.Admin.ViewModels.Dashboard;

public class DashboardViewModel
{
    // --- Các thuộc tính thống kê cũ ---
    public int TotalPublishedArticles { get; set; }
    public int TotalDraftArticles { get; set; }
    public int TotalPendingArticles { get; set; }
    public int TotalActiveProducts { get; set; }
    public int TotalInactiveProducts { get; set; }
    public int TotalProductCategories { get; set; }
    public int TotalArticleCategories { get; set; }
    public int TotalFaqCategories { get; set; }
    public int TotalProductTags { get; set; }
    public int TotalArticleTags { get; set; }
    public int TotalNewContacts { get; set; }
    public int TotalProcessingContacts { get; set; }
    public int TotalActiveNewsletters { get; set; }
    public int TotalActiveTestimonials { get; set; }
    public int TotalActiveBrands { get; set; }
    public int TotalActiveUsers { get; set; }
    public int TotalActiveFAQs { get; set; }

    // --- Dữ liệu cho Biểu đồ ---

    /// <summary>
    /// Dữ liệu cho biểu đồ trạng thái bài viết (Pie Chart).
    /// </summary>
    public ChartData ArticleStatusChart { get; set; } = new();
    [JsonIgnore] // Không cần serialize chính object này nữa
    public string ArticleStatusChartJson => JsonConvert.SerializeObject(ArticleStatusChart);

    /// <summary>
    /// Dữ liệu cho biểu đồ liên hệ mới theo ngày (Line Chart).
    /// </summary>
    public ChartData RecentContactsChart { get; set; } = new();
    [JsonIgnore]
    public string RecentContactsChartJson => JsonConvert.SerializeObject(RecentContactsChart);

    /// <summary>
    /// Dữ liệu cho biểu đồ sản phẩm theo danh mục (Top 5 Bar Chart).
    /// </summary>
    public ChartData ProductCategoryChart { get; set; } = new();
    [JsonIgnore]
    public string ProductCategoryChartJson => JsonConvert.SerializeObject(ProductCategoryChart);

    // --- Các lớp hỗ trợ cho dữ liệu biểu đồ ---
    public class ChartData
    {
        public List<string> Labels { get; set; } = new();
        // Sử dụng List<ChartSeries> cho các biểu đồ có thể có nhiều series
        public List<ChartSeries> Series { get; set; } = new();
        // Hoặc dùng trực tiếp List<decimal> nếu chỉ có 1 series (như Pie/Donut)
        public List<decimal> SingleSeriesData { get; set; } = new();
    }

    public class ChartSeries
    {
        public string Name { get; set; } = string.Empty;
        public List<decimal> Data { get; set; } = new();
    }
}