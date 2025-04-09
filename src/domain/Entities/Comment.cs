using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class Comment : BaseEntity<int>
{
    public string Content { get; set; } = string.Empty;
    public string? AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorEmail { get; set; }
    public string? AuthorAvatar { get; set; }
    public string? AuthorWebsite { get; set; }
    public bool IsApproved { get; set; } = false;
    public int? ParentId { get; set; }
    public int ArticleId { get; set; }

    // Navigation properties
    public virtual Comment? Parent { get; set; }
    public virtual ICollection<Comment>? Replies { get; set; }
    public virtual Article? Article { get; set; }
}

public class CommentConfiguration : BaseEntityConfiguration<Comment, int>
{
    public override void Configure(EntityTypeBuilder<Comment> builder)
    {
        base.Configure(builder);

        builder.ToTable("comments");

        builder.Property(e => e.Content).HasColumnName("content").IsRequired().HasColumnType("text");
        builder.Property(e => e.AuthorId).HasColumnName("author_id").HasMaxLength(50);
        builder.Property(e => e.AuthorName).HasColumnName("author_name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.AuthorEmail).HasColumnName("author_email").HasMaxLength(100);
        builder.Property(e => e.AuthorAvatar).HasColumnName("author_avatar").HasMaxLength(255);
        builder.Property(e => e.AuthorWebsite).HasColumnName("author_website").HasMaxLength(255);
        builder.Property(e => e.IsApproved).HasColumnName("is_approved").HasDefaultValue(false);
        builder.Property(e => e.ParentId).HasColumnName("parent_id");
        builder.Property(e => e.ArticleId).HasColumnName("article_id");

        builder.HasIndex(e => e.ArticleId).HasDatabaseName("idx_comments_article_id");
        builder.HasIndex(e => e.ParentId).HasDatabaseName("idx_comments_parent_id");
        builder.HasIndex(e => e.IsApproved).HasDatabaseName("idx_comments_is_approved");
        builder.HasIndex(e => e.AuthorId).HasDatabaseName("idx_comments_author_id");

        builder.HasOne(e => e.Article)
            .WithMany(a => a.Comments)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Parent)
            .WithMany(c => c.Replies)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

