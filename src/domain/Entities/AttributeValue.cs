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

        builder.HasOne(v => v.Attribute)
               .WithMany(a => a.Values)
               .HasForeignKey(v => v.AttributeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
