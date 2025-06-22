using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ArticleProduct
{
    public int ArticleId { get; set; }
    public virtual Article Article { get; set; } = null!;

    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
}

public class ArticleProductConfiguration : IEntityTypeConfiguration<ArticleProduct>
{
    public void Configure(EntityTypeBuilder<ArticleProduct> builder)
    {
        builder.HasKey(ap => new { ap.ArticleId, ap.ProductId });

        builder.HasOne(ap => ap.Article)
            .WithMany(a => a.ArticleProducts)
            .HasForeignKey(ap => ap.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ap => ap.Product)
            .WithMany(p => p.ArticleProducts)
            .HasForeignKey(ap => ap.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}