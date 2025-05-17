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
    public PublishStatus Status { get; set; } = PublishStatus.Draft;
    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<ArticleTag>? ArticleTags { get; set; }
    public virtual ICollection<ArticleProduct>? ArticleProducts { get; set; }
}

public class ArticleConfiguration : SeoEntityConfiguration<Article, int>
{
    public override void Configure(EntityTypeBuilder<Article> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Title).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).IsRequired().HasMaxLength(255);
        builder.HasIndex(e => e.Slug).IsUnique();

        builder.Property(e => e.Content).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(e => e.Summary).HasMaxLength(500);
        builder.Property(e => e.FeaturedImage).HasMaxLength(255);
        builder.Property(e => e.ThumbnailImage).HasMaxLength(255);
        builder.Property(e => e.ViewCount).HasDefaultValue(0);
        builder.Property(e => e.IsFeatured).HasDefaultValue(false);
        builder.Property(e => e.PublishedAt);
        builder.HasIndex(e => e.PublishedAt);

        builder.Property(e => e.AuthorId).HasMaxLength(50);
        builder.Property(e => e.AuthorName).HasMaxLength(100);
        builder.Property(e => e.AuthorAvatar).HasMaxLength(255);
        builder.Property(e => e.EstimatedReadingMinutes).HasDefaultValue(0);
        builder.Property(e => e.CategoryId);
        builder.Property(e => e.Status)
            .IsRequired()
            .HasDefaultValue(PublishStatus.Draft);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Articles)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}