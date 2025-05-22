using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class Role : IdentityRole<int>
{
    // IdentityRole<int> đã có: Id, Name, NormalizedName, ConcurrencyStamp
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
    }
}
