using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Seo;

public class SeoGeneralSettingsViewModel
{
    [Display(Name = "Tiêu đề mặc định")]
    public string DefaultTitle { get; set; } = string.Empty;

    [Display(Name = "Mô tả mặc định")]
    public string DefaultDescription { get; set; } = string.Empty;

    [Display(Name = "Từ khóa mặc định")]
    public string DefaultKeywords { get; set; } = string.Empty;

    [Display(Name = "Tên trang web")]
    public string SiteName { get; set; } = string.Empty;

    [Display(Name = "Google Analytics ID")]
    public string GoogleAnalyticsId { get; set; } = string.Empty;

    [Display(Name = "Google Tag Manager ID")]
    public string GoogleTagManagerId { get; set; } = string.Empty;

    [Display(Name = "Facebook App ID")]
    public string FacebookAppId { get; set; } = string.Empty;

    [Display(Name = "Twitter Username")]
    public string TwitterUsername { get; set; } = string.Empty;

    [Display(Name = "Nội dung thẻ Robots")]
    public string RobotsContent { get; set; } = "index, follow";

    [Display(Name = "Cài đặt Sitemap")]
    public string SitemapSettings { get; set; } = string.Empty;

    [Display(Name = "Dữ liệu có cấu trúc - Tổ chức")]
    public string StructuredDataOrganization { get; set; } = string.Empty;

    [Display(Name = "Dữ liệu có cấu trúc - Website")]
    public string StructuredDataWebsite { get; set; } = string.Empty;

    [Display(Name = "Mã tùy chỉnh trong thẻ Head")]
    public string CustomHeadCode { get; set; } = string.Empty;

    [Display(Name = "Mã tùy chỉnh cuối trang")]
    public string CustomFooterCode { get; set; } = string.Empty;
}
