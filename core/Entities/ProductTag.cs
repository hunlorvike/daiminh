using core.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ProductTag : BaseEntity
{
    public int ProductId { get; set; }
    public int TagId { get; set; }

    // Navigation properties
    public virtual Product? Product { get; set; }
    public virtual Tag? Tag { get; set; }
}

public class ProductTagConfiguration : BaseEntityConfiguration<ProductTag>
{
    public override void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_tags");

        builder.HasKey(x => new { x.ProductId, x.TagId });

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.TagId).HasColumnName("tag_id");

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("idx_product_tags_product_id");
        builder.HasIndex(x => x.TagId)
            .HasDatabaseName("idx_product_tags_tag_id");

        builder.HasOne(x => x.Product)
            .WithMany(x => x.ProductTags)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Tag)
            .WithMany(x => x.ProductTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}