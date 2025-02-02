using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Entities;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}

public abstract class BaseEntity<TKey> : BaseEntity
{
    public TKey Id { get; set; } = default!;
}

public abstract class SeoEntity<TKey> : BaseEntity<TKey>
{
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string CanonicalUrl { get; set; } = string.Empty;
    public string OgTitle { get; set; } = string.Empty;
    public string OgDescription { get; set; } = string.Empty;
    public string OgImage { get; set; } = string.Empty;
    public JsonDocument? StructuredData { get; set; }
}

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at").IsRequired(false);

        builder.HasQueryFilter(e => e.DeletedAt == null);
    }
}

public abstract class BaseEntityConfiguration<T, TKey> : BaseEntityConfiguration<T> where T : BaseEntity<TKey>
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        if (typeof(TKey) == typeof(int)) builder.Property(e => e.Id).UseIdentityAlwaysColumn();
    }
}

public abstract class SeoEntityConfiguration<TEntity, TKey> : BaseEntityConfiguration<TEntity, TKey>
    where TEntity : SeoEntity<TKey>
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.MetaTitle)
            .HasColumnName("meta_title")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(e => e.MetaDescription)
            .HasColumnName("meta_description")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.CanonicalUrl)
            .HasColumnName("canonical_url")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.OgTitle)
            .HasColumnName("og_title")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(e => e.OgDescription)
            .HasColumnName("og_description")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.OgImage)
            .HasColumnName("og_image")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.StructuredData)
            .HasColumnName("structured_data")
            .HasColumnType("jsonb")
            .IsRequired(false)
            .HasConversion(
                v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => v == null ? null : JsonSerializer.Deserialize<JsonDocument>(v, (JsonSerializerOptions)null!)
            );
    }
}