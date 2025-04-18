using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.ViewModels.Project;
public class ProjectViewModel : ISeoPropertiesViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tên dự án")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ chứa chữ thường, số, dấu gạch ngang")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả chi tiết")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Description { get; set; } = string.Empty; // Rich Text

    [Display(Name = "Mô tả ngắn")]
    [MaxLength(500)]
    public string? ShortDescription { get; set; }

    [Display(Name = "Khách hàng / Chủ đầu tư")]
    [MaxLength(255)]
    public string? Client { get; set; }

    [Display(Name = "Địa điểm")]
    [MaxLength(255)]
    public string? Location { get; set; }

    [Display(Name = "Diện tích (m²)")]
    [Range(0, double.MaxValue, ErrorMessage = "{0} phải là số không âm")]
    public decimal? Area { get; set; }

    [Display(Name = "Ngày bắt đầu")]
    [DataType(DataType.DateTime)]
    public DateTime? StartDate { get; set; }

    [Display(Name = "Ngày hoàn thành")]
    [DataType(DataType.DateTime)]
    public DateTime? CompletionDate { get; set; }

    [Display(Name = "Ảnh đại diện (Featured)")]
    [MaxLength(2048)]
    public string? FeaturedImage { get; set; } // MinIO Path

    [Display(Name = "Ảnh thu nhỏ (Thumbnail)")]
    [MaxLength(2048)]
    public string? ThumbnailImage { get; set; } // MinIO Path

    [Display(Name = "Nổi bật")]
    public bool IsFeatured { get; set; } = false;

    [Display(Name = "Tình trạng dự án")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public ProjectStatus Status { get; set; } = ProjectStatus.InProgress;

    [Display(Name = "Trạng thái xuất bản")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public PublishStatus PublishStatus { get; set; } = PublishStatus.Draft;

    // --- Relationships ---
    [Display(Name = "Danh mục dự án")]
    [Required(ErrorMessage = "Vui lòng chọn một {0}")]
    public int CategoryId { get; set; }

    [Display(Name = "Thẻ (Tags)")]
    public List<int> SelectedTagIds { get; set; } = new List<int>();

    [Display(Name = "Sản phẩm sử dụng")]
    public List<int> SelectedProductIds { get; set; } = new List<int>();

    [Display(Name = "Bộ sưu tập ảnh dự án")]
    public List<ProjectImageViewModel> Images { get; set; } = new List<ProjectImageViewModel>();

    // --- SEO Fields ---
    [Display(Name = "Meta Title")][MaxLength(100)] public string? MetaTitle { get; set; }
    [Display(Name = "Meta Description")][MaxLength(300)] public string? MetaDescription { get; set; }
    [Display(Name = "Meta Keywords")][MaxLength(200)] public string? MetaKeywords { get; set; }
    [Display(Name = "Canonical URL")][MaxLength(255)][Url] public string? CanonicalUrl { get; set; }
    [Display(Name = "No Index")] public bool NoIndex { get; set; } = false;
    [Display(Name = "No Follow")] public bool NoFollow { get; set; } = false;
    [Display(Name = "Open Graph Title")][MaxLength(100)] public string? OgTitle { get; set; }
    [Display(Name = "Open Graph Description")][MaxLength(300)] public string? OgDescription { get; set; }
    [Display(Name = "Open Graph Image (URL/Path)")][MaxLength(255)] public string? OgImage { get; set; }
    [Display(Name = "Open Graph Type")][MaxLength(50)] public string? OgType { get; set; } = "article"; // Or custom type
    [Display(Name = "Twitter Title")][MaxLength(100)] public string? TwitterTitle { get; set; }
    [Display(Name = "Twitter Description")][MaxLength(300)] public string? TwitterDescription { get; set; }
    [Display(Name = "Twitter Image (URL/Path)")][MaxLength(255)] public string? TwitterImage { get; set; }
    [Display(Name = "Twitter Card Type")][MaxLength(50)] public string? TwitterCard { get; set; } = "summary_large_image";
    [Display(Name = "Schema Markup (JSON-LD)")] public string? SchemaMarkup { get; set; }
    [Display(Name = "Breadcrumb JSON")] public string? BreadcrumbJson { get; set; }
    [Display(Name = "Sitemap Priority (0.0 - 1.0)")][Range(0.0, 1.0)] public double SitemapPriority { get; set; } = 0.7;
    [Display(Name = "Sitemap Change Frequency")][Required] public string SitemapChangeFrequency { get; set; } = "monthly";

    // --- Dropdown Data ---
    public SelectList? CategoryList { get; set; }
    public SelectList? TagList { get; set; }
    public SelectList? ProductList { get; set; }
    public SelectList? StatusList { get; set; }
    public SelectList? PublishStatusList { get; set; }
}