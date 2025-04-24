using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Shared;

public class SeoViewModel
{
    [Display(Name = "Tiêu đề Meta (SEO)", Prompt = "Tiêu đề hiển thị trên tab trình duyệt và kết quả tìm kiếm (tối đa 100 ký tự)")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? MetaTitle { get; set; }

    [Display(Name = "Mô tả Meta (SEO)", Prompt = "Mô tả ngắn gọn hiển thị trong kết quả tìm kiếm (tối đa 300 ký tự)")]
    [MaxLength(300, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.MultilineText)]
    public string? MetaDescription { get; set; }

    [Display(Name = "Từ khóa Meta (SEO)", Prompt = "Các từ khóa liên quan, cách nhau bởi dấu phẩy (tối đa 200 ký tự)")]
    [MaxLength(200, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? MetaKeywords { get; set; }

    [Display(Name = "URL Canonical", Prompt = "URL chính thức của trang này nếu có nội dung trùng lặp")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.Url)]
    public string? CanonicalUrl { get; set; }

    [Display(Name = "Không lập chỉ mục (NoIndex)")]
    public bool NoIndex { get; set; } = false;

    [Display(Name = "Không theo dõi liên kết (NoFollow)")]
    public bool NoFollow { get; set; } = false;

    [Display(Name = "Tiêu đề Open Graph (Facebook, LinkedIn)", Prompt = "Tiêu đề hiển thị khi chia sẻ trên mạng xã hội (tối đa 100 ký tự)")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? OgTitle { get; set; }

    [Display(Name = "Mô tả Open Graph", Prompt = "Mô tả hiển thị khi chia sẻ trên mạng xã hội (tối đa 300 ký tự)")]
    [MaxLength(300, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.MultilineText)]
    public string? OgDescription { get; set; }

    [Display(Name = "Ảnh Open Graph", Prompt = "URL của ảnh hiển thị khi chia sẻ (tỷ lệ 1.91:1)")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? OgImage { get; set; }

    [Display(Name = "Loại Open Graph", Prompt = "Loại nội dung (website, article, video, etc.)")]
    public string? OgType { get; set; } = "website";

    [Display(Name = "Tiêu đề Twitter Card", Prompt = "Tiêu đề hiển thị trên Twitter Card (tối đa 100 ký tự)")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? TwitterTitle { get; set; }

    [Display(Name = "Mô tả Twitter Card", Prompt = "Mô tả hiển thị trên Twitter Card (tối đa 300 ký tự)")]
    [MaxLength(300, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.MultilineText)]
    public string? TwitterDescription { get; set; }

    [Display(Name = "Ảnh Twitter Card", Prompt = "URL của ảnh hiển thị trên Twitter Card (tỷ lệ 1:1 hoặc 1.91:1)")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? TwitterImage { get; set; }

    [Display(Name = "Loại Twitter Card", Prompt = "Loại Twitter Card (summary, summary_large_image, app, player)")]
    public string? TwitterCard { get; set; } = "summary_large_image";

    [Display(Name = "Schema Markup (JSON-LD)", Prompt = "Dữ liệu có cấu trúc dạng JSON-LD")]
    [DataType(DataType.MultilineText)]
    public string? SchemaMarkup { get; set; }

    [Display(Name = "Breadcrumb JSON-LD", Prompt = "Dữ liệu có cấu trúc dạng JSON-LD cho Breadcrumb")]
    public string? BreadcrumbJson { get; set; }

    [Display(Name = "Độ ưu tiên Sitemap", Prompt = "Giá trị từ 0.0 đến 1.0 (mặc định 0.5)")]
    [Range(0.0, 1.0, ErrorMessage = "{0} phải nằm trong khoảng từ {1} đến {2}.")]
    public double? SitemapPriority { get; set; } = 0.5;

    [Display(Name = "Tần suất thay đổi Sitemap")]
    [MaxLength(20)]
    public string? SitemapChangeFrequency { get; set; }
}