using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ProductTag : BaseEntity<int>
{
    public int ProductId { get; set; }
    public int TagId { get; set; }
    
    // Navigation properties
    public virtual Product? Product { get; set; }
    public virtual Tag? Tag { get; set; }
}

public class ProductTagConfiguration : BaseEntityConfiguration<ProductTag, int>
{
    public override void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_tags");

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.TagId).HasColumnName("tag_id");

        builder.HasIndex(e => new { e.ProductId, e.TagId })
            .HasDatabaseName("idx_product_tags_product_tag")
            .IsUnique();
        builder.HasIndex(e => e.ProductId).HasDatabaseName("idx_product_tags_product_id");
        builder.HasIndex(e => e.TagId).HasDatabaseName("idx_product_tags_tag_id");

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

