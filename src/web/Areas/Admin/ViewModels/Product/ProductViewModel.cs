using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Product;
// Implement SEO Interface
public class ProductViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tên sản phẩm", Prompt = "Nhập tên sản phẩm")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug", Prompt = "ten-san-pham")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả chi tiết", Prompt = "Nhập mô tả đầy đủ cho sản phẩm")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Description { get; set; } = string.Empty; // Rich Text Editor

    [Display(Name = "Mô tả ngắn", Prompt = "Nhập mô tả ngắn gọn (tối đa 500 ký tự)")]
    [MaxLength(500)]
    public string? ShortDescription { get; set; }

    [Display(Name = "Nhà sản xuất", Prompt = "Tên nhà sản xuất")]
    [MaxLength(255)]
    public string? Manufacturer { get; set; }

    [Display(Name = "Xuất xứ", Prompt = "Ví dụ: Việt Nam, Mỹ, Đức")]
    [MaxLength(100)]
    public string? Origin { get; set; }

    [Display(Name = "Thông số kỹ thuật", Prompt = "Nhập thông số dưới dạng văn bản hoặc HTML")]
    public string? Specifications { get; set; } // Text Area or Rich Text

    [Display(Name = "Hướng dẫn sử dụng", Prompt = "Nhập hướng dẫn sử dụng")]
    public string? Usage { get; set; } // Text Area or Rich Text

    [Display(Name = "Tính năng nổi bật", Prompt = "Liệt kê các tính năng chính")]
    public string? Features { get; set; } // Text Area or Rich Text

    [Display(Name = "Thông tin đóng gói", Prompt = "Ví dụ: Thùng 20kg, Lon 5L")]
    public string? PackagingInfo { get; set; } // Text Area

    [Display(Name = "Hướng dẫn bảo quản", Prompt = "Cách bảo quản sản phẩm")]
    public string? StorageInstructions { get; set; } // Text Area

    [Display(Name = "Thông tin an toàn", Prompt = "Lưu ý về an toàn khi sử dụng")]
    public string? SafetyInfo { get; set; } // Text Area

    [Display(Name = "Khu vực ứng dụng", Prompt = "Các lĩnh vực hoặc nơi sản phẩm được sử dụng")]
    public string? ApplicationAreas { get; set; } // Text Area

    [Display(Name = "Tài liệu kỹ thuật (JSON)", Prompt = "[{\"name\": \"TDS\", \"url\": \"...\"}]")]
    public string? TechnicalDocuments { get; set; } // JSON - Text Area

    [Display(Name = "Nổi bật")]
    public bool IsFeatured { get; set; } = false;

    [Display(Name = "Hiển thị")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Loại sản phẩm")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public int ProductTypeId { get; set; }

    [Display(Name = "Trạng thái xuất bản")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    [Display(Name = "Thương hiệu")]
    public int? BrandId { get; set; } // Nullable

    // --- Relationships ---
    [Display(Name = "Danh mục")]
    [Required(ErrorMessage = "Vui lòng chọn một {0}.")]
    public int CategoryId { get; set; }

    [Display(Name = "Thẻ (Tags)")]
    public List<int> SelectedTagIds { get; set; } = new List<int>();

    [Display(Name = "Hình ảnh")]
    public List<ProductImageViewModel> Images { get; set; } = new List<ProductImageViewModel>();

    [Display(Name = "Biến thể & Kho hàng")]
    public List<ProductVariantViewModel> Variants { get; set; } = new List<ProductVariantViewModel>();

    // --- SEO Fields (from ISeoPropertiesViewModel) ---
    [Display(Name = "Meta Title", Prompt = "Tiêu đề SEO")][MaxLength(100)] public string? MetaTitle { get; set; }
    [Display(Name = "Meta Description", Prompt = "Mô tả SEO")][MaxLength(300)] public string? MetaDescription { get; set; }
    [Display(Name = "Meta Keywords", Prompt = "Từ khóa SEO")][MaxLength(200)] public string? MetaKeywords { get; set; }
    [Display(Name = "Canonical URL")][MaxLength(255)][Url] public string? CanonicalUrl { get; set; }
    [Display(Name = "No Index")] public bool NoIndex { get; set; } = false;
    [Display(Name = "No Follow")] public bool NoFollow { get; set; } = false;
    [Display(Name = "Open Graph Title")][MaxLength(100)] public string? OgTitle { get; set; }
    [Display(Name = "Open Graph Description")][MaxLength(300)] public string? OgDescription { get; set; }
    [Display(Name = "Open Graph Image (URL)", Prompt = "URL ảnh cho Facebook, Zalo...")][MaxLength(255)][Url] public string? OgImage { get; set; } // Stores full URL
    [Display(Name = "Open Graph Type")][MaxLength(50)] public string? OgType { get; set; } = "product";
    [Display(Name = "Twitter Title")][MaxLength(100)] public string? TwitterTitle { get; set; }
    [Display(Name = "Twitter Description")][MaxLength(300)] public string? TwitterDescription { get; set; }
    [Display(Name = "Twitter Image (URL)", Prompt = "URL ảnh cho Twitter")][MaxLength(255)][Url] public string? TwitterImage { get; set; } // Stores full URL
    [Display(Name = "Twitter Card Type")][MaxLength(50)] public string? TwitterCard { get; set; } = "summary_large_image";
    [Display(Name = "Schema Markup (JSON-LD)", Prompt = "Dữ liệu cấu trúc Schema.org")] public string? SchemaMarkup { get; set; }
    [Display(Name = "Breadcrumb JSON", Prompt = "Dữ liệu Breadcrumb JSON (nếu cần)")] public string? BreadcrumbJson { get; set; }
    [Display(Name = "Sitemap Priority (0.0 - 1.0)")][Range(0.0, 1.0)] public double SitemapPriority { get; set; } = 0.8;
    [Display(Name = "Sitemap Change Frequency")][Required] public string SitemapChangeFrequency { get; set; } = "weekly";

    // --- Dropdown Data ---
    public SelectList? ProductTypeList { get; set; }
    public SelectList? BrandList { get; set; }
    public SelectList? CategoryList { get; set; } // For multi-select
    public SelectList? TagList { get; set; } // For multi-select
    public SelectList? StatusList { get; set; }
}