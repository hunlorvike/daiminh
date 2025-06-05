using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Category : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public int? ParentId { get; set; }
    public int OrderIndex { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public CategoryType Type { get; set; } = CategoryType.Product;
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category>? Children { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
    public virtual ICollection<Article>? Articles { get; set; }
    public virtual ICollection<FAQ>? FAQs { get; set; }
}

public class CategoryConfiguration : BaseEntityConfiguration<Category, int>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Slug).IsRequired().HasMaxLength(100);
        builder.HasIndex(e => e.Slug).IsUnique();

        builder.Property(e => e.Description).HasColumnType("TEXT");

        builder.Property(e => e.Icon).HasMaxLength(50);
        builder.Property(e => e.ParentId);
        builder.Property(e => e.OrderIndex).HasDefaultValue(0);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.Type)
            .IsRequired()
            .HasDefaultValue(CategoryType.Product);

        builder.HasOne(e => e.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}