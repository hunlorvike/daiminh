using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Category;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? ImageUrl { get; set; } // Stores MinIO Path (ObjectName)
    public int? ParentId { get; set; }
    public int OrderIndex { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public CategoryType Type { get; set; } = CategoryType.Product;

    // --- SEO Fields ---
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public bool NoIndex { get; set; } = false;
    public bool NoFollow { get; set; } = false;
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; } // Stores full URL or Path (decide based on need)
    public string? OgType { get; set; } = "website";
    public string? TwitterTitle { get; set; }
    public string? TwitterDescription { get; set; }
    public string? TwitterImage { get; set; } // Stores full URL or Path
    public string? TwitterCard { get; set; } = "summary_large_image";
    public string? SchemaMarkup { get; set; }
    public string? BreadcrumbJson { get; set; }
    public double SitemapPriority { get; set; } = 0.5;
    public string SitemapChangeFrequency { get; set; } = "monthly";

    // --- Helper for Dropdown ---
    public SelectList? ParentCategoryList { get; set; }
}