using System.Text.Json;
using core.Common.Enums;
using core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class Content : SeoEntity<int>
{
    public int ContentTypeId { get; set; }
    public int? AuthorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public PublishStatus Status { get; set; }

    // Navigation properties
    public ContentType ContentType { get; set; }
    public User Author { get; set; }
    public ICollection<ContentFieldValue> FieldValues { get; set; }
    public ICollection<ContentCategory> ContentCategories { get; set; }
    public ICollection<ContentTag> ContentTags { get; set; }
    public ICollection<ContentComment> Comments { get; set; }
}

public class ContentConfiguration : SeoEntityConfiguration<Content, int>
{
    public override void Configure(EntityTypeBuilder<Content> builder)
    {
        base.Configure(builder);

        builder.ToTable("contents");

        builder.Property(e => e.ContentTypeId).HasColumnName("content_type_id");
        builder.Property(e => e.AuthorId).HasColumnName("author_id");
        builder.Property(e => e.Title).HasColumnName("title").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Status).HasColumnName("status")
            .HasConversion(v => v.ToStringValue(), v => v.ToPublishStatusEnum()).IsRequired().HasMaxLength(20)
            .HasDefaultValue(PublishStatus.Draft);

        builder.HasIndex(x => x.Slug).HasDatabaseName("idx_contents_slug");
        builder.HasIndex(x => x.ContentTypeId).HasDatabaseName("idx_contents_content_type_id");
        builder.HasIndex(x => x.AuthorId).HasDatabaseName("idx_contents_author_id");

        builder.HasOne(x => x.ContentType)
            .WithMany(x => x.Contents)
            .HasForeignKey(x => x.ContentTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}