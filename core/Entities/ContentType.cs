using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public class ContentType : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<ContentFieldDefinition> FieldDefinitions { get; set; }
    public ICollection<Content> Contents { get; set; }
}

public class ContentTypeConfiguration : BaseEntityConfiguration<ContentType, int>
{
    public override void Configure(EntityTypeBuilder<ContentType> builder)
    {
        base.Configure(builder);

        builder.ToTable("content_types");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Description).HasColumnName("description");

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_content_types_slug");
    }
}