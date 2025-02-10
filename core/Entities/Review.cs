using core.Common.Enums;
using core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class Review : BaseEntity<int>
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public short Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

    // Navigation properties
    public virtual Product Product { get; set; } = new();
    public virtual User User { get; set; } = new();
}

public class ReviewConfiguration : BaseEntityConfiguration<Review, int>
{
    public override void Configure(EntityTypeBuilder<Review> builder)
    {
        base.Configure(builder);

        builder.ToTable("reviews");

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.Rating).HasColumnName("rating");
        builder.Property(e => e.Comment).HasColumnName("comment");
        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<ReviewStatus>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(ReviewStatus.Pending);

        builder.HasCheckConstraint("CK_Review_Rating", "rating BETWEEN 1 AND 5");

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("idx_reviews_product_id");
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("idx_reviews_user_id");

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}