using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class FAQ : BaseEntity<int>
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int OrderIndex { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public int? CategoryId { get; set; }
    public virtual Category Category { get; set; }
}

public class FAQConfiguration : BaseEntityConfiguration<FAQ, int>
{
    public override void Configure(EntityTypeBuilder<FAQ> builder)
    {
        base.Configure(builder);
        builder.ToTable("faqs");
        builder.Property(e => e.Question).HasColumnName("question").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Answer).HasColumnName("answer").IsRequired().HasColumnType("nvarchar(max)");
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.CategoryId).HasColumnName("category_id");
        builder.HasOne(e => e.Category)
            .WithMany(c => c.FAQs)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}