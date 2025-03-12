using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Constants;
using shared.Interfaces;
using shared.Models;

namespace domain.Entities;

public class Role : BaseEntity<int>
{
    public string? Name { get; set; } = string.Empty;
    public string Permissions { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<User>? Users { get; set; }
}

public class RoleConfiguration : BaseEntityConfiguration<Role, int>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        builder.ToTable("roles");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Permissions).HasColumnName("permissions");

        builder.HasIndex(e => e.Name).HasDatabaseName("idx_roles_name");

        builder.HasData(new RoleSeeder().DataSeeder());
    }
}

public class RoleSeeder : ISeeder<Role>
{
    public IEnumerable<Role> DataSeeder()
    {
        return
        [
            new Role
            {
                Id = 1,
                Name = RoleConstants.Admin,
                Permissions = ""
            },
            new Role
            {
                Id = 2,
                Name = RoleConstants.User,
                Permissions = ""
            },
            new Role
            {
                Id = 3,
                Name = RoleConstants.Manager,
                Permissions = ""
            }
        ];
    }
}