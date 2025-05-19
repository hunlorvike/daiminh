using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class UserRole : IdentityUserRole<int>
{
    // Không thêm gì, IdentityUserRole<int> đã đủ (UserId, RoleId)
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(x => new { x.UserId, x.RoleId });

        builder.HasData(new UserRole
        {
            UserId = 1,
            RoleId = 1
        });
    }
}
