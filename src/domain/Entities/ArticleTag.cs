using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ArticleTag : BaseEntity<int>
{
    public int ArticleId { get; set; }
    public int TagId { get; set; }

    // Navigation properties
    public virtual Article? Article { get; set; }
    public virtual Tag? Tag { get; set; }
}

public class ArticleTagConfiguration : BaseEntityConfiguration<ArticleTag, int>
{
    public override void Configure(EntityTypeBuilder<ArticleTag> builder)
    {
        base.Configure(builder);

        builder.ToTable("article_tags");

        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.TagId).HasColumnName("tag_id");

        builder.HasIndex(e => new { e.ArticleId, e.TagId })
            .HasDatabaseName("idx_article_tags_article_tag")
            .IsUnique();
        builder.HasIndex(e => e.ArticleId).HasDatabaseName("idx_article_tags_article_id");
        builder.HasIndex(e => e.TagId).HasDatabaseName("idx_article_tags_tag_id");

        builder.HasOne(e => e.Article)
            .WithMany(a => a.ArticleTags)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Tag)
            .WithMany(t => t.ArticleTags)
            .HasForeignKey(e => e.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

