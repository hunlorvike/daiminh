using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class GalleryTag : BaseEntity<int>
{
    public int GalleryId { get; set; }
    public int TagId { get; set; }

    // Navigation properties
    public virtual Gallery? Gallery { get; set; }
    public virtual Tag? Tag { get; set; }
}

public class GalleryTagConfiguration : BaseEntityConfiguration<GalleryTag, int>
{
    public override void Configure(EntityTypeBuilder<GalleryTag> builder)
    {
        base.Configure(builder);

        builder.ToTable("gallery_tags");

        builder.Property(e => e.GalleryId).HasColumnName("gallery_id");
        builder.Property(e => e.TagId).HasColumnName("tag_id");

        builder.HasIndex(e => new { e.GalleryId, e.TagId })
            .HasDatabaseName("idx_gallery_tags_gallery_tag")
            .IsUnique();
        builder.HasIndex(e => e.GalleryId).HasDatabaseName("idx_gallery_tags_gallery_id");
        builder.HasIndex(e => e.TagId).HasDatabaseName("idx_gallery_tags_tag_id");

        builder.HasOne(e => e.Gallery)
            .WithMany(g => g.GalleryTags)
            .HasForeignKey(e => e.GalleryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Tag)
            .WithMany(t => t.GalleryTags)
            .HasForeignKey(e => e.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

