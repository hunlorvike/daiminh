using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ProductFieldValue : BaseEntity<int>
{
    public int ProductId { get; set; }
    public int FieldDefinitionId { get; set; }
    public string? Value { get; set; }

    // Navigation properties
    public virtual Product? Product { get; set; }
    public virtual ProductFieldDefinition? FieldDefinition { get; set; }
}

public class ProductFieldValueConfiguration : BaseEntityConfiguration<ProductFieldValue, int>
{
    public override void Configure(EntityTypeBuilder<ProductFieldValue> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_field_values");

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.FieldDefinitionId).HasColumnName("field_definition_id");
        builder.Property(e => e.Value).HasColumnName("value").HasColumnType("text");

        builder.HasIndex(e => new { e.ProductId, e.FieldDefinitionId })
            .HasDatabaseName("idx_product_field_values_product_field")
            .IsUnique();
        builder.HasIndex(e => e.ProductId).HasDatabaseName("idx_product_field_values_product_id");
        builder.HasIndex(e => e.FieldDefinitionId).HasDatabaseName("idx_product_field_values_field_definition_id");

        builder.HasOne(e => e.Product)
            .WithMany(p => p.FieldValues)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.FieldDefinition)
            .WithMany(fd => fd.FieldValues)
            .HasForeignKey(e => e.FieldDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

