using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class Role : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<UserRole>? UserRoles { get; set; }
    public virtual ICollection<RolePermission>? RolePermissions { get; set; }
}

public class RoleConfiguration : BaseEntityConfiguration<Role, int>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        builder.ToTable("roles");
        builder.Property(r => r.Name)
               .HasColumnName("name")
               .IsRequired()
               .HasMaxLength(50);
        builder.HasIndex(r => r.Name)
               .IsUnique();
        builder.HasMany(r => r.UserRoles)
               .WithOne(ur => ur.Role)
               .HasForeignKey(ur => ur.RoleId);
        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId);

        builder.HasData(
            new Role { Id = 1, Name = "Admin", CreatedAt = DateTime.UtcNow },
            new Role { Id = 2, Name = "Manager", CreatedAt = DateTime.UtcNow },
            new Role { Id = 3, Name = "User", CreatedAt = DateTime.UtcNow }
        );
    }
}