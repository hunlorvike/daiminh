using core.Entities.Marketing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Configuration.Marketing;

public class SeoMetadataConfiguration : IEntityTypeConfiguration<SeoMetadata>
{
    public void Configure(EntityTypeBuilder<SeoMetadata> builder)
    {
        builder.ToTable("SeoMetadata");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EntityType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.TwitterTitle)
            .HasMaxLength(200);

        builder.Property(x => x.TwitterDescription)
            .HasMaxLength(500);

        builder.Property(x => x.TwitterImage)
            .HasMaxLength(500);

        builder.Property(x => x.OgTitle)
            .HasMaxLength(200);

        builder.Property(x => x.OgDescription)
            .HasMaxLength(500);

        builder.Property(x => x.OgImage)
            .HasMaxLength(500);

        builder.Property(x => x.CanonicalUrl)
            .HasMaxLength(500);

        builder.Property(x => x.Robots)
            .HasMaxLength(50);

        builder.Property(x => x.Priority)
            .HasPrecision(3, 2)
            .HasDefaultValue(0.5m);

        builder.HasIndex(x => x.EntityId)
            .HasDatabaseName("IX_SeoMetadata_EntityId");

        builder.HasIndex(x => x.EntityType)
            .HasDatabaseName("IX_SeoMetadata_EntityType");
    }
}