using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ProductFieldValue : BaseEntity<int>
{
    public int ProductId { get; set; }
    public int FieldId { get; set; }
    public string Value { get; set; }

    // Navigation properties
    public virtual Product? Product { get; set; }
    public virtual ProductFieldDefinition? Field { get; set; }
}

public class ProductFieldValueConfiguration : BaseEntityConfiguration<ProductFieldValue, int>
{
    public override void Configure(EntityTypeBuilder<ProductFieldValue> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_field_values");

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.FieldId).HasColumnName("field_id");
        builder.Property(e => e.Value).HasColumnName("value");

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("idx_product_field_values_product_id");
        builder.HasIndex(x => x.FieldId)
            .HasDatabaseName("idx_product_field_values_field_id");

        builder.HasOne(x => x.Product)
            .WithMany(x => x.FieldValues)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Field)
            .WithMany(x => x.FieldValues)
            .HasForeignKey(x => x.FieldId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}