using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ContentFieldValue : BaseEntity<int>
{
    public int ContentItemId { get; set; }
    public int FieldId { get; set; }
    public string Value { get; set; }

    // Navigation properties
    public Content ContentItem { get; set; }
    public ContentFieldDefinition Field { get; set; }
}

public class ContentFieldValueConfiguration : BaseEntityConfiguration<ContentFieldValue, int>
{
    public override void Configure(EntityTypeBuilder<ContentFieldValue> builder)
    {
        base.Configure(builder);

        builder.ToTable("content_field_values");

        builder.Property(e => e.ContentItemId).HasColumnName("content_item_id");
        builder.Property(e => e.FieldId).HasColumnName("field_id");
        builder.Property(e => e.Value).HasColumnName("value");

        builder.HasOne(x => x.ContentItem)
            .WithMany(x => x.FieldValues)
            .HasForeignKey(x => x.ContentItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Field)
            .WithMany(x => x.FieldValues)
            .HasForeignKey(x => x.FieldId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ContentItemId)
            .HasDatabaseName("idx_content_field_values_content_item_id");
        builder.HasIndex(x => x.FieldId)
            .HasDatabaseName("idx_content_field_values_field_id");
    }
}