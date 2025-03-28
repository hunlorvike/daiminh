using shared.Enums;
using web.Areas.Admin.ViewModels.Category;
using web.Areas.Admin.ViewModels.ProductType;
using web.Areas.Admin.ViewModels.Tag;

namespace web.Areas.Admin.ViewModels.Product;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Manufacturer { get; set; }
    public string? Origin { get; set; }
    public string? Specifications { get; set; }
    public string? Usage { get; set; }
    public string? Features { get; set; }
    public string? PackagingInfo { get; set; }
    public string? StorageInstructions { get; set; }
    public string? SafetyInfo { get; set; }
    public string? ApplicationAreas { get; set; }
    public string? TechnicalDocuments { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public int ProductTypeId { get; set; }
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    // SEO fields
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public bool NoIndex { get; set; }
    public bool NoFollow { get; set; }

    // For multi-select
    public List<int> CategoryIds { get; set; } = new List<int>();
    public List<int> TagIds { get; set; } = new List<int>();

    // For file uploads
    public List<IFormFile>? ImageFiles { get; set; }

    // For display purposes
    public IEnumerable<ProductTypeViewModel>? ProductTypes { get; set; }
    public IEnumerable<CategoryViewModel>? Categories { get; set; }
    public IEnumerable<TagViewModel>? Tags { get; set; }
    public IEnumerable<ProductImageViewModel>? Images { get; set; }
}
