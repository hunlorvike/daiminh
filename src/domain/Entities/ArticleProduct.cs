using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ArticleProduct
{
    public int ArticleId { get; set; }
    public int ProductId { get; set; }
    public virtual Article? Article { get; set; }
    public virtual Product? Product { get; set; }
}

public class ArticleProductConfiguration : IEntityTypeConfiguration<ArticleProduct>
{
    public void Configure(EntityTypeBuilder<ArticleProduct> builder)
    {
        builder.ToTable("article_products");
        builder.HasKey(e => new { e.ArticleId, e.ProductId });
        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.ProductId).HasColumnName("product_id");
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
