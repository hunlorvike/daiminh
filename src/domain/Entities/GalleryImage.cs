using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class GalleryImage : BaseEntity<int>
{
    public int GalleryId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? AltText { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int OrderIndex { get; set; } = 0;
    
    // Navigation properties
    public virtual Gallery? Gallery { get; set; }
}

public class GalleryImageConfiguration : BaseEntityConfiguration<GalleryImage, int>
{
    public override void Configure(EntityTypeBuilder<GalleryImage> builder)
    {
        base.Configure(builder);

        builder.ToTable("gallery_images");

        builder.Property(e => e.GalleryId).HasColumnName("gallery_id");
        builder.Property(e => e.ImageUrl).HasColumnName("image_url").IsRequired().HasMaxLength(255);
        builder.Property(e => e.ThumbnailUrl).HasColumnName("thumbnail_url").HasMaxLength(255);
        builder.Property(e => e.AltText).HasColumnName("alt_text").HasMaxLength(255);
        builder.Property(e => e.Title).HasColumnName("title").HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);

        builder.HasIndex(e => e.GalleryId).HasDatabaseName("idx_gallery_images_gallery_id");
        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_gallery_images_order_index");

        builder.HasOne(e => e.Gallery)
            .WithMany(g => g.Images)
            .HasForeignKey(e => e.GalleryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

