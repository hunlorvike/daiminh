using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ProductType : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Product>? Products { get; set; }
    public virtual ICollection<ProductFieldDefinition>? FieldDefinitions { get; set; }
}

public class ProductTypeConfiguration : BaseEntityConfiguration<ProductType, int>
{
    public override void Configure(EntityTypeBuilder<ProductType> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_types");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(e => e.Icon).HasColumnName("icon").HasMaxLength(50);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_product_types_slug").IsUnique();
        builder.HasIndex(e => e.IsActive).HasDatabaseName("idx_product_types_is_active");
    }
}

