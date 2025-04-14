using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Gallery : SeoEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public int ViewCount { get; set; } = 0;
    public bool IsFeatured { get; set; } = false;
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    // New property for one-to-many relationship
    public int? CategoryId { get; set; }

    // Navigation properties
    public virtual Category? Category { get; set; }
    public virtual ICollection<GalleryImage>? Images { get; set; }
    public virtual ICollection<GalleryTag>? GalleryTags { get; set; }
}

public class GalleryConfiguration : SeoEntityConfiguration<Gallery, int>
{
    public override void Configure(EntityTypeBuilder<Gallery> builder)
    {
        base.Configure(builder);

        builder.ToTable("galleries");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
        builder.Property(e => e.CoverImage).HasColumnName("cover_image").HasMaxLength(255);
        builder.Property(e => e.ViewCount).HasColumnName("view_count").HasDefaultValue(0);
        builder.Property(e => e.IsFeatured).HasColumnName("is_featured").HasDefaultValue(false);
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<PublishStatus>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(PublishStatus.Draft);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_galleries_slug").IsUnique();
        builder.HasIndex(e => e.Status).HasDatabaseName("idx_galleries_status");
        builder.HasIndex(e => e.ViewCount).HasDatabaseName("idx_galleries_view_count");
        builder.HasIndex(e => e.IsFeatured).HasDatabaseName("idx_galleries_is_featured");

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Galleries)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

