using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ContentCategory : BaseEntity
{
    public int ContentId { get; set; }
    public int CategoryId { get; set; }

    // Navigation properties
    public virtual Content Content { get; set; } = new();
    public virtual Category Category { get; set; } = new();
}

public class ContentCategoryConfiguration : BaseEntityConfiguration<ContentCategory>
{
    public override void Configure(EntityTypeBuilder<ContentCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("content_categories");

        builder.HasKey(x => new { x.ContentId, x.CategoryId });

        builder.Property(e => e.ContentId).HasColumnName("content_id");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        builder.HasIndex(x => x.ContentId)
            .HasDatabaseName("idx_content_categories_content_id");
        builder.HasIndex(x => x.CategoryId)
            .HasDatabaseName("idx_content_categories_category_id");

        builder.HasOne(x => x.Content)
            .WithMany(x => x.ContentCategories)
            .HasForeignKey(x => x.ContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.ContentCategories)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}