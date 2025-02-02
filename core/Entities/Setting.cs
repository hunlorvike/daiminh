using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class Setting : BaseEntity<int>
{
    public string SettingKey { get; set; } = string.Empty;
    public string SettingValue { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class SettingConfiguration : BaseEntityConfiguration<Setting, int>
{
    public override void Configure(EntityTypeBuilder<Setting> builder)
    {
        base.Configure(builder);

        builder.ToTable("settings");

        builder.Property(e => e.SettingKey).HasColumnName("setting_key").IsRequired().HasMaxLength(50);
        builder.Property(e => e.SettingValue).HasColumnName("setting_value").IsRequired();
        builder.Property(e => e.Description).HasColumnName("description");

        builder.HasIndex(e => e.SettingKey)
            .HasDatabaseName("idx_settings_setting_key")
            .IsUnique();
    }
}