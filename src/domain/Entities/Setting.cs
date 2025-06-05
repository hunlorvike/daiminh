using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Setting : BaseEntity<int>
{
    public string Key { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // e.g., "General", "Email", "Social Media", "Payment", "SEO", "Contact", "Company Info", "Theme", "Analytics", "Security", "Cache", "API", "Custom"
    public FieldType Type { get; set; }
    public string? Description { get; set; }
    public string? DefaultValue { get; set; }
    public string? Value { get; set; }
    public bool IsActive { get; set; } = true;
}

public class SettingConfiguration : BaseEntityConfiguration<Setting, int>
{
    public override void Configure(EntityTypeBuilder<Setting> builder)
    {
        base.Configure(builder);
        builder.Property(s => s.Key).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Category).IsRequired().HasMaxLength(50);
        builder.HasIndex(s => new { s.Key, s.Category }).IsUnique();
        builder.Property(e => e.Type)
            .IsRequired()
            .HasDefaultValue(FieldType.Text);
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.Property(s => s.DefaultValue).HasColumnType("TEXT");
        builder.Property(s => s.Value).HasColumnType("TEXT");
        builder.Property(s => s.IsActive).HasDefaultValue(true);
    }
}