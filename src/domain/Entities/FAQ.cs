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
    public virtual Category? Category { get; set; }
}

public class FAQConfiguration : BaseEntityConfiguration<FAQ, int>
{
    public override void Configure(EntityTypeBuilder<FAQ> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Question).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Answer).IsRequired().HasColumnType("TEXT");
        builder.Property(e => e.OrderIndex).HasDefaultValue(0);
        builder.HasIndex(e => e.OrderIndex);

        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.CategoryId);
        builder.HasOne(e => e.Category)
            .WithMany(c => c.FAQs)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}