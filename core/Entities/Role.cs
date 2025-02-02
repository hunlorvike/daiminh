using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class Role : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public JsonDocument Permissions { get; set; }
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<User> Users { get; set; }
}

public class RoleConfiguration : BaseEntityConfiguration<Role, int>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        builder.ToTable("roles");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Permissions).HasColumnName("permissions").HasColumnType("jsonb");
        builder.Property(e => e.Description).HasColumnName("description");

        builder.HasIndex(e => e.Name).HasDatabaseName("idx_roles_name");
    }
}