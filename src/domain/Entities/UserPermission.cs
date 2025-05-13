using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class UserPermission
{
    public int UserId { get; set; }
    public int PermissionId { get; set; }
    public virtual User? User { get; set; }
    public virtual Permission? Permission { get; set; }
}

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.ToTable("user_permissions");

        builder.HasKey(up => new { up.UserId, up.PermissionId });
        builder.Property(up => up.UserId).HasColumnName("user_id");
        builder.Property(up => up.PermissionId).HasColumnName("permission_id");

        builder.HasOne(up => up.User)
               .WithMany(u => u.UserPermissions)
               .HasForeignKey(up => up.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(up => up.Permission)
               .WithMany(p => p.UserPermissions)
               .HasForeignKey(up => up.PermissionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}