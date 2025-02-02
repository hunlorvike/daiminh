using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ProductImage : BaseEntity<int>
{
    public int ProductId { get; set; }
    public string ImageUrl { get; set; }
    public string AltText { get; set; }
    public bool IsPrimary { get; set; }
    public short DisplayOrder { get; set; }

    // Navigation properties
    public Product Product { get; set; }
}

public class ProductImageConfiguration : BaseEntityConfiguration<ProductImage, int>
{
    public override void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        base.Configure(builder);
        builder.ToTable("product_image");

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.ImageUrl).HasColumnName("image_url").IsRequired().HasMaxLength(255);
        builder.Property(e => e.AltText).HasColumnName("alt_text").HasMaxLength(255);
        builder.Property(e => e.IsPrimary).HasColumnName("is_primary").HasDefaultValue(false);
        builder.Property(e => e.DisplayOrder).HasColumnName("display_order");

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("idx_product_images_product_id");
    }
}