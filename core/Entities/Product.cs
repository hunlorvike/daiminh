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
    public PublishStatus Status { get; set; } = PublishStatus.Draft;
    public int ProductTypeId { get; set; }

    // Navigation properties
    public virtual ProductType ProductType { get; set; } = new();
    public virtual ICollection<ProductFieldValue> FieldValues { get; set; } = new List<ProductFieldValue>();
    public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    public virtual ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}

public class ProductConfiguration : SeoEntityConfiguration<Product, int>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable("products");

        builder.Property(e => e.ProductTypeId).HasColumnName("product_type_id");
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.BasePrice).HasColumnName("base_price").HasPrecision(10, 2);
        builder.Property(e => e.Sku).HasColumnName("sku").HasMaxLength(50);
        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<PublishStatus>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(PublishStatus.Draft);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_products_slug").IsUnique();
        builder.HasIndex(e => e.Sku).IsUnique();

        builder.HasIndex(p => p.ProductTypeId)
            .HasDatabaseName("idx_products_product_type_id");

        builder.HasOne(p => p.ProductType)
            .WithMany(pt => pt.Products)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}