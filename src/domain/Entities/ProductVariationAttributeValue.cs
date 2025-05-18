using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ProductVariationAttributeValue
{
    public int ProductVariationId { get; set; }
    public int AttributeValueId { get; set; }
    public virtual ProductVariation ProductVariation { get; set; } = null!;
    public virtual AttributeValue AttributeValue { get; set; } = null!;
}

public class ProductVariationAttributeValueConfiguration : IEntityTypeConfiguration<ProductVariationAttributeValue>
{
    public void Configure(EntityTypeBuilder<ProductVariationAttributeValue> builder)
    {
        builder.HasKey(pvav => new { pvav.ProductVariationId, pvav.AttributeValueId });
        builder.HasOne(pvav => pvav.ProductVariation)
            .WithMany(pv => pv.ProductVariationAttributeValues)
            .HasForeignKey(pvav => pvav.ProductVariationId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(pvav => pvav.AttributeValue)
            .WithMany(av => av.ProductVariationAttributeValues)
            .HasForeignKey(pvav => pvav.AttributeValueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}