using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class Category : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }

    // Navigation properties
    public Category ParentCategory { get; set; }
    public ICollection<Category> ChildCategories { get; set; }
    public ICollection<Content> ContentItems { get; set; }
    public ICollection<Product> Products { get; set; }
}

public class CategoryConfiguration : BaseEntityConfiguration<Category, int>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.ToTable("categories");

        builder.Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(x => x.Slug).HasColumnName("slug").IsRequired().HasMaxLength(100);
        builder.Property(x => x.ParentCategoryId).HasColumnName("parent_category_id");

        builder.HasIndex(e => e.Name).IsUnique().HasDatabaseName("idx_categories_name");
        builder.HasIndex(e => e.ParentCategoryId).HasDatabaseName("idx_categories_parent_category_id");

        builder.HasOne(x => x.ParentCategory)
            .WithMany(x => x.ChildCategories)
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}