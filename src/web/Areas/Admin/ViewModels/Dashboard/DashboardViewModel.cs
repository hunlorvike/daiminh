namespace web.Areas.Admin.ViewModels.Dashboard;

/// <summary>
/// ViewModel chứa dữ liệu tổng hợp cho trang Dashboard.
/// </summary>
public class DashboardViewModel
{
    // Số lượng bài viết theo trạng thái
    public int TotalPublishedArticles { get; set; }
    public int TotalDraftArticles { get; set; }

    // Số lượng sản phẩm theo trạng thái
    public int TotalActiveProducts { get; set; }
    public int TotalInactiveProducts { get; set; }

    // Số lượng danh mục theo loại
    public int TotalProductCategories { get; set; }
    public int TotalArticleCategories { get; set; }
    public int TotalFaqCategories { get; set; }

    // Số lượng thẻ theo loại
    public int TotalProductTags { get; set; }
    public int TotalArticleTags { get; set; }

    // Số lượng liên hệ mới
    public int TotalNewContacts { get; set; }

    // Số lượng đăng ký nhận tin
    public int TotalActiveNewsletters { get; set; }

    // Số lượng đánh giá
    public int TotalActiveTestimonials { get; set; }

    // Số lượng thương hiệu
    public int TotalActiveBrands { get; set; }

    // Số lượng người dùng
    public int TotalActiveUsers { get; set; }

    // Số lượng Media Files (có thể thêm nếu cần)
    public int TotalMediaFiles { get; set; }
    public int TotalMediaFolders { get; set; }

    // Số lượng FAQ
    public int TotalActiveFAQs { get; set; }

    public List<RecentArticleViewModel> RecentArticles { get; set; } = new();
    public List<RecentContactViewModel> RecentContacts { get; set; } = new();
}

public class RecentArticleViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public string? AuthorName { get; set; }
}

public class RecentContactViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}