using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ProductImage : BaseEntity<int>
{
    public int ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? AltText { get; set; }
    public string? Title { get; set; }
    public int OrderIndex { get; set; } = 0;
    public bool IsMain { get; set; } = false;

    // Navigation properties
    public virtual Product? Product { get; set; }
}

public class ProductImageConfiguration : BaseEntityConfiguration<ProductImage, int>
{
    public override void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_images");

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.ImageUrl).HasColumnName("image_url").IsRequired().HasMaxLength(255);
        builder.Property(e => e.ThumbnailUrl).HasColumnName("thumbnail_url").HasMaxLength(255);
        builder.Property(e => e.AltText).HasColumnName("alt_text").HasMaxLength(255);
        builder.Property(e => e.Title).HasColumnName("title").HasMaxLength(255);
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.Property(e => e.IsMain).HasColumnName("is_main").HasDefaultValue(false);

        builder.HasIndex(e => e.ProductId).HasDatabaseName("idx_product_images_product_id");
        builder.HasIndex(e => new { e.ProductId, e.IsMain }).HasDatabaseName("idx_product_images_product_main");
        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_product_images_order_index");

        builder.HasOne(e => e.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

