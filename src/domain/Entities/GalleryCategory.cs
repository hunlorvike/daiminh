using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class GalleryCategory : BaseEntity<int>
{
    public int GalleryId { get; set; }
    public int CategoryId { get; set; }

    // Navigation properties
    public virtual Gallery? Gallery { get; set; }
    public virtual Category? Category { get; set; }
}

public class GalleryCategoryConfiguration : BaseEntityConfiguration<GalleryCategory, int>
{
    public override void Configure(EntityTypeBuilder<GalleryCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("gallery_categories");

        builder.Property(e => e.GalleryId).HasColumnName("gallery_id");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        builder.HasIndex(e => new { e.GalleryId, e.CategoryId })
            .HasDatabaseName("idx_gallery_categories_gallery_category")
            .IsUnique();
        builder.HasIndex(e => e.GalleryId).HasDatabaseName("idx_gallery_categories_gallery_id");
        builder.HasIndex(e => e.CategoryId).HasDatabaseName("idx_gallery_categories_category_id");

        builder.HasOne(e => e.Gallery)
            .WithMany(g => g.GalleryCategories)
            .HasForeignKey(e => e.GalleryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.GalleryCategories)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

