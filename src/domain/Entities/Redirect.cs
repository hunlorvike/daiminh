using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
using shared.Models;

namespace domain.Entities;

public class Redirect : BaseEntity<int>
{
    public string SourceUrl { get; set; } = string.Empty;
    public string TargetUrl { get; set; } = string.Empty;
    public RedirectType Type { get; set; } = RedirectType.Permanent; // 301 or 302
    public bool IsRegex { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public int HitCount { get; set; } = 0;
    public string? Notes { get; set; }
}

public class RedirectConfiguration : BaseEntityConfiguration<Redirect, int>
{
    public override void Configure(EntityTypeBuilder<Redirect> builder)
    {
        base.Configure(builder);

        builder.ToTable("redirects");

        builder.Property(e => e.SourceUrl).HasColumnName("source_url").IsRequired().HasMaxLength(500);
        builder.Property(e => e.TargetUrl).HasColumnName("target_url").IsRequired().HasMaxLength(500);
        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<RedirectType>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(RedirectType.Permanent);
        builder.Property(e => e.IsRegex).HasColumnName("is_regex").HasDefaultValue(false);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.HitCount).HasColumnName("hit_count").HasDefaultValue(0);
        builder.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(255);

        builder.HasIndex(e => e.SourceUrl).HasDatabaseName("idx_redirects_source_url");
        builder.HasIndex(e => e.IsActive).HasDatabaseName("idx_redirects_is_active");
    }
}

