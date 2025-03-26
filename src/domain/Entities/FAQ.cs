using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class FAQ : BaseEntity<int>
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int OrderIndex { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual FAQCategory? Category { get; set; }
}

public class FAQConfiguration : BaseEntityConfiguration<FAQ, int>
{
    public override void Configure(EntityTypeBuilder<FAQ> builder)
    {
        base.Configure(builder);

        builder.ToTable("faqs");

        builder.Property(e => e.Question).HasColumnName("question").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Answer).HasColumnName("answer").IsRequired().HasColumnType("text");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        builder.HasIndex(e => e.CategoryId).HasDatabaseName("idx_faqs_category_id");
        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_faqs_order_index");
        builder.HasIndex(e => e.IsActive).HasDatabaseName("idx_faqs_is_active");

        builder.HasOne(e => e.Category)
            .WithMany(c => c.FAQs)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

