using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Article : SeoEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? FeaturedImage { get; set; }
    public string? ThumbnailImage { get; set; }
    public int ViewCount { get; set; } = 0;
    public bool IsFeatured { get; set; } = false;
    public DateTime? PublishedAt { get; set; }
    public string? AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorAvatar { get; set; }
    public int EstimatedReadingMinutes { get; set; } = 0;
    public ArticleType Type { get; set; } = ArticleType.Knowledge;
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    // Navigation properties
    public virtual ICollection<ArticleCategory>? ArticleCategories { get; set; }
    public virtual ICollection<ArticleTag>? ArticleTags { get; set; }
    public virtual ICollection<ArticleProduct>? ArticleProducts { get; set; }
    public virtual ICollection<Comment>? Comments { get; set; }
}

public class ArticleConfiguration : SeoEntityConfiguration<Article, int>
{
    public override void Configure(EntityTypeBuilder<Article> builder)
    {
        base.Configure(builder);

        builder.ToTable("articles");

        builder.Property(e => e.Title).HasColumnName("title").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Content).HasColumnName("content").HasColumnType("text").IsRequired();
        builder.Property(e => e.Summary).HasColumnName("summary").HasMaxLength(500);
        builder.Property(e => e.FeaturedImage).HasColumnName("featured_image").HasMaxLength(255);
        builder.Property(e => e.ThumbnailImage).HasColumnName("thumbnail_image").HasMaxLength(255);
        builder.Property(e => e.ViewCount).HasColumnName("view_count").HasDefaultValue(0);
        builder.Property(e => e.IsFeatured).HasColumnName("is_featured").HasDefaultValue(false);
        builder.Property(e => e.PublishedAt).HasColumnName("published_at");
        builder.Property(e => e.AuthorId).HasColumnName("author_id").HasMaxLength(50);
        builder.Property(e => e.AuthorName).HasColumnName("author_name").HasMaxLength(100);
        builder.Property(e => e.AuthorAvatar).HasColumnName("author_avatar").HasMaxLength(255);
        builder.Property(e => e.EstimatedReadingMinutes).HasColumnName("estimated_reading_minutes").HasDefaultValue(0);
        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<ArticleType>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(ArticleType.Knowledge);
        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<PublishStatus>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(PublishStatus.Draft);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_articles_slug").IsUnique();
        builder.HasIndex(e => e.Status).HasDatabaseName("idx_articles_status");
        builder.HasIndex(e => e.Type).HasDatabaseName("idx_articles_type");
        builder.HasIndex(e => e.ViewCount).HasDatabaseName("idx_articles_view_count");
        builder.HasIndex(e => e.IsFeatured).HasDatabaseName("idx_articles_is_featured");
        builder.HasIndex(e => e.PublishedAt).HasDatabaseName("idx_articles_published_at");
        builder.HasIndex(e => e.AuthorId).HasDatabaseName("idx_articles_author_id");
    }
}

