using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ProductVariant : BaseEntity<int>
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; } = 0;
    public string? Color { get; set; }
    public string? Size { get; set; }
    public string? Packaging { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual Product? Product { get; set; }
}

public class ProductVariantConfiguration : BaseEntityConfiguration<ProductVariant, int>
{
    public override void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_variants");

        builder.Property(v => v.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(v => v.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(v => v.Sku).HasColumnName("sku").IsRequired().HasMaxLength(100);
        builder.Property(v => v.Price).HasColumnName("price").HasColumnType("decimal").HasPrecision(18, 2).IsRequired();
        builder.Property(v => v.StockQuantity).HasColumnName("stock_quantity").HasDefaultValue(0);
        builder.Property(v => v.Color).HasColumnName("color").HasMaxLength(50);
        builder.Property(v => v.Size).HasColumnName("size").HasMaxLength(50);
        builder.Property(v => v.Packaging).HasColumnName("packaging").HasMaxLength(50);
        builder.Property(v => v.ImageUrl).HasColumnName("image_url").HasMaxLength(2048);
        builder.Property(v => v.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        builder.HasIndex(v => v.Sku).HasDatabaseName("idx_product_variants_sku").IsUnique();
        builder.HasIndex(v => v.ProductId).HasDatabaseName("idx_product_variants_product_id");
        builder.HasIndex(v => v.IsActive).HasDatabaseName("idx_product_variants_is_active");

        builder.HasOne(v => v.Product)
               .WithMany(p => p.Variants)
               .HasForeignKey(v => v.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}