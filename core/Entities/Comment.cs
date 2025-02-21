using core.Common.Enums;
using core.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class Comment : BaseEntity<int>
{
    public int ContentId { get; set; }
    public int? UserId { get; set; }
    public int? ParentCommentId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public CommentStatus Status { get; set; } = CommentStatus.Approved;

    // Navigation properties
    public virtual Content Content { get; set; } = new();
    public virtual User User { get; set; } = new();
    public virtual Comment ParentComment { get; set; } = new();
    public virtual ICollection<Comment> ChildComments { get; set; } = [];
}

public class CommentConfiguration : BaseEntityConfiguration<Comment, int>
{
    public override void Configure(EntityTypeBuilder<Comment> builder)
    {
        base.Configure(builder);

        builder.ToTable("comments");

        builder.Property(e => e.ContentId).HasColumnName("content_id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.ParentCommentId).HasColumnName("parent_comment_id");
        builder.Property(e => e.Subject).HasColumnName("subject").IsRequired();
        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<CommentStatus>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(CommentStatus.Approved);

        builder.HasIndex(x => x.ContentId)
            .HasDatabaseName("idx_comments_content_id");
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("idx_comments_user_id");
        builder.HasIndex(x => x.ParentCommentId)
            .HasDatabaseName("idx_comments_parent_comment_id");

        builder.HasOne(x => x.ParentComment)
            .WithMany(x => x.ChildComments)
            .HasForeignKey(x => x.ParentCommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Content)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}