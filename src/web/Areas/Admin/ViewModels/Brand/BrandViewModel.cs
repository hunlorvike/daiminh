using System.ComponentModel.DataAnnotations;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.ViewModels.Brand;

public class BrandViewModel : ISeoPropertiesViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tên thương hiệu", Prompt = "Nhập tên thương hiệu")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [StringLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug", Prompt = "url-thuong-hieu")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [StringLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả chi tiết về thương hiệu")]
    public string? Description { get; set; }

    [Display(Name = "Logo (MinIO Path)", Prompt = "Chọn hoặc nhập đường dẫn logo")]
    [StringLength(2048, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? LogoUrl { get; set; } // Stores MinIO Path

    [Display(Name = "Website", Prompt = "https://www.example.com")]
    [StringLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [Url(ErrorMessage = "{0} không phải là một URL hợp lệ.")]
    public string? Website { get; set; }

    [Display(Name = "Kích hoạt", Prompt = "Chọn trạng thái kích hoạt")]
    public bool IsActive { get; set; } = true;

    // --- SEO Fields --- From ISeoPropertiesViewModel
    [Display(Name = "Meta Title", Prompt = "Tiêu đề SEO hiển thị trên Google")]
    [StringLength(100, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    public string? MetaTitle { get; set; }

    [Display(Name = "Meta Description", Prompt = "Mô tả SEO hiển thị trên Google")]
    [StringLength(300, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    public string? MetaDescription { get; set; }

    [Display(Name = "Meta Keywords", Prompt = "Từ khóa SEO (cách nhau bởi dấu phẩy)")]
    [StringLength(200, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    public string? MetaKeywords { get; set; }

    [Display(Name = "Canonical URL", Prompt = "https://yourdomain.com/preferred-brand-url")]
    [StringLength(255, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    [Url(ErrorMessage = "{0} không phải là một URL hợp lệ.")]
    public string? CanonicalUrl { get; set; }

    [Display(Name = "No Index")]
    public bool NoIndex { get; set; } = false;

    [Display(Name = "No Follow")]
    public bool NoFollow { get; set; } = false;

    [Display(Name = "Open Graph Title", Prompt = "Tiêu đề khi chia sẻ link (Facebook, Zalo...)")]
    [StringLength(100, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    public string? OgTitle { get; set; }

    [Display(Name = "Open Graph Description", Prompt = "Mô tả ngắn khi chia sẻ link")]
    [StringLength(300, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    public string? OgDescription { get; set; }

    [Display(Name = "Open Graph Image (URL)", Prompt = "Nhập URL hoặc chọn ảnh (tỷ lệ 1.91:1)")]
    [StringLength(255, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    [Url(ErrorMessage = "{0} không phải là một URL hợp lệ.")]
    public string? OgImage { get; set; } // Stores Full URL

    [Display(Name = "Open Graph Type", Prompt = "website, product.group, article")]
    [StringLength(50, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    public string? OgType { get; set; } = "website";

    [Display(Name = "Twitter Title", Prompt = "Tiêu đề khi chia sẻ link trên Twitter")]
    [StringLength(100, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    public string? TwitterTitle { get; set; }

    [Display(Name = "Twitter Description", Prompt = "Mô tả ngắn khi chia sẻ link trên Twitter")]
    [StringLength(300, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    public string? TwitterDescription { get; set; }

    [Display(Name = "Twitter Image (URL)", Prompt = "Nhập URL hoặc chọn ảnh")]
    [StringLength(255, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    [Url(ErrorMessage = "{0} không phải là một URL hợp lệ.")]
    public string? TwitterImage { get; set; } // Stores Full URL

    [Display(Name = "Twitter Card Type")]
    [StringLength(50, ErrorMessage = "{0} không vượt quá {1} ký tự.")]
    public string? TwitterCard { get; set; } = "summary_large_image";

    [Display(Name = "Schema Markup (JSON-LD)", Prompt = "Dữ liệu cấu trúc JSON-LD")]
    public string? SchemaMarkup { get; set; } // Text type

    [Display(Name = "Breadcrumb JSON", Prompt = "Dữ liệu JSON cho breadcrumbs (nếu cần)")]
    public string? BreadcrumbJson { get; set; } // Text type

    [Display(Name = "Sitemap Priority (0.0 - 1.0)", Prompt = "Độ ưu tiên trong sitemap")]
    [Range(0.0, 1.0, ErrorMessage = "{0} phải nằm trong khoảng từ {1} đến {2}.")]
    public double SitemapPriority { get; set; } = 0.5; // Default changed from 0.6 to 0.5 for consistency

    [Display(Name = "Sitemap Change Frequency")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public string? SitemapChangeFrequency { get; set; } = "monthly"; // Default changed from weekly to monthly
}