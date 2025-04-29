using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Tag : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TagType Type { get; set; } = TagType.Product;
    public virtual ICollection<ProductTag>? ProductTags { get; set; }
    public virtual ICollection<ArticleTag>? ArticleTags { get; set; }
}

public class TagConfiguration : BaseEntityConfiguration<Tag, int>
{
    public override void Configure(EntityTypeBuilder<Tag> builder)
    {
        base.Configure(builder);
        builder.ToTable("tags");
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(50);
        builder.HasIndex(e => e.Slug).IsUnique();

        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(e => e.Type)
            .HasColumnName("type")
            .IsRequired()
            .HasDefaultValue(TagType.Product);

        builder.HasData(
            new Tag { Id = 1, Name = "Chống thấm", Slug = "chong-tham", Type = TagType.Product, CreatedAt = DateTime.UtcNow },
            new Tag { Id = 2, Name = "Sơn nội thất", Slug = "son-noi-that", Type = TagType.Product, CreatedAt = DateTime.UtcNow }, // Đổi slug để tránh trùng với category
            new Tag { Id = 3, Name = "Sơn ngoại thất", Slug = "son-ngoai-that", Type = TagType.Product, CreatedAt = DateTime.UtcNow }, // Đổi slug
            new Tag { Id = 4, Name = "Phụ gia bê tông", Slug = "phu-gia-be-tong", Type = TagType.Product, CreatedAt = DateTime.UtcNow }, // Đổi slug
            new Tag { Id = 5, Name = "Keo chít mạch", Slug = "keo-chit-mach", Type = TagType.Product, CreatedAt = DateTime.UtcNow }, // Đổi slug
            new Tag { Id = 6, Name = "Vật liệu xây dựng", Slug = "vat-lieu-xay-dung", Type = TagType.Product, CreatedAt = DateTime.UtcNow }, // Đổi slug

            new Tag { Id = 7, Name = "Tư vấn chọn sơn", Slug = "tu-van-chon-son", Type = TagType.Article, CreatedAt = DateTime.UtcNow },
            new Tag { Id = 8, Name = "Hướng dẫn thi công", Slug = "huong-dan-thi-cong", Type = TagType.Article, CreatedAt = DateTime.UtcNow },
            new Tag { Id = 9, Name = "Kinh nghiệm chống thấm", Slug = "kinh-nghiem-chong-tham", Type = TagType.Article, CreatedAt = DateTime.UtcNow },
            new Tag { Id = 10, Name = "Bảo trì nhà cửa", Slug = "bao-tri-nha-cua", Type = TagType.Article, CreatedAt = DateTime.UtcNow }
        );
    }
}