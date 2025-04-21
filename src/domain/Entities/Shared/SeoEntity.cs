using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities.Shared;

public abstract class SeoEntity<TKey> : BaseEntity<TKey> where TKey : IEquatable<TKey>
{
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public bool NoIndex { get; set; } = false;
    public bool NoFollow { get; set; } = false;
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string? OgType { get; set; } = "website";
    public string? TwitterTitle { get; set; }
    public string? TwitterDescription { get; set; }
    public string? TwitterImage { get; set; }
    public string? TwitterCard { get; set; } = "summary_large_image";
    public string? SchemaMarkup { get; set; }
    public string? BreadcrumbJson { get; set; }
    public double? SitemapPriority { get; set; } = 0.5;
    public string? SitemapChangeFrequency { get; set; } = "monthly"; // always, hourly, daily, weekly, monthly, yearly, never
}

public abstract class SeoEntityConfiguration<TEntity, TKey> : BaseEntityConfiguration<TEntity, TKey>
    where TEntity : SeoEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.MetaTitle).HasColumnName("meta_title").HasMaxLength(100);
        builder.Property(e => e.MetaDescription).HasColumnName("meta_description").HasMaxLength(300);
        builder.Property(e => e.MetaKeywords).HasColumnName("meta_keywords").HasMaxLength(200);

        builder.Property(e => e.CanonicalUrl).HasColumnName("canonical_url").HasMaxLength(255);
        builder.Property(e => e.NoIndex).HasColumnName("no_index").HasDefaultValue(false);
        builder.Property(e => e.NoFollow).HasColumnName("no_follow").HasDefaultValue(false);

        builder.Property(e => e.OgTitle).HasColumnName("og_title").HasMaxLength(100);
        builder.Property(e => e.OgDescription).HasColumnName("og_description").HasMaxLength(300);
        builder.Property(e => e.OgImage).HasColumnName("og_image").HasMaxLength(255);
        builder.Property(e => e.OgType).HasColumnName("og_type").HasMaxLength(50).HasDefaultValue("website");

        builder.Property(e => e.TwitterTitle).HasColumnName("twitter_title").HasMaxLength(100);
        builder.Property(e => e.TwitterDescription).HasColumnName("twitter_description").HasMaxLength(300);
        builder.Property(e => e.TwitterImage).HasColumnName("twitter_image").HasMaxLength(255);
        builder.Property(e => e.TwitterCard).HasColumnName("twitter_card").HasMaxLength(50).HasDefaultValue("summary_large_image");

        builder.Property(e => e.SchemaMarkup).HasColumnName("schema_markup").HasColumnType("nvarchar(max)");
        builder.Property(e => e.BreadcrumbJson).HasColumnName("breadcrumb_json").HasColumnType("nvarchar(max)");

        builder.Property(e => e.SitemapPriority).HasColumnName("sitemap_priority").HasColumnType("float").HasDefaultValue(0.5);
        builder.Property(e => e.SitemapChangeFrequency).HasColumnName("sitemap_change_frequency").HasMaxLength(20).HasDefaultValue("monthly");
    }
}

