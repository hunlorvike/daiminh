using core.Common.Enums;
using core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class Product : SeoEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string Sku { get; set; } = string.Empty;
    public PublishStatus Status { get; set; }

    // Navigation properties
    public ProductType ProductType { get; set; }
    public ICollection<ProductFieldValue> FieldValues { get; set; }
    public ICollection<ProductImage> Images { get; set; }
    public ICollection<ProductCategory> ProductCategories { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; }
    public ICollection<ProductComment> Comments { get; set; }
    public ICollection<ProductReview> Reviews { get; set; }
}

public class ProductConfiguration : SeoEntityConfiguration<Product, int>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable("products");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.BasePrice).HasColumnName("base_price").HasPrecision(10, 2);
        builder.Property(e => e.Sku).HasColumnName("sku").HasMaxLength(50);
        builder.Property(e => e.Status).HasColumnName("status")
            .HasConversion(v => v.ToStringValue(), v => v.ToPublishStatusEnum()).IsRequired().HasMaxLength(20)
            .HasDefaultValue(PublishStatus.Draft);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_products_slug").IsUnique();
        builder.HasIndex(e => e.Sku).IsUnique();
    }
}