using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Category : SeoEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? ImageUrl { get; set; }
    public int? ParentId { get; set; }
    public int OrderIndex { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public CategoryType Type { get; set; } = CategoryType.Product;

    // Navigation properties
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category>? Children { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
    public virtual ICollection<Article>? Articles { get; set; }
    public virtual ICollection<Project>? Projects { get; set; }
    public virtual ICollection<Gallery>? Galleries { get; set; }
    public virtual ICollection<FAQ>? FAQs { get; set; }
}

public class CategoryConfiguration : SeoEntityConfiguration<Category, int>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.ToTable("categories");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(e => e.Icon).HasColumnName("icon").HasMaxLength(50);
        builder.Property(e => e.ImageUrl).HasColumnName("image_url").HasMaxLength(255);
        builder.Property(e => e.ParentId).HasColumnName("parent_id");
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<CategoryType>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(CategoryType.Product);

        builder.HasIndex(e => new { e.Slug, e.Type }).HasDatabaseName("idx_categories_slug_type").IsUnique();
        builder.HasIndex(e => e.ParentId).HasDatabaseName("idx_categories_parent_id");
        builder.HasIndex(e => e.IsActive).HasDatabaseName("idx_categories_is_active");
        builder.HasIndex(e => e.Type).HasDatabaseName("idx_categories_type");
        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_categories_order_index");

        builder.HasOne(e => e.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}