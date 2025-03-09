using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ProductType : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<ProductFieldDefinition>? FieldDefinitions { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
}

public class ProductTypeConfiguration : BaseEntityConfiguration<ProductType, int>
{
    public override void Configure(EntityTypeBuilder<ProductType> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_types");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(50);

        builder.HasIndex(e => e.Name).HasDatabaseName("idx_product_types_name").IsUnique();
        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_product_types_slug").IsUnique();
    }
}