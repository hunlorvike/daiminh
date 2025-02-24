using core.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BC = BCrypt.Net.BCrypt;

namespace core.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int? RoleId { get; set; }

    // Navigation properties
    public virtual Role? Role { get; set; }
    public virtual ICollection<Content>? Contents { get; set; }
    public virtual ICollection<Comment>? Comments { get; set; }
    public virtual ICollection<Review>? Reviews { get; set; }
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
        builder.Property(x => x.RoleId).HasColumnName("role_id");

        builder.HasIndex(x => x.Username).HasDatabaseName("idx_users_username").IsUnique();
        builder.HasIndex(x => x.Email).HasDatabaseName("idx_users_email").IsUnique();

        builder.HasOne(x => x.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(new UserSeeder().DataSeeder());
    }
}

public class UserSeeder : ISeeder<User>
{
    public IEnumerable<User> DataSeeder()
    {
        return
        [
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@admin.com",
                PasswordHash = BC.HashPassword("123456a@A"),
                RoleId = 1
            },
            new User
            {
                Id = 2,
                Username = "user",
                Email = "user@user.com",
                PasswordHash = BC.HashPassword("123456a@A"),
                RoleId = 2
            },
            new User
            {
                Id = 3,
                Username = "manager",
                Email = "manager@manager.com",
                PasswordHash = BC.HashPassword("123456a@A"),
                RoleId = 3
            }
        ];
    }
}