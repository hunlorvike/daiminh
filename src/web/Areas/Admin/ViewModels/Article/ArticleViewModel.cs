using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.ViewModels.Article;
public class ArticleViewModel : ISeoPropertiesViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tiêu đề", Prompt = "Nhập tiêu đề bài viết")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Slug", Prompt = "tieu-de-bai-viet")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Nội dung chi tiết", Prompt = "Soạn thảo nội dung bài viết...")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Content { get; set; } = string.Empty; // Summernote

    [Display(Name = "Tóm tắt / Mô tả ngắn", Prompt = "Nhập mô tả ngắn gọn (hiển thị ở danh sách, tối đa 500 ký tự)")]
    [MaxLength(500)]
    public string? Summary { get; set; }

    [Display(Name = "Ảnh đại diện (Featured)", Prompt = "Chọn hoặc nhập đường dẫn ảnh MinIO")]
    [MaxLength(2048)]
    public string? FeaturedImage { get; set; } // MinIO Path

    [Display(Name = "Ảnh thu nhỏ (Thumbnail)", Prompt = "Chọn hoặc nhập đường dẫn ảnh MinIO (nếu khác ảnh Featured)")]
    [MaxLength(2048)]
    public string? ThumbnailImage { get; set; } // MinIO Path

    [Display(Name = "Nổi bật")]
    public bool IsFeatured { get; set; } = false;

    // Removed IsActive - Use Status instead for articles
    // public bool IsActive { get; set; }

    [Display(Name = "Ước tính phút đọc")]
    public int EstimatedReadingMinutes { get; set; } // Calculated field

    [Display(Name = "Ngày xuất bản", Prompt = "Để trống nếu muốn xuất bản ngay khi chuyển trạng thái")]
    public DateTime? PublishedAt { get; set; } // DateTime picker

    [Display(Name = "ID Tác giả (nếu có)")]
    [MaxLength(50)]
    public string? AuthorId { get; set; } // Optional

    [Display(Name = "Tên tác giả hiển thị", Prompt = "Tên sẽ hiển thị cùng bài viết")]
    [MaxLength(100)]
    public string? AuthorName { get; set; }

    [Display(Name = "Avatar tác giả (MinIO Path)", Prompt = "Chọn hoặc nhập đường dẫn ảnh MinIO")]
    [MaxLength(255)]
    public string? AuthorAvatar { get; set; } // MinIO Path

    [Display(Name = "Loại bài viết")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public ArticleType Type { get; set; } = ArticleType.Knowledge;

    [Display(Name = "Trạng thái")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    // --- Relationships ---
    [Display(Name = "Danh mục")]
    [Required(ErrorMessage = "Vui lòng chọn một {0}.")]
    public int CategoryId { get; set; }

    [Display(Name = "Thẻ (Tags)")]
    public List<int> SelectedTagIds { get; set; } = new List<int>();

    [Display(Name = "Sản phẩm liên quan")]
    public List<int> SelectedProductIds { get; set; } = new List<int>();

    // --- SEO Fields (from ISeoPropertiesViewModel) ---
    [Display(Name = "Meta Title")][MaxLength(100)] public string? MetaTitle { get; set; }
    [Display(Name = "Meta Description")][MaxLength(300)] public string? MetaDescription { get; set; }
    [Display(Name = "Meta Keywords")][MaxLength(200)] public string? MetaKeywords { get; set; }
    [Display(Name = "Canonical URL")][MaxLength(255)][Url] public string? CanonicalUrl { get; set; }
    [Display(Name = "No Index")] public bool NoIndex { get; set; } = false;
    [Display(Name = "No Follow")] public bool NoFollow { get; set; } = false;
    [Display(Name = "Open Graph Title")][MaxLength(100)] public string? OgTitle { get; set; }
    [Display(Name = "Open Graph Description")][MaxLength(300)] public string? OgDescription { get; set; }
    [Display(Name = "Open Graph Image (URL/Path)")][MaxLength(255)] public string? OgImage { get; set; } // Store Path or URL? Decide convention
    [Display(Name = "Open Graph Type")][MaxLength(50)] public string? OgType { get; set; } = "article";
    [Display(Name = "Twitter Title")][MaxLength(100)] public string? TwitterTitle { get; set; }
    [Display(Name = "Twitter Description")][MaxLength(300)] public string? TwitterDescription { get; set; }
    [Display(Name = "Twitter Image (URL/Path)")][MaxLength(255)] public string? TwitterImage { get; set; }
    [Display(Name = "Twitter Card Type")][MaxLength(50)] public string? TwitterCard { get; set; } = "summary_large_image";
    [Display(Name = "Schema Markup (JSON-LD)")] public string? SchemaMarkup { get; set; }
    [Display(Name = "Breadcrumb JSON")] public string? BreadcrumbJson { get; set; }
    [Display(Name = "Sitemap Priority (0.0 - 1.0)")][Range(0.0, 1.0)] public double SitemapPriority { get; set; } = 0.7;
    [Display(Name = "Sitemap Change Frequency")][Required] public string SitemapChangeFrequency { get; set; } = "weekly";

    // --- Dropdown Data ---
    public SelectList? CategoryList { get; set; }
    public SelectList? TagList { get; set; }
    public SelectList? ProductList { get; set; }
    public SelectList? StatusList { get; set; }
    public SelectList? TypeList { get; set; }
}