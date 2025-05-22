using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;
public class Attribute : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
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
    }
}