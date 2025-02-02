using core.Common.Enums;
using core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ContentComment : BaseEntity<int>
{
    public int ContentItemId { get; set; }
    public int? UserId { get; set; }
    public int? ParentCommentId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public CommentStatus Status { get; set; }

    // Navigation properties
    public Content Content { get; set; }
    public User User { get; set; }
    public ContentComment ParentComment { get; set; }
    public ICollection<ContentComment> ChildComments { get; set; }
}

public class ContentCommentConfiguration : BaseEntityConfiguration<ContentComment, int>
{
    public override void Configure(EntityTypeBuilder<ContentComment> builder)
    {
        base.Configure(builder);

        builder.ToTable("content_comments");

        builder.Property(e => e.ContentItemId).HasColumnName("content_item_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.ParentCommentId).HasColumnName("parent_comment_id");
        builder.Property(e => e.Subject).HasColumnName("subject").IsRequired();
        builder.Property(e => e.Status).HasColumnName("status")
            .HasConversion(v => v.ToStringValue(), v => v.ToCommentStatusEnum()).IsRequired().HasMaxLength(20)
            .HasDefaultValue(CommentStatus.Approved);

        builder.HasOne(x => x.Content)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.ContentItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ParentComment)
            .WithMany(x => x.ChildComments)
            .HasForeignKey(x => x.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ContentItemId)
            .HasDatabaseName("idx_content_comments_content_item_id");
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("idx_content_comments_user_id");
        builder.HasIndex(x => x.ParentCommentId)
            .HasDatabaseName("idx_content_comments_parent_comment_id");
    }
}