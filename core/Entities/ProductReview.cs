using core.Common.Enums;
using core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ProductReview : BaseEntity<int>
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public short Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public ReviewStatus Status { get; set; }

    // Navigation properties
    public Product Product { get; set; }
    public User User { get; set; }
}

public class ProductReviewConfiguration : BaseEntityConfiguration<ProductReview, int>
{
    public override void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_reviews");

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.Rating).HasColumnName("rating");
        builder.Property(e => e.Comment).HasColumnName("comment");
        builder.Property(e => e.Status).HasColumnName("status")
            .HasConversion(v => v.ToStringValue(), v => v.ToReviewStatusEnum()).IsRequired().HasMaxLength(20)
            .HasDefaultValue(ReviewStatus.Pending);

        builder.HasCheckConstraint("CK_ProductReview_Rating", "rating BETWEEN 1 AND 5");

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("idx_product_reviews_product_id");
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("idx_product_reviews_user_id");
    }
}