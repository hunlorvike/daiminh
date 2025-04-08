using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}

public class UserConfiguration : BaseEntityConfiguration<User, int>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("users");

        builder.Property(x => x.Username).HasColumnName("username").IsRequired().HasMaxLength(50);
        builder.Property(x => x.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
        builder.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired();

        builder.HasIndex(x => x.Username).HasDatabaseName("idx_users_username");
        builder.HasIndex(x => x.Email).HasDatabaseName("idx_users_email");

        var hasher = new PasswordHasher<User>();

        User adminUser = new()
        {
            Id = 1,
            Username = "admin",
            Email = "admin@admin.com",
            PasswordHash = ""
        };

        adminUser.PasswordHash = hasher.HashPassword(adminUser, "123123123");

        builder.HasData(adminUser);
    }
}
