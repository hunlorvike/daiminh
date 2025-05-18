using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;
public class ProductReview : BaseEntity<int>
{
    public int ProductId { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; } = string.Empty;
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    public virtual Product? Product { get; set; }
    public virtual User? User { get; set; }
}

public class ProductReviewConfiguration : BaseEntityConfiguration<ProductReview, int>
{
    public override void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.UserId);
        builder.Property(e => e.UserName).HasMaxLength(100);
        builder.Property(e => e.UserEmail).HasMaxLength(255);
        builder.HasIndex(e => e.ProductId);
        builder.HasIndex(e => e.Rating);
        builder.Property(e => e.Rating).IsRequired();
        builder.Property(e => e.Content).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(e => e.Status)
            .IsRequired()
            .HasDefaultValue(ReviewStatus.Pending);
        builder.HasOne(e => e.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.User)
            .WithMany(u => u.ReviewsWritten)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}