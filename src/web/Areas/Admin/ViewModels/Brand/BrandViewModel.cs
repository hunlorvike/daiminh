using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Brand;

public class BrandViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên thương hiệu.")]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập slug.")]
    [StringLength(255)]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.")]
    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    [StringLength(2048, ErrorMessage = "Đường dẫn logo quá dài.")]
    public string? LogoUrl { get; set; }

    [StringLength(255)]
    public string? Website { get; set; }

    public bool IsActive { get; set; } = true;

    // --- SEO Fields ---
    [StringLength(100, ErrorMessage = "Meta Title không vượt quá 100 ký tự.")]
    public string? MetaTitle { get; set; }

    [StringLength(300, ErrorMessage = "Meta Description không vượt quá 300 ký tự.")]
    public string? MetaDescription { get; set; }

    [StringLength(200, ErrorMessage = "Meta Keywords không vượt quá 200 ký tự.")]
    public string? MetaKeywords { get; set; }

    [StringLength(255, ErrorMessage = "Canonical URL không vượt quá 255 ký tự.")]
    public string? CanonicalUrl { get; set; }

    public bool NoIndex { get; set; } = false;
    public bool NoFollow { get; set; } = false;

    [StringLength(100, ErrorMessage = "Open Graph Title không vượt quá 100 ký tự.")]
    public string? OgTitle { get; set; }

    [StringLength(300, ErrorMessage = "Open Graph Description không vượt quá 300 ký tự.")]
    public string? OgDescription { get; set; }

    [StringLength(255, ErrorMessage = "Open Graph Image URL không vượt quá 255 ký tự.")]
    public string? OgImage { get; set; }

    [StringLength(50)]
    public string? OgType { get; set; } = "website";

    [StringLength(100, ErrorMessage = "Twitter Title không vượt quá 100 ký tự.")]
    public string? TwitterTitle { get; set; }

    [StringLength(300, ErrorMessage = "Twitter Description không vượt quá 300 ký tự.")]
    public string? TwitterDescription { get; set; }

    [StringLength(255, ErrorMessage = "Twitter Image URL không vượt quá 255 ký tự.")]
    public string? TwitterImage { get; set; }

    [StringLength(50)]
    public string? TwitterCard { get; set; } = "summary_large_image";

    public string? SchemaMarkup { get; set; }

    public string? BreadcrumbJson { get; set; }

    [Range(0.0, 1.0, ErrorMessage = "Sitemap Priority phải từ 0.0 đến 1.0.")]
    public double SitemapPriority { get; set; } = 0.5;

    [Required(ErrorMessage = "Vui lòng chọn tần suất cập nhật sitemap.")]
    public string SitemapChangeFrequency { get; set; } = "monthly";
}