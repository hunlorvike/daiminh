using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ProductCategory : BaseEntity<int>
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }

    // Navigation properties
    public virtual Product? Product { get; set; }
    public virtual Category? Category { get; set; }
}

public class ProductCategoryConfiguration : BaseEntityConfiguration<ProductCategory, int>
{
    public override void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_categories");

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        builder.HasIndex(e => new { e.ProductId, e.CategoryId })
            .HasDatabaseName("idx_product_categories_product_category")
            .IsUnique();
        builder.HasIndex(e => e.ProductId).HasDatabaseName("idx_product_categories_product_id");
        builder.HasIndex(e => e.CategoryId).HasDatabaseName("idx_product_categories_category_id");

        builder.HasOne(e => e.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

