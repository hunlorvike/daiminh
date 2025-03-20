using domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
using shared.Models;

namespace domain.Entities;

public class Category : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
    public EntityType EntityType { get; set; } = EntityType.Product;

    // Navigation properties
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category>? ChildCategories { get; set; }
    public virtual ICollection<ContentCategory>? ContentCategories { get; set; }
    public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
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
        builder.Property(x => x.EntityType).HasColumnName("entity_type").IsRequired().HasDefaultValue(EntityType.Product);

        builder.HasIndex(e => e.Name).IsUnique().HasDatabaseName("idx_categories_name");
        builder.HasIndex(e => e.ParentCategoryId).HasDatabaseName("idx_categories_parent_category_id");

        builder.HasOne(x => x.ParentCategory)
            .WithMany(x => x.ChildCategories)
            .HasForeignKey(x => x.ParentCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(new CategorySeeder().DataSeeder());
    }
}

public class CategorySeeder : ISeeder<Category>
{
    public IEnumerable<Category> DataSeeder()
    {
        return
        [
            // Danh mục sản phẩm
            new Category{ Id = 1, Name = "Sơn nội thất", Slug = "son-noi-that", EntityType = EntityType.Product },
            new Category { Id = 2, Name = "Sơn ngoại thất", Slug = "son-ngoai-that", EntityType = EntityType.Product },
            new Category { Id = 3, Name = "Sơn chống thấm", Slug = "son-chong-tham", EntityType = EntityType.Product },
            new Category { Id = 4, Name = "Sơn lót", Slug = "son-lot", EntityType = EntityType.Product },
            new Category { Id = 5, Name = "Sơn gỗ", Slug = "son-go", EntityType = EntityType.Product },
            new Category { Id = 6, Name = "Sơn kim loại", Slug = "son-kim-loai", EntityType = EntityType.Product },
            new Category { Id = 7, Name = "Dụng cụ sơn", Slug = "dung-cu-son", EntityType = EntityType.Product },

            // Danh mục con của "Sơn nội thất"
            new Category{ Id = 8, Name = "Sơn bóng nội thất", Slug = "son-bong-noi-that", EntityType = EntityType.Product, ParentCategoryId = 1 },
            new Category{ Id = 9, Name = "Sơn mờ nội thất", Slug = "son-mo-noi-that", EntityType = EntityType.Product, ParentCategoryId = 1 },

            // Danh mục con của "Sơn ngoại thất"
            new Category{ Id = 10, Name = "Sơn bóng ngoại thất", Slug = "son-bong-ngoai-that", EntityType = EntityType.Product, ParentCategoryId = 2 },
            new Category{ Id = 11, Name = "Sơn mờ ngoại thất", Slug = "son-mo-ngoai-that", EntityType = EntityType.Product, ParentCategoryId = 2 },

            // Danh mục con của "Sơn chống thấm"
            new Category{ Id = 12, Name = "Sơn chống thấm gốc nước", Slug = "son-chong-tham-goc-nuoc", EntityType = EntityType.Product, ParentCategoryId = 3 },
            new Category{ Id = 13, Name = "Sơn chống thấm gốc dầu", Slug = "son-chong-tham-goc-dau", EntityType = EntityType.Product, ParentCategoryId = 3 },

            // Danh mục con của "Dụng cụ sơn"
            new Category{ Id = 14, Name = "Cọ sơn", Slug = "co-son", EntityType = EntityType.Product, ParentCategoryId = 7 },
            new Category{ Id = 15, Name = "Con lăn sơn", Slug = "con-lan-son", EntityType = EntityType.Product, ParentCategoryId = 7 },
            new Category{ Id = 16, Name = "Băng keo dán sơn", Slug = "bang-keo-dan-son", EntityType = EntityType.Product, ParentCategoryId = 7 },

            // Danh mục dịch vụ
            new Category{ Id = 101, Name = "Dịch vụ thi công sơn trọn gói", Slug = "dich-vu-thi-cong-son-tron-goi", EntityType = EntityType.Service },
            new Category{ Id = 102, Name = "Tư vấn phối màu sơn", Slug = "tu-van-phoi-mau-son", EntityType = EntityType.Service },
            new Category{ Id = 103, Name = "Tư vấn kỹ thuật sơn", Slug = "tu-van-ky-thuat-son", EntityType = EntityType.Service },
            // Bạn có thể thêm các danh mục con cho dịch vụ nếu cần
            new Category{ Id = 104, Name = "Thi công sơn nội thất trọn gói", Slug = "thi-cong-son-noi-that-tron-goi", EntityType = EntityType.Service, ParentCategoryId = 101 },
            new Category{ Id = 105, Name = "Thi công sơn ngoại thất trọn gói", Slug = "thi-cong-son-ngoai-that-tron-goi", EntityType = EntityType.Service, ParentCategoryId = 101 },
        ];
    }
}