using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class FAQCategory : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Slug { get; set; } = string.Empty;
    public int OrderIndex { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<FAQ>? FAQs { get; set; }
}

public class FAQCategoryConfiguration : BaseEntityConfiguration<FAQCategory, int>
{
    public override void Configure(EntityTypeBuilder<FAQCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("faq_categories");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(100);
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_faq_categories_slug").IsUnique();
        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_faq_categories_order_index");
        builder.HasIndex(e => e.IsActive).HasDatabaseName("idx_faq_categories_is_active");
    }
}

