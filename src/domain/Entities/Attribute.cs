using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;
public class Attribute : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty; // VD: Màu sắc, Dung tích
    public string Slug { get; set; } = string.Empty; // VD: color, volume
    public virtual ICollection<AttributeValue>? Values { get; set; }
    public virtual ICollection<ProductAttribute>? ProductAttributes { get; set; }
}

public class AttributeConfiguration : BaseEntityConfiguration<Attribute, int>
{
    public override void Configure(EntityTypeBuilder<Attribute> builder)
    {
        base.Configure(builder);
        builder.Property(a => a.Name).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Slug).HasMaxLength(120).IsRequired();
        builder.HasIndex(a => a.Slug).IsUnique();

        builder.HasData(
            new Attribute { Id = 1, Name = "Màu sắc", Slug = "mau-sac", CreatedAt = DateTime.UtcNow },
            new Attribute { Id = 2, Name = "Dung tích", Slug = "dung-tich", CreatedAt = DateTime.UtcNow },
            new Attribute { Id = 3, Name = "Độ bóng", Slug = "do-bong", CreatedAt = DateTime.UtcNow },
            new Attribute { Id = 4, Name = "Bề mặt áp dụng", Slug = "be-mat-ap-dung", CreatedAt = DateTime.UtcNow }
        );
    }
}