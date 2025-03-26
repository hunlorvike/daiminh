using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ArticleCategory : BaseEntity<int>
{
    public int ArticleId { get; set; }
    public int CategoryId { get; set; }
    
    // Navigation properties
    public virtual Article? Article { get; set; }
    public virtual Category? Category { get; set; }
}

public class ArticleCategoryConfiguration : BaseEntityConfiguration<ArticleCategory, int>
{
    public override void Configure(EntityTypeBuilder<ArticleCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("article_categories");

        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        builder.HasIndex(e => new { e.ArticleId, e.CategoryId })
            .HasDatabaseName("idx_article_categories_article_category")
            .IsUnique();
        builder.HasIndex(e => e.ArticleId).HasDatabaseName("idx_article_categories_article_id");
        builder.HasIndex(e => e.CategoryId).HasDatabaseName("idx_article_categories_category_id");

        builder.HasOne(e => e.Article)
            .WithMany(a => a.ArticleCategories)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.ArticleCategories)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

