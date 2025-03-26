using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class SeoSettings : BaseEntity<int>
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Các cài đặt SEO toàn trang
    public static string DefaultTitle = "default_title";
    public static string DefaultDescription = "default_description";
    public static string DefaultKeywords = "default_keywords";
    public static string SiteName = "site_name";
    public static string GoogleAnalyticsId = "google_analytics_id";
    public static string GoogleTagManagerId = "google_tag_manager_id";
    public static string FacebookAppId = "facebook_app_id";
    public static string TwitterUsername = "twitter_username";
    public static string RobotsContent = "robots_content";
    public static string SitemapSettings = "sitemap_settings";
    public static string StructuredDataOrganization = "structured_data_organization";
    public static string StructuredDataWebsite = "structured_data_website";
    public static string CustomHeadCode = "custom_head_code";
    public static string CustomFooterCode = "custom_footer_code";
}

public class SeoSettingsConfiguration : BaseEntityConfiguration<SeoSettings, int>
{
    public override void Configure(EntityTypeBuilder<SeoSettings> builder)
    {
        base.Configure(builder);

        builder.ToTable("seo_settings");

        builder.Property(e => e.Key).HasColumnName("key").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Value).HasColumnName("value").HasColumnType("text");
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        builder.HasIndex(e => e.Key).HasDatabaseName("idx_seo_settings_key").IsUnique();
        builder.HasIndex(e => e.IsActive).HasDatabaseName("idx_seo_settings_is_active");
    }
}

