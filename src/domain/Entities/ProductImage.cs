using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ProductImage : BaseEntity<int>
{
    public int ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int OrderIndex { get; set; } = 0;
    public bool IsMain { get; set; } = false;
    public virtual Product? Product { get; set; }
}

public class ProductImageConfiguration : BaseEntityConfiguration<ProductImage, int>
{
    public override void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.ProductId);
        builder.Property(e => e.ImageUrl).IsRequired().HasMaxLength(255);
        builder.Property(e => e.OrderIndex).HasDefaultValue(0);
        builder.HasIndex(e => e.ProductId);
        builder.HasIndex(e => e.OrderIndex);
        builder.Property(e => e.IsMain).HasDefaultValue(false);
        builder.HasOne(e => e.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}