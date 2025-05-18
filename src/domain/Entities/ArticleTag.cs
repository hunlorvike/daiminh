using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ArticleTag
{
    public int ArticleId { get; set; }
    public int TagId { get; set; }
    public virtual Article? Article { get; set; }
    public virtual Tag? Tag { get; set; }
}

public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
{
    public void Configure(EntityTypeBuilder<ArticleTag> builder)
    {
        builder.HasKey(e => new { e.ArticleId, e.TagId });
        builder.Property(e => e.ArticleId);
        builder.Property(e => e.TagId);
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