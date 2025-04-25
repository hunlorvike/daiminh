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
        builder.ToTable("attributes");
        builder.Property(a => a.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(a => a.Slug).HasColumnName("slug").HasMaxLength(120).IsRequired();
    }
}

