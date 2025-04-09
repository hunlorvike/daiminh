using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class FAQCategory : BaseEntity<int>
{
    public int FAQId { get; set; }
    public int CategoryId { get; set; }

    // Navigation properties
    public virtual FAQ? FAQ { get; set; }
    public virtual Category? Category { get; set; }
}

public class FAQCategoryConfiguration : BaseEntityConfiguration<FAQCategory, int>
{
    public override void Configure(EntityTypeBuilder<FAQCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("faq_categories");

        builder.Property(e => e.FAQId).HasColumnName("faq_id");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        builder.HasIndex(e => new { e.FAQId, e.CategoryId })
            .HasDatabaseName("idx_faq_categories_faq_category")
            .IsUnique();

        builder.HasIndex(e => e.FAQId).HasDatabaseName("idx_faq_categories_faq_id");
        builder.HasIndex(e => e.CategoryId).HasDatabaseName("idx_faq_categories_category_id");

        builder.HasOne(e => e.FAQ)
            .WithMany(g => g.FAQCategories)
            .HasForeignKey(e => e.FAQId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.FAQCategories)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

