using core.Common.Enums;
using core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ProductComment : BaseEntity<int>
{
    public int ProductId { get; set; }
    public int? UserId { get; set; }
    public int? ParentCommentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public CommentStatus Status { get; set; }

    // Navigation properties
    public Product Product { get; set; }
    public User User { get; set; }
    public ProductComment ParentComment { get; set; }
    public ICollection<ProductComment> ChildComments { get; set; }
}

public class ProductCommentConfiguration : BaseEntityConfiguration<ProductComment, int>
{
    public override void Configure(EntityTypeBuilder<ProductComment> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_comments");

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.ParentCommentId).HasColumnName("parent_comment_id");
        builder.Property(e => e.Content).HasColumnName("content").IsRequired();
        builder.Property(e => e.Status).HasColumnName("status")
            .HasConversion(v => v.ToStringValue(), v => v.ToCommentStatusEnum()).IsRequired().HasMaxLength(20)
            .HasDefaultValue(CommentStatus.Approved);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ParentComment)
            .WithMany(x => x.ChildComments)
            .HasForeignKey(x => x.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("idx_product_comments_product_id");
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("idx_product_comments_user_id");
        builder.HasIndex(x => x.ParentCommentId)
            .HasDatabaseName("idx_product_comments_parent_comment_id");
    }
}