using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Product;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; // Rich Text Editor?
    public string? ShortDescription { get; set; }
    public string? Manufacturer { get; set; }
    public string? Origin { get; set; }
    public string? Specifications { get; set; } // Text Area
    public string? Usage { get; set; } // Text Area
    public string? Features { get; set; } // Text Area
    public string? PackagingInfo { get; set; } // Text Area
    public string? StorageInstructions { get; set; } // Text Area
    public string? SafetyInfo { get; set; } // Text Area
    public string? ApplicationAreas { get; set; } // Text Area
    public string? TechnicalDocuments { get; set; } // JSON - Text Area
    public bool IsFeatured { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public int ProductTypeId { get; set; }
    public PublishStatus Status { get; set; } = PublishStatus.Draft;
    public int? BrandId { get; set; }

    // --- Relationships ---
    public List<int> SelectedCategoryIds { get; set; } = new List<int>();
    public List<int> SelectedTagIds { get; set; } = new List<int>();
    public List<ProductImageViewModel> Images { get; set; } = new List<ProductImageViewModel>();
    public List<ProductVariantViewModel> Variants { get; set; } = new List<ProductVariantViewModel>();

    // --- SEO Fields ---
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public bool NoIndex { get; set; } = false;
    public bool NoFollow { get; set; } = false;
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; } // Stores full URL or Path
    public string? OgType { get; set; } = "product";
    public string? TwitterTitle { get; set; }
    public string? TwitterDescription { get; set; }
    public string? TwitterImage { get; set; } // Stores full URL or Path
    public string? TwitterCard { get; set; } = "summary_large_image";
    public string? SchemaMarkup { get; set; } // JSON-LD - Text Area
    public string? BreadcrumbJson { get; set; } // Text Area
    public double SitemapPriority { get; set; } = 0.8; // Higher priority for products
    public string SitemapChangeFrequency { get; set; } = "weekly";

    // --- Dropdown Data ---
    public SelectList? ProductTypeList { get; set; }
    public SelectList? BrandList { get; set; }
    public SelectList? CategoryList { get; set; } // Multi-select handled in View
    public SelectList? TagList { get; set; } // Multi-select handled in View
    public SelectList? StatusList { get; set; }
}