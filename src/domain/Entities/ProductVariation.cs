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
        builder.Property(v => v.ProductId).IsRequired();
        builder.Property(v => v.Price).HasColumnType("decimal(18, 2)").IsRequired();
        builder.Property(v => v.SalePrice).HasColumnType("decimal(18, 2)");
        builder.Property(v => v.StockQuantity).IsRequired().HasDefaultValue(0);
        builder.Property(v => v.ImageUrl).HasMaxLength(255);
        builder.Property(v => v.IsDefault).IsRequired().HasDefaultValue(false);
        builder.Property(v => v.IsActive).IsRequired().HasDefaultValue(true);
        builder.HasIndex(v => v.ProductId);
        builder.HasIndex(v => v.IsDefault);
        builder.HasOne(v => v.Product)
               .WithMany(p => p.Variations)
               .HasForeignKey(v => v.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}