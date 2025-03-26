using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ArticleProduct : BaseEntity<int>
{
    public int ArticleId { get; set; }
    public int ProductId { get; set; }
    public int OrderIndex { get; set; } = 0;

    // Navigation properties
    public virtual Article? Article { get; set; }
    public virtual Product? Product { get; set; }
}

public class ArticleProductConfiguration : BaseEntityConfiguration<ArticleProduct, int>
{
    public override void Configure(EntityTypeBuilder<ArticleProduct> builder)
    {
        base.Configure(builder);

        builder.ToTable("article_products");

        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);

        builder.HasIndex(e => new { e.ArticleId, e.ProductId })
            .HasDatabaseName("idx_article_products_article_product")
            .IsUnique();
        builder.HasIndex(e => e.ArticleId).HasDatabaseName("idx_article_products_article_id");
        builder.HasIndex(e => e.ProductId).HasDatabaseName("idx_article_products_product_id");
        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_article_products_order_index");

        builder.HasOne(e => e.Article)
            .WithMany(a => a.ArticleProducts)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Product)
            .WithMany(p => p.ArticleProducts)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

