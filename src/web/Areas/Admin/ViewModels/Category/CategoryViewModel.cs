using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Category;

public class CategoryViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} không được để trống")]
    [Display(Name = "Tên danh mục", Prompt = "Nhập tên danh mục")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} không được để trống")]
    [Display(Name = "Slug (URL thân thiện)", Prompt = "Nhập slug URL (không dấu, chữ thường, gạch ngang)")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả ngắn cho danh mục (không bắt buộc)")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? Description { get; set; }

    [Display(Name = "Icon (Ví dụ: ti ti-tag)", Prompt = "Nhập tên biểu tượng hoặc mã icon ví dụ: ti ti-tag")]
    [MaxLength(50, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? Icon { get; set; }

    [Display(Name = "Ảnh đại diện (MinIO Path)", Prompt = "Nhập đường dẫn ảnh đại diện")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Danh mục cha", Prompt = "Chọn danh mục cha (nếu có)")]
    public int? ParentId { get; set; }

    [Display(Name = "Thứ tự hiển thị", Prompt = "Nhập thứ tự hiển thị (số không âm)")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm")]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Hiển thị", Prompt = "Chọn trạng thái hiển thị")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Loại danh mục", Prompt = "Chọn loại danh mục")]
    public CategoryType Type { get; set; } = CategoryType.Product;

    // --- SEO Fields ---
    [Display(Name = "Meta Title", Prompt = "Nhập tiêu đề SEO (Meta Title)")]
    public string? MetaTitle { get; set; }

    [Display(Name = "Meta Description", Prompt = "Nhập mô tả SEO (Meta Description)")]
    public string? MetaDescription { get; set; }

    [Display(Name = "Meta Keywords", Prompt = "Nhập từ khóa SEO (Meta Keywords)")]
    public string? MetaKeywords { get; set; }

    [Display(Name = "Canonical URL", Prompt = "https://yourdomain.com/preferred-url")]
    public string? CanonicalUrl { get; set; }

    [Display(Name = "Không Index (NoIndex)", Prompt = "Chọn nếu không muốn index danh mục này")]
    public bool NoIndex { get; set; } = false;

    [Display(Name = "Không Follow (NoFollow)", Prompt = "Chọn nếu không muốn follow danh mục này")]
    public bool NoFollow { get; set; } = false;

    [Display(Name = "Open Graph Title", Prompt = "Nhập tiêu đề Open Graph (OG Title)")]
    public string? OgTitle { get; set; }

    [Display(Name = "Open Graph Description", Prompt = "Nhập mô tả Open Graph (OG Description)")]
    public string? OgDescription { get; set; }

    [Display(Name = "Open Graph Image (URL/Path)", Prompt = "Nhập đường dẫn ảnh Open Graph (OG Image tỷ lệ 1.91:1)")]
    public string? OgImage { get; set; }

    [Display(Name = "Open Graph Type", Prompt = "Nhập loại Open Graph (OG Type)")]
    public string? OgType { get; set; } = "website";

    [Display(Name = "Twitter Title", Prompt = "Nhập tiêu đề Twitter")]
    public string? TwitterTitle { get; set; }

    [Display(Name = "Twitter Description", Prompt = "Nhập mô tả Twitter")]
    public string? TwitterDescription { get; set; }

    [Display(Name = "Twitter Image (URL/Path)", Prompt = "Nhập đường dẫn ảnh Twitter")]
    public string? TwitterImage { get; set; }

    [Display(Name = "Twitter Card Type", Prompt = "Nhập loại thẻ Twitter (Twitter Card Type)")]
    public string? TwitterCard { get; set; } = "summary_large_image";

    [Display(Name = "Schema Markup (JSON-LD)", Prompt = "Ví dụ: { \"@context\": \"https://schema.org\", \"@type\": \"CollectionPage\", ... }")]
    public string? SchemaMarkup { get; set; }

    [Display(Name = "Breadcrumb JSON", Prompt = "Ví dụ: [{ \"@type\": \"ListItem\", \"position\": 1, \"name\": \"...\", \"item\": \"...\" }]")]
    public string? BreadcrumbJson { get; set; }

    [Display(Name = "Sitemap Priority (0.0 - 1.0)", Prompt = "Nhập độ ưu tiên trong sitemap (0.0 - 1.0)")]
    [Range(0.0, 1.0, ErrorMessage = "{0} phải nằm trong khoảng từ {1} đến {2}")]
    public double SitemapPriority { get; set; } = 0.7;

    [Display(Name = "Sitemap Change Frequency", Prompt = "Nhập tần suất thay đổi trong sitemap")]
    public string? SitemapChangeFrequency { get; set; } = "weekly";

    public SelectList? ParentCategoryList { get; set; }
}
