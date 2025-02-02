using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ContentCategory : BaseEntity
{
    public int ContentItemId { get; set; }
    public int CategoryId { get; set; }

    // Navigation properties
    public Content ContentItem { get; set; }
    public Category Category { get; set; }
}

public class ContentCategoryConfiguration : BaseEntityConfiguration<ContentCategory>
{
    public override void Configure(EntityTypeBuilder<ContentCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("content_categories");

        builder.HasKey(x => new { x.ContentItemId, x.CategoryId });

        builder.Property(e => e.ContentItemId).HasColumnName("content_item_id");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        builder.HasOne(x => x.ContentItem)
            .WithMany(x => x.ContentCategories)
            .HasForeignKey(x => x.ContentItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ContentItemId)
            .HasDatabaseName("idx_content_categories_content_item_id");
        builder.HasIndex(x => x.CategoryId)
            .HasDatabaseName("idx_content_categories_category_id");
    }
}