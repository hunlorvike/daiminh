using System.Text.Json;
using core.Common.Enums;
using core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ContentFieldDefinition : BaseEntity<int>
{
    public int ContentTypeId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public FieldType FieldType { get; set; } = FieldType.Text;
    public bool IsRequired { get; set; }
    public string? FieldOptions { get; set; }

    // Navigation properties
    public virtual ContentType ContentType { get; set; } = new();
    public virtual ICollection<ContentFieldValue> FieldValues { get; set; } = new List<ContentFieldValue>();
}

public class ContentFieldDefinitionConfiguration : BaseEntityConfiguration<ContentFieldDefinition, int>
{
    public override void Configure(EntityTypeBuilder<ContentFieldDefinition> builder)
    {
        base.Configure(builder);

        builder.ToTable("content_field_definitions");

        builder.Property(e => e.ContentTypeId).HasColumnName("content_type_id");
        builder.Property(e => e.FieldName).HasColumnName("field_name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.FieldType)
            .HasColumnName("field_type")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<FieldType>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(FieldType.Text);

        builder.Property(e => e.IsRequired).HasColumnName("is_required").HasDefaultValue(false);
        builder.Property(e => e.FieldOptions).HasColumnName("field_options");

        builder.HasIndex(x => x.ContentTypeId)
            .HasDatabaseName("idx_content_field_definitions_content_type_id");

        builder.HasOne(x => x.ContentType)
            .WithMany(x => x.FieldDefinitions)
            .HasForeignKey(x => x.ContentTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}