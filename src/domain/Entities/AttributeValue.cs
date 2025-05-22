using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;
public class AttributeValue : BaseEntity<int>
{
    public int AttributeId { get; set; }
    public string Value { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public virtual Attribute? Attribute { get; set; }
    public virtual ICollection<ProductVariationAttributeValue>? ProductVariationAttributeValues { get; set; }
}

public class AttributeValueConfiguration : BaseEntityConfiguration<AttributeValue, int>
{
    public override void Configure(EntityTypeBuilder<AttributeValue> builder)
    {
        base.Configure(builder);
        builder.Property(v => v.AttributeId).IsRequired();
        builder.Property(v => v.Value).HasMaxLength(100).IsRequired();
        builder.Property(v => v.Slug).HasMaxLength(120).IsRequired();
        builder.HasIndex(v => new { v.AttributeId, v.Slug }).IsUnique();

        builder.HasOne(v => v.Attribute)
               .WithMany(a => a.Values)
               .HasForeignKey(v => v.AttributeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}