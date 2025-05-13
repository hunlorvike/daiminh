using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class Permission : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<RolePermission>? RolePermissions { get; set; }
    public virtual ICollection<UserPermission>? UserPermissions { get; set; }
}

public class PermissionConfiguration : BaseEntityConfiguration<Permission, int>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);

        builder.ToTable("permissions");
        builder.Property(p => p.Name)
               .HasColumnName("name")
               .IsRequired()
               .HasMaxLength(100);
        builder.HasIndex(p => p.Name)
               .IsUnique();
        builder.HasMany(p => p.RolePermissions)
               .WithOne(rp => rp.Permission)
               .HasForeignKey(rp => rp.PermissionId);
    }
}