using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;
using System.ComponentModel.DataAnnotations;

namespace domain.Entities;

public class Setting : BaseEntity<int>
{
    public string Key { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty; // e.g., "General", "Email", "Social Media", "Payment", "SEO", "Contact", "Company Info", "Theme", "Analytics", "Security", "Cache", "API", "Custom"

    public string Type { get; set; } = string.Empty; // e.g., "Text", "Number", "Boolean", "JSON", "HTML", "Image", "File", "Color", "Date", "Time", "DateTime", "Email", "URL", "Phone", "Address", "Currency", "Percentage", "Select", "MultiSelect", "Radio", "Checkbox", "TextArea", "RichText", "Code", "Password", "Secret", "Other"

    public string? Description { get; set; } // Description of the setting

    public string? DefaultValue { get; set; } // Default value for the setting

    public string? Value { get; set; } // Current value of the setting

    public bool IsActive { get; set; } = true; // Whether the setting is active
}

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("settings");

        builder.Property(s => s.Key).HasColumnName("key").IsRequired().HasMaxLength(100);
        builder.Property(s => s.Category).HasColumnName("category").IsRequired().HasMaxLength(50);
        builder.Property(s => s.Type).HasColumnName("type").IsRequired().HasMaxLength(50);
        builder.Property(s => s.Description).HasColumnName("description").HasMaxLength(500);
        builder.Property(s => s.DefaultValue).HasColumnName("default_value");
        builder.Property(s => s.Value).HasColumnName("value");
        builder.Property(s => s.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        // Indexes
        builder.HasIndex(s => s.Key).HasDatabaseName("idx_settings_key").IsUnique();
        builder.HasIndex(s => s.Category).HasDatabaseName("idx_settings_category");
        builder.HasIndex(s => s.Type).HasDatabaseName("idx_settings_type");
        builder.HasIndex(s => s.IsActive).HasDatabaseName("idx_settings_is_active");
    }
} 