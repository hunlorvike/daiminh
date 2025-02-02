using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ContentTag : BaseEntity
{
    public int ContentItemId { get; set; }
    public int TagId { get; set; }

    // Navigation properties
    public Content ContentItem { get; set; }
    public Tag Tag { get; set; }
}

public class ContentTagConfiguration : BaseEntityConfiguration<ContentTag>
{
    public override void Configure(EntityTypeBuilder<ContentTag> builder)
    {
        base.Configure(builder);

        builder.ToTable("content_tags");

        builder.HasKey(x => new { x.ContentItemId, x.TagId });

        builder.Property(e => e.ContentItemId).HasColumnName("content_item_id");
        builder.Property(e => e.TagId).HasColumnName("tag_id");

        builder.HasOne(x => x.ContentItem)
            .WithMany(x => x.ContentTags)
            .HasForeignKey(x => x.ContentItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Tag)
            .WithMany()
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ContentItemId)
            .HasDatabaseName("idx_content_tags_content_item_id");
        builder.HasIndex(x => x.TagId)
            .HasDatabaseName("idx_content_tags_tag_id");
    }
}