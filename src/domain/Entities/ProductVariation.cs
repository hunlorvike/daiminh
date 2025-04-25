using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ProductVariation : BaseEntity<int>
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual Product Product { get; set; } = null!;
    public virtual ICollection<ProductVariationAttributeValue>? ProductVariationAttributeValues { get; set; }
}

public class ProductVariationConfiguration : BaseEntityConfiguration<ProductVariation, int>
{
    public override void Configure(EntityTypeBuilder<ProductVariation> builder)
    {
        base.Configure(builder);
        builder.ToTable("product_variations");

        builder.Property(v => v.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(v => v.Price).HasColumnName("price").HasColumnType("decimal(18, 2)").IsRequired();
        builder.Property(v => v.SalePrice).HasColumnName("sale_price").HasColumnType("decimal(18, 2)");
        builder.Property(v => v.StockQuantity).HasColumnName("stock_quantity").IsRequired().HasDefaultValue(0);
        builder.Property(v => v.ImageUrl).HasColumnName("image_url").HasMaxLength(255);
        builder.Property(v => v.IsDefault).HasColumnName("is_default").IsRequired().HasDefaultValue(false);
        builder.Property(v => v.IsActive).HasColumnName("is_active").IsRequired().HasDefaultValue(true);

        builder.HasOne(v => v.Product)
               .WithMany(p => p.Variations)
               .HasForeignKey(v => v.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
