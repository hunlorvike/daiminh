using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ProductCategory : BaseEntity
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }

    // Navigation properties
    public Product Product { get; set; }
    public Category Category { get; set; }
}

public class ProductCategoryConfiguration : BaseEntityConfiguration<ProductCategory>
{
    public override void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_categories");

        builder.HasKey(x => new { x.ProductId, x.CategoryId });

        builder.HasOne(x => x.Product)
            .WithMany(x => x.ProductCategories)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("idx_product_categories_product_id");
        builder.HasIndex(x => x.CategoryId)
            .HasDatabaseName("idx_product_categories_category_id");
    }
}