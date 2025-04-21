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

        builder.ToTable("tags");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(e => e.Type)
            .HasColumnName("type")
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(TagType.Product);

        builder.HasIndex(e => new { e.Slug, e.Type }).HasDatabaseName("idx_tags_slug_type").IsUnique();
        builder.HasIndex(e => e.Type).HasDatabaseName("idx_tags_type");
    }
}
