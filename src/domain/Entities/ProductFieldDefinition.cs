using domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
using shared.Models;
using System.Text.Json;

namespace domain.Entities;

public class ProductFieldDefinition : BaseEntity<int>
{
    public int ProductTypeId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public FieldType FieldType { get; set; } = FieldType.Text;
    public bool IsRequired { get; set; }
    public string? FieldOptions { get; set; }

    // Navigation properties
    public virtual ProductType? ProductType { get; set; }
    public virtual ICollection<ProductFieldValue>? FieldValues { get; set; }
}

public class ProductFieldDefinitionConfiguration : BaseEntityConfiguration<ProductFieldDefinition, int>
{
    public override void Configure(EntityTypeBuilder<ProductFieldDefinition> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_field_definitions");

        builder.Property(e => e.ProductTypeId).HasColumnName("product_type_id");
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

        builder.HasIndex(x => x.ProductTypeId)
            .HasDatabaseName("idx_product_field_definitions_product_type_id");

        builder.HasOne(x => x.ProductType)
            .WithMany(x => x.FieldDefinitions)
            .HasForeignKey(x => x.ProductTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(new ProductFieldDefinitionSeeder().DataSeeder());
    }
}

public class ProductFieldDefinitionSeeder : ISeeder<ProductFieldDefinition>
{
    public IEnumerable<ProductFieldDefinition> DataSeeder()
    {
        return
        [
            // Thuộc tính cho ProductType "Sơn Nước" (Id = 1)
            new ProductFieldDefinition { Id = 1, ProductTypeId = 1, FieldName = "Độ bóng", FieldType = FieldType.Select, IsRequired = true,
                FieldOptions = JsonSerializer.Serialize(new List<object> {
                    new { value = "bong-mo", label = "Bóng mờ" },
                    new { value = "bong-nhe", label = "Bóng nhẹ" },
                    new { value = "bong-cao", label = "Bóng cao" },
                    new { value = "sieu-bong", label = "Siêu bóng" }
                })
            },
            new ProductFieldDefinition { Id = 2, ProductTypeId = 1, FieldName = "Dung tích", FieldType = FieldType.Select, IsRequired = true,
                FieldOptions = JsonSerializer.Serialize(new List<object> {
                    new { value = "1-lit", label = "1 Lít" },
                    new { value = "5-lit", label = "5 Lít" },
                    new { value = "18-lit", label = "18 Lít" }
                })
            },
            new ProductFieldDefinition { Id = 3, ProductTypeId = 1, FieldName = "Màu sắc", FieldType = FieldType.Text, IsRequired = true },

            // Thuộc tính cho ProductType "Sơn Dầu" (Id = 2)
            new ProductFieldDefinition { Id = 4 ,ProductTypeId = 2, FieldName = "Độ bóng", FieldType = FieldType.Select, IsRequired = true,
                FieldOptions = JsonSerializer.Serialize(new List<object> {
                    new { value = "bong", label = "Bóng" },
                    new { value = "mo", label = "Mờ" }
                })
            },
            new ProductFieldDefinition { Id = 5, ProductTypeId = 2, FieldName = "Dung tích", FieldType = FieldType.Select, IsRequired = true,
                FieldOptions = JsonSerializer.Serialize(new List<object> {
                    new { value = "0-5-lit", label = "0.5 Lít" },
                    new { value = "1-lit", label = "1 Lít" },
                    new { value = "4-lit", label = "4 Lít" }
                })
            },
            new ProductFieldDefinition { Id = 6, ProductTypeId = 2, FieldName = "Loại bề mặt", FieldType = FieldType.Select, IsRequired = true,
                FieldOptions = JsonSerializer.Serialize(new List<object> {
                    new { value = "go", label = "Gỗ" },
                    new { value = "kim-loai", label = "Kim loại" }
                })
            },
            new ProductFieldDefinition { Id = 7, ProductTypeId = 2, FieldName = "Màu sắc", FieldType = FieldType.Text, IsRequired = true },

            // Thuộc tính cho ProductType "Sơn Chống Thấm" (Id = 7)
            new ProductFieldDefinition { Id = 8, ProductTypeId = 7, FieldName = "Loại chống thấm", FieldType = FieldType.Select, IsRequired = true,
                FieldOptions = JsonSerializer.Serialize(new List<object> {
                    new { value = "tuong-dung", label = "Tường đứng" },
                    new { value = "san-mai", label = "Sàn mái" },
                    new { value = "nha-ve-sinh", label = "Nhà vệ sinh" }
                })
            },
            new ProductFieldDefinition { Id = 9,ProductTypeId = 7, FieldName = "Dung tích", FieldType = FieldType.Select, IsRequired = true,
                FieldOptions = JsonSerializer.Serialize(new List<object> {
                    new { value = "1-kg", label = "1 Kg" },
                    new { value = "5-kg", label = "5 Kg" },
                    new { value = "20-kg", label = "20 Kg" }
                })
            },
            new ProductFieldDefinition { Id = 10, ProductTypeId = 7, FieldName = "Màu sắc", FieldType = FieldType.Text, IsRequired = false }
        ];
    }
}