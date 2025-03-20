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
    }
}