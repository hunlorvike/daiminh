using domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
using shared.Models;
using System.Text.Json;

namespace domain.Entities;

public class ContentFieldDefinition : BaseEntity<int>
{
    public int ContentTypeId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public FieldType FieldType { get; set; } = FieldType.Text;
    public bool IsRequired { get; set; }
    public string? FieldOptions { get; set; }

    // Navigation properties
    public virtual ContentType? ContentType { get; set; }
    public virtual ICollection<ContentFieldValue>? FieldValues { get; set; }
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

        builder.HasData(new ContentFieldDefinitionSeeder().DataSeeder());
    }
}

public class ContentFieldDefinitionSeeder : ISeeder<ContentFieldDefinition>
{
    public IEnumerable<ContentFieldDefinition> DataSeeder()
    {
        return
        [
            // Các trường cho ContentType "Dịch vụ" (Id = 3)
            new ContentFieldDefinition { Id = 1, ContentTypeId = 3, FieldName = "Mô tả ngắn", FieldType = FieldType.Text, IsRequired = true },
            new ContentFieldDefinition { Id = 2, ContentTypeId = 3, FieldName = "Quy trình chi tiết", FieldType = FieldType.Text, IsRequired = false },
            new ContentFieldDefinition { Id = 3, ContentTypeId = 3, FieldName = "Bảng giá tham khảo", FieldType = FieldType.Text, IsRequired = false },

            // Các trường cho ContentType "Tư vấn" (Id = 4)
            new ContentFieldDefinition { Id = 4, ContentTypeId = 4, FieldName = "Mô tả ngắn", FieldType = FieldType.Text, IsRequired = true },
            new ContentFieldDefinition { Id = 5, ContentTypeId = 4, FieldName = "Nội dung chi tiết", FieldType = FieldType.Text, IsRequired = false },
            new ContentFieldDefinition { Id = 6, ContentTypeId = 4, FieldName = "Hình thức tư vấn", FieldType = FieldType.Select, IsRequired = true,
                FieldOptions = JsonSerializer.Serialize(new List<object> {
                    new { value = "online", label = "Trực tuyến" },
                    new { value = "offline", label = "Trực tiếp" }
                })
            },
            new ContentFieldDefinition { Id = 7, ContentTypeId = 4, FieldName = "Thời lượng tư vấn (phút)", FieldType = FieldType.Number, IsRequired = true },
            new ContentFieldDefinition { Id = 8, ContentTypeId = 4, FieldName = "Chi phí", FieldType = FieldType.Number, IsRequired = false },
            new ContentFieldDefinition { Id = 9, ContentTypeId = 4, FieldName = "Ảnh minh họa", FieldType = FieldType.Text, IsRequired = false }
        ];
    }
}