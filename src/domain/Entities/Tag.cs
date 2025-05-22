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
        builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Slug).IsRequired().HasMaxLength(50);
        builder.HasIndex(e => e.Slug).IsUnique();
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.Type)
            .IsRequired()
            .HasDefaultValue(TagType.Product);
    }
}