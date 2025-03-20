using domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
using shared.Models;

namespace domain.Entities;

public class Tag : BaseEntity<int>
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public EntityType EntityType { get; set; } = EntityType.Product;

    // Navigation properties
    public virtual ICollection<ContentTag>? ContentTags { get; set; }
    public virtual ICollection<ProductTag>? ProductTags { get; set; }
}

public class TagConfiguration : BaseEntityConfiguration<Tag, int>
{
    public override void Configure(EntityTypeBuilder<Tag> builder)
    {
        base.Configure(builder);
        builder.ToTable("tags");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(50);
        builder.Property(x => x.EntityType).HasColumnName("entity_type").IsRequired().HasDefaultValue(EntityType.Product);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_tags_slug");

        builder.HasData(new TagSeeder().DataSeeder());
    }
}

public class TagSeeder : ISeeder<Tag>
{
    public IEnumerable<Tag> DataSeeder()
    {
        return
        [
            // Các tag liên quan đến sản phẩm (sơn)
            new Tag{ Id = 1, Name = "Chống thấm", Slug = "chong-tham", EntityType = EntityType.Product },
            new Tag{ Id = 2, Name = "Bền màu", Slug = "ben-mau", EntityType = EntityType.Product },
            new Tag{ Id = 3, Name = "Dễ lau chùi", Slug = "de-lau-chui", EntityType = EntityType.Product },
            new Tag{ Id = 4, Name = "Kháng khuẩn", Slug = "khang-khuan", EntityType = EntityType.Product },
            new Tag{ Id = 5, Name = "Chống nấm mốc", Slug = "chong-nam-moc", EntityType = EntityType.Product },
            new Tag{ Id = 6, Name = "Giá rẻ", Slug = "gia-re", EntityType = EntityType.Product },
            new Tag{ Id = 7, Name = "Chất lượng", Slug = "chat-luong", EntityType = EntityType.Product },
            new Tag{ Id = 8, Name = "Cao cấp", Slug = "cao-cap", EntityType = EntityType.Product },
            new Tag{ Id = 9, Name = "Màu sắc bền đẹp", Slug = "mau-sac-ben-dep", EntityType = EntityType.Product },
            new Tag{ Id = 10, Name = "Dễ thi công", Slug = "de-thi-cong", EntityType = EntityType.Product },
            new Tag{ Id = 11, Name = "An toàn", Slug = "an-toan", EntityType = EntityType.Product },
            new Tag{ Id = 12, Name = "Thương hiệu nổi tiếng", Slug = "thuong-hieu-noi-tieng", EntityType = EntityType.Product },

            // Các tag liên quan đến dịch vụ (thi công, tư vấn)
            new Tag{ Id = 101, Name = "Thi công nhanh chóng", Slug = "thi-cong-nhanh-chong", EntityType = EntityType.Service },
            new Tag{ Id = 102, Name = "Thợ sơn tay nghề cao", Slug = "tho-son-tay-nghe-cao", EntityType = EntityType.Service },
            new Tag{ Id = 103, Name = "Bảo hành dài hạn", Slug = "bao-hanh-dai-han", EntityType = EntityType.Service },
            new Tag{ Id = 104, Name = "Tư vấn màu sắc miễn phí", Slug = "tu-van-mau-sac-mien-phi", EntityType = EntityType.Service },
            new Tag{ Id = 105, Name = "Báo giá cạnh tranh", Slug = "bao-gia-canh-tranh", EntityType = EntityType.Service },
            new Tag{ Id = 106, Name = "Thi công sơn nội thất", Slug = "thi-cong-son-noi-that", EntityType = EntityType.Service },
            new Tag{ Id = 107, Name = "Thi công sơn ngoại thất", Slug = "thi-cong-son-ngoai-that", EntityType = EntityType.Service },
            new Tag{ Id = 108, Name = "Tư vấn kỹ thuật chuyên nghiệp", Slug = "tu-van-ky-thuat-chuyen-nghiep", EntityType = EntityType.Service },
            new Tag{ Id = 109, Name = "Thi công sơn trọn gói", Slug = "thi-cong-son-tron-goi", EntityType = EntityType.Service },
            new Tag{ Id = 110, Name = "Tư vấn lựa chọn sơn", Slug = "tu-van-lua-chon-son", EntityType = EntityType.Service },
            new Tag{ Id = 111, Name = "Đảm bảo chất lượng thi công", Slug = "dam-bao-chat-luong-thi-cong", EntityType = EntityType.Service },
            new Tag{ Id = 112, Name = "Hỗ trợ tận tâm", Slug = "ho-tro-tan-tam", EntityType = EntityType.Service },        ];
    }
}