using core.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ProductFieldDefinition : BaseEntity<int>
{
    public int ProductTypeId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public FieldType FieldType { get; set; } = FieldType.Text;
    public bool IsRequired { get; set; }
    public string FieldOptions { get; set; }

    // Navigation properties
    public virtual ProductType ProductType { get; set; } = new();
    public virtual ICollection<ProductFieldValue> FieldValues { get; set; } = [];
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
    }
}