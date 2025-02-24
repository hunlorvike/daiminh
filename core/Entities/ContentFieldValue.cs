using core.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ContentFieldValue : BaseEntity<int>
{
    public int ContentId { get; set; }
    public int FieldId { get; set; }
    public string Value { get; set; } = string.Empty;

    // Navigation properties
    public virtual Content? Content { get; set; }
    public virtual ContentFieldDefinition? Field { get; set; }
}

public class ContentFieldValueConfiguration : BaseEntityConfiguration<ContentFieldValue, int>
{
    public override void Configure(EntityTypeBuilder<ContentFieldValue> builder)
    {
        base.Configure(builder);

        builder.ToTable("content_field_values");

        builder.Property(e => e.ContentId).HasColumnName("content_id");
        builder.Property(e => e.FieldId).HasColumnName("field_id");
        builder.Property(e => e.Value).HasColumnName("value");

        builder.HasIndex(x => x.ContentId)
            .HasDatabaseName("idx_content_field_values_content_id");
        builder.HasIndex(x => x.FieldId)
            .HasDatabaseName("idx_content_field_values_field_id");

        builder.HasOne(x => x.Content)
            .WithMany(x => x.FieldValues)
            .HasForeignKey(x => x.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Field)
            .WithMany(x => x.FieldValues)
            .HasForeignKey(x => x.FieldId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}