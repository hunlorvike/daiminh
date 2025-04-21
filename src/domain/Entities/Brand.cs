using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class Brand : SeoEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? Website { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Product>? Products { get; set; }
}

public class BrandConfiguration : SeoEntityConfiguration<Brand, int>
{
    public override void Configure(EntityTypeBuilder<Brand> builder)
    {
        base.Configure(builder);

        builder.ToTable("brands");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description").HasColumnType("nvarchar(max)");
        builder.Property(e => e.LogoUrl).HasColumnName("logo_url").HasMaxLength(2048);
        builder.Property(e => e.Website).HasColumnName("website").HasMaxLength(255);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_brands_slug").IsUnique();
        builder.HasIndex(e => e.IsActive).HasDatabaseName("idx_brands_is_active");
    }
}