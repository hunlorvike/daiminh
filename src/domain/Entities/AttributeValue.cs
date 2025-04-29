using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;
public class AttributeValue : BaseEntity<int>
{
    public int AttributeId { get; set; }
    public string Value { get; set; } = string.Empty; // VD: Đỏ, 5L, Bóng mờ
    public string Slug { get; set; } = string.Empty; // VD: red, 5-litre, semi-gloss
    public virtual Attribute? Attribute { get; set; }
    public virtual ICollection<ProductVariationAttributeValue>? ProductVariationAttributeValues { get; set; }
}

public class AttributeValueConfiguration : BaseEntityConfiguration<AttributeValue, int>
{
    public override void Configure(EntityTypeBuilder<AttributeValue> builder)
    {
        base.Configure(builder);
        builder.ToTable("attribute_values");

        builder.Property(v => v.AttributeId).HasColumnName("attribute_id").IsRequired();
        builder.Property(v => v.Value).HasColumnName("value").HasMaxLength(100).IsRequired();
        builder.Property(v => v.Slug).HasColumnName("slug").HasMaxLength(120).IsRequired();
        builder.HasIndex(v => new { v.AttributeId, v.Slug }).IsUnique();

        builder.HasOne(v => v.Attribute)
               .WithMany(a => a.Values)
               .HasForeignKey(v => v.AttributeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
            new AttributeValue { Id = 1, AttributeId = 1, Value = "Trắng", Slug = "trang", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 2, AttributeId = 1, Value = "Đỏ", Slug = "do", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 3, AttributeId = 1, Value = "Xanh dương", Slug = "xanh-duong", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 4, AttributeId = 1, Value = "Vàng", Slug = "vang", CreatedAt = DateTime.UtcNow },

            new AttributeValue { Id = 5, AttributeId = 2, Value = "1 Lít", Slug = "1-lit", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 6, AttributeId = 2, Value = "5 Lít", Slug = "5-lit", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 7, AttributeId = 2, Value = "18 Lít", Slug = "18-lit", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 8, AttributeId = 2, Value = "20 Kg", Slug = "20-kg", CreatedAt = DateTime.UtcNow },

            new AttributeValue { Id = 9, AttributeId = 3, Value = "Bóng", Slug = "bong", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 10, AttributeId = 3, Value = "Mờ", Slug = "mo", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 11, AttributeId = 3, Value = "Bán bóng", Slug = "ban-bong", CreatedAt = DateTime.UtcNow },

            new AttributeValue { Id = 12, AttributeId = 4, Value = "Tường nội thất", Slug = "tuong-noi-that", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 13, AttributeId = 4, Value = "Tường ngoại thất", Slug = "tuong-ngoai-that", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 14, AttributeId = 4, Value = "Sàn bê tông", Slug = "san-be-tong", CreatedAt = DateTime.UtcNow },
            new AttributeValue { Id = 15, AttributeId = 4, Value = "Sân thượng", Slug = "san-thuong", CreatedAt = DateTime.UtcNow }
        );
    }
}