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

        builder.Property(e => e.MetaTitle).HasMaxLength(100);
        builder.Property(e => e.MetaDescription).HasMaxLength(300);
        builder.Property(e => e.MetaKeywords).HasMaxLength(200);

        builder.Property(e => e.CanonicalUrl).HasMaxLength(255);
        builder.Property(e => e.NoIndex).HasDefaultValue(false);
        builder.Property(e => e.NoFollow).HasDefaultValue(false);

        builder.Property(e => e.OgTitle).HasMaxLength(100);
        builder.Property(e => e.OgDescription).HasMaxLength(300);
        builder.Property(e => e.OgImage).HasMaxLength(255);
        builder.Property(e => e.OgType).HasMaxLength(50).HasDefaultValue("website");

        builder.Property(e => e.TwitterTitle).HasMaxLength(100);
        builder.Property(e => e.TwitterDescription).HasMaxLength(300);
        builder.Property(e => e.TwitterImage).HasMaxLength(255);
        builder.Property(e => e.TwitterCard).HasMaxLength(50).HasDefaultValue("summary_large_image");

        builder.Property(e => e.SchemaMarkup).HasColumnType("TEXT");
        builder.Property(e => e.BreadcrumbJson).HasColumnType("TEXT");

        builder.Property(e => e.SitemapPriority).HasDefaultValue(0.5);
        builder.Property(e => e.SitemapChangeFrequency).HasMaxLength(20).HasDefaultValue("monthly");
    }
}