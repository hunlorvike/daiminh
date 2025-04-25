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
        builder.ToTable("product_variation_attribute_values");
        builder.HasKey(pvav => new { pvav.ProductVariationId, pvav.AttributeValueId });
        builder.Property(pvav => pvav.ProductVariationId).HasColumnName("product_variation_id");
        builder.Property(pvav => pvav.AttributeValueId).HasColumnName("attribute_value_id");
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