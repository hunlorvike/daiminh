using core.Common.Enums;
using core.Entities.CMS;
using core.Entities.Shared;

namespace core.Entities.Ecommerce;

public class Product : ActivatableEntity
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public ProductType Type { get; set; }
    public Guid? ManufacturerId { get; set; }
    public Guid CategoryId { get; set; }
    public string SKU { get; set; } = null!;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? TechnicalSpecs { get; set; }
    public string? ApplicationGuide { get; set; }
    public string? Certificates { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }

    public Manufacturer? Manufacturer { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<ProductMedia> Medias { get; set; } = new List<ProductMedia>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}