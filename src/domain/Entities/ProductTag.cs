using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ProductTag
{
    public int ProductId { get; set; }
    public int TagId { get; set; }
    public virtual Product? Product { get; set; }
    public virtual Tag? Tag { get; set; }
}

public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.HasKey(e => new { e.ProductId, e.TagId });
        builder.HasOne(e => e.Product)
            .WithMany(p => p.ProductTags)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Tag)
            .WithMany(t => t.ProductTags)
            .HasForeignKey(e => e.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}