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

        builder.ToTable("categories");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(100);
        builder.HasIndex(e => e.Slug).IsUnique();

        builder.Property(e => e.Description).HasColumnName("description").HasColumnType("nvarchar(max)");

        builder.Property(e => e.Icon).HasColumnName("icon").HasMaxLength(50);
        builder.Property(e => e.ParentId).HasColumnName("parent_id");
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.Type)
            .HasColumnName("type")
            .IsRequired()
            .HasDefaultValue(CategoryType.Product);

        builder.HasOne(e => e.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Category { Id = 1, Name = "Sơn", Slug = "son", Type = CategoryType.Product, OrderIndex = 0, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 2, Name = "Chống Thấm", Slug = "chong-tham", Type = CategoryType.Product, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 3, Name = "Vật Liệu Xây Dựng", Slug = "vat-lieu-xay-dung", Type = CategoryType.Product, OrderIndex = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 4, Name = "Tin Tức & Sự Kiện", Slug = "tin-tuc-su-kien", Type = CategoryType.Article, OrderIndex = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 5, Name = "Câu Hỏi Thường Gặp", Slug = "cau-hoi-thuong-gap", Type = CategoryType.FAQ, OrderIndex = 4, IsActive = true, CreatedAt = DateTime.UtcNow },

            new Category { Id = 6, ParentId = 1, Name = "Sơn Nội Thất", Slug = "son-noi-that", Type = CategoryType.Product, OrderIndex = 0, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 7, ParentId = 1, Name = "Sơn Ngoại Thất", Slug = "son-ngoai-that", Type = CategoryType.Product, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 8, ParentId = 1, Name = "Sơn Lót", Slug = "son-lot", Type = CategoryType.Product, OrderIndex = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 9, ParentId = 1, Name = "Sơn Chống Kiềm", Slug = "son-chong-kiem", Type = CategoryType.Product, OrderIndex = 3, IsActive = true, CreatedAt = DateTime.UtcNow },

            new Category { Id = 10, ParentId = 2, Name = "Chống Thấm Sàn Mái", Slug = "chong-tham-san-mai", Type = CategoryType.Product, OrderIndex = 0, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 11, ParentId = 2, Name = "Chống Thấm Tường", Slug = "chong-tham-tuong", Type = CategoryType.Product, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 12, ParentId = 2, Name = "Chống Thấm Nhà Vệ Sinh", Slug = "chong-tham-nha-ve-sinh", Type = CategoryType.Product, OrderIndex = 2, IsActive = true, CreatedAt = DateTime.UtcNow },

            new Category { Id = 13, ParentId = 3, Name = "Phụ Gia Bê Tông", Slug = "phu-gia-be-tong", Type = CategoryType.Product, OrderIndex = 0, IsActive = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 14, ParentId = 3, Name = "Keo Chít Mạch", Slug = "keo-chit-mach", Type = CategoryType.Product, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.UtcNow }
        );
    }
}