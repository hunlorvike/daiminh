using core.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class Setting : BaseEntity<int>
{
    public string Group { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
}

public class SettingConfiguration : BaseEntityConfiguration<Setting, int>
{
    public override void Configure(EntityTypeBuilder<Setting> builder)
    {
        base.Configure(builder);

        builder.ToTable("settings");

        builder.Property(x => x.Group).HasColumnName("group").HasMaxLength(100).IsRequired();
        builder.Property(e => e.Key).HasColumnName("key").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Value).HasColumnName("value").IsRequired();
        builder.Property(e => e.Description).HasColumnName("description").IsRequired();
        builder.Property(x => x.Order).HasColumnName("order_number").IsRequired();

        builder.HasIndex(e => e.Key)
            .HasDatabaseName("idx_settings_key")
            .IsUnique();
    }
}