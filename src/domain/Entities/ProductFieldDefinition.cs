using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
using shared.Models;

namespace domain.Entities;

public class ProductFieldDefinition : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? Description { get; set; }
    public FieldType Type { get; set; } = FieldType.Text;
    public string? DefaultValue { get; set; }
    public string? Options { get; set; } // JSON for select, multiselect, etc.
    public bool IsRequired { get; set; } = false;
    public int OrderIndex { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public bool IsFilterable { get; set; } = false;
    public bool IsVisibleInList { get; set; } = false;
    public int ProductTypeId { get; set; }

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

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Key).HasColumnName("key").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<FieldType>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(FieldType.Text);
        builder.Property(e => e.DefaultValue).HasColumnName("default_value").HasMaxLength(255);
        builder.Property(e => e.Options).HasColumnName("options").HasColumnType("json");
        builder.Property(e => e.IsRequired).HasColumnName("is_required").HasDefaultValue(false);
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.IsFilterable).HasColumnName("is_filterable").HasDefaultValue(false);
        builder.Property(e => e.IsVisibleInList).HasColumnName("is_visible_in_list").HasDefaultValue(false);
        builder.Property(e => e.ProductTypeId).HasColumnName("product_type_id");

        builder.HasIndex(e => new { e.ProductTypeId, e.Key })
            .HasDatabaseName("idx_product_field_definitions_type_key")
            .IsUnique();
        builder.HasIndex(e => e.ProductTypeId).HasDatabaseName("idx_product_field_definitions_product_type_id");
        builder.HasIndex(e => e.IsActive).HasDatabaseName("idx_product_field_definitions_is_active");
        builder.HasIndex(e => e.IsFilterable).HasDatabaseName("idx_product_field_definitions_is_filterable");
        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_product_field_definitions_order_index");

        builder.HasOne(e => e.ProductType)
            .WithMany(pt => pt.FieldDefinitions)
            .HasForeignKey(e => e.ProductTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

