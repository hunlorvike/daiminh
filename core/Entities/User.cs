using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int? RoleId { get; set; }

    // Navigation properties
    public Role? Role { get; set; }
    public ICollection<Content> ContentItems { get; set; }
    public ICollection<ContentComment> ContentComments { get; set; }
    public ICollection<ProductComment> ProductComments { get; set; }
    public ICollection<ProductReview> ProductReviews { get; set; }
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
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}