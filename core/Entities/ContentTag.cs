using core.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ContentTag : BaseEntity
{
    public int ContentId { get; set; }
    public int TagId { get; set; }

    // Navigation properties
    public virtual Content? Content { get; set; }
    public virtual Tag? Tag { get; set; }
}

public class ContentTagConfiguration : BaseEntityConfiguration<ContentTag>
{
    public override void Configure(EntityTypeBuilder<ContentTag> builder)
    {
        base.Configure(builder);

        builder.ToTable("content_tags");

        builder.HasKey(x => new { x.ContentId, x.TagId });

        builder.Property(e => e.ContentId).HasColumnName("content_id");
        builder.Property(e => e.TagId).HasColumnName("tag_id");

        builder.HasIndex(x => x.ContentId)
            .HasDatabaseName("idx_content_tags_content_id");
        builder.HasIndex(x => x.TagId)
            .HasDatabaseName("idx_content_tags_tag_id");

        builder.HasOne(x => x.Content)
            .WithMany(x => x.ContentTags)
            .HasForeignKey(x => x.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Tag)
            .WithMany(x => x.ContentTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}