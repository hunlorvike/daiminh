using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class Newsletter : BaseEntity<int>
{
    public string Email { get; set; } = string.Empty;
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
        builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(e => e.Email).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(100);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.IpAddress).HasMaxLength(50);
        builder.Property(e => e.UserAgent).HasMaxLength(255);
        builder.Property(e => e.ConfirmedAt);
        builder.Property(e => e.UnsubscribedAt);
    }
}