using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
using shared.Models;

namespace domain.Entities;

public class SeoAnalytics : BaseEntity<int>
{
    public string EntityType { get; set; } = string.Empty; // Product, Article, Project, etc.
    public int EntityId { get; set; }
    public string? EntityTitle { get; set; }
    public string? EntityUrl { get; set; }
    public int Impressions { get; set; } = 0; // Số lần hiển thị trên SERP
    public int Clicks { get; set; } = 0; // Số lần click từ SERP
    public double CTR { get; set; } = 0; // Click-through rate
    public double AveragePosition { get; set; } = 0; // Vị trí trung bình trên SERP
    public string? TopKeywords { get; set; } // JSON array of top keywords
    public DateTime Date { get; set; } = DateTime.UtcNow.Date;
}

public class SeoAnalyticsConfiguration : BaseEntityConfiguration<SeoAnalytics, int>
{
    public override void Configure(EntityTypeBuilder<SeoAnalytics> builder)
    {
        base.Configure(builder);

        builder.ToTable("seo_analytics");

        builder.Property(e => e.EntityType).HasColumnName("entity_type").IsRequired().HasMaxLength(50);
        builder.Property(e => e.EntityId).HasColumnName("entity_id");
        builder.Property(e => e.EntityTitle).HasColumnName("entity_title").HasMaxLength(255);
        builder.Property(e => e.EntityUrl).HasColumnName("entity_url").HasMaxLength(500);
        builder.Property(e => e.Impressions).HasColumnName("impressions").HasDefaultValue(0);
        builder.Property(e => e.Clicks).HasColumnName("clicks").HasDefaultValue(0);
        builder.Property(e => e.CTR).HasColumnName("ctr").HasPrecision(5, 2).HasDefaultValue(0);
        builder.Property(e => e.AveragePosition).HasColumnName("average_position").HasPrecision(5, 2).HasDefaultValue(0);
        builder.Property(e => e.TopKeywords).HasColumnName("top_keywords").HasColumnType("json");
        builder.Property(e => e.Date).HasColumnName("date");

        builder.HasIndex(e => new { e.EntityType, e.EntityId, e.Date })
            .HasDatabaseName("idx_seo_analytics_entity_date")
            .IsUnique();
        builder.HasIndex(e => e.EntityType).HasDatabaseName("idx_seo_analytics_entity_type");
        builder.HasIndex(e => e.Date).HasDatabaseName("idx_seo_analytics_date");
    }
}

