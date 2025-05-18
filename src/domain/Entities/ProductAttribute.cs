using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ProductAttribute
{
    public int ProductId { get; set; }
    public int AttributeId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual Attribute Attribute { get; set; } = null!;
}

public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.HasKey(pa => new { pa.ProductId, pa.AttributeId });

        builder.HasOne(pa => pa.Product)
            .WithMany(p => p.ProductAttributes)
            .HasForeignKey(pa => pa.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pa => pa.Attribute)
            .WithMany(a => a.ProductAttributes)
            .HasForeignKey(pa => pa.AttributeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}