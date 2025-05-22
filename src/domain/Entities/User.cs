using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class User : IdentityUser<int>
{
    public string? FullName { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual ICollection<ProductReview>? ReviewsWritten { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.Property(x => x.FullName).HasMaxLength(100);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
    }
}
