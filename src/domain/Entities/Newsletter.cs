using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class Newsletter : BaseEntity<int>
{
    public string Email { get; set; } = string.Empty;
    public string? Name { get; set; }
    public bool IsActive { get; set; } = true;  } = string.Empty;
    public string? Name { get; set; }
    public bool IsActive { get; set; } = true;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? UnsubscribedAt { get; set; }
}

public class NewsletterConfiguration : BaseEntityConfiguration<Newsletter, int>
{
    public override void Configure(EntityTypeBuilder<Newsletter> builder)
    {
        base.Configure(builder);

        builder.ToTable("newsletters");

        builder.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(100);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(50);
        builder.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(255);
        builder.Property(e => e.ConfirmedAt).HasColumnName("confirmed_at");
        builder.Property(e => e.UnsubscribedAt).HasColumnName("unsubscribed_at");

        builder.HasIndex(e => e.Email).HasDatabaseName("idx_newsletters_email").IsUnique();
        builder.HasIndex(e => e.IsActive).HasDatabaseName("idx_newsletters_is_active");
    }
}

