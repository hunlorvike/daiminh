using domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ContentType : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<ContentFieldDefinition>? FieldDefinitions { get; set; }

    public virtual ICollection<Content>? Contents { get; set; }
}

public class ContentTypeConfiguration : BaseEntityConfiguration<ContentType, int>
{
    public override void Configure(EntityTypeBuilder<ContentType> builder)
    {
        base.Configure(builder);

        builder.ToTable("content_types");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(50);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_content_types_slug");

        builder.HasData(new ContentTypeSeeder().DataSeeder());
    }
}

public class ContentTypeSeeder : ISeeder<ContentType>
{
    public IEnumerable<ContentType> DataSeeder()
    {
        return
        new List<ContentType>
        {
            new ContentType { Id = 1, Name = "Bài viết", Slug = "bai-viet" }, // Dùng cho các bài blog, tin tức
            new ContentType { Id = 2, Name = "Trang tĩnh", Slug = "trang-tinh" }, // Dùng cho các trang giới thiệu, liên hệ, chính sách
            new ContentType { Id = 3, Name = "Dịch vụ", Slug = "dich-vu" }, // Dùng để quản lý nội dung giới thiệu các dịch vụ
            new ContentType { Id = 4, Name = "Tư vấn", Slug = "tu-van" }, // Dùng để quản lý nội dung giới thiệu các gói tư vấn
            new ContentType { Id = 5, Name = "Chính sách", Slug = "chinh-sach" } // Dùng để quản lý nội dung giới thiệu các sản phẩm
        };
    }
}