using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class Brand : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? Website { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Product>? Products { get; set; }
}

public class BrandConfiguration : BaseEntityConfiguration<Brand, int>
{
    public override void Configure(EntityTypeBuilder<Brand> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).IsRequired().HasMaxLength(255);
        builder.HasIndex(e => e.Slug).IsUnique();

        builder.Property(e => e.Description).HasColumnType("nvarchar(max)");
        builder.Property(e => e.LogoUrl).HasMaxLength(2048);
        builder.Property(e => e.Website).HasMaxLength(255);
        builder.Property(e => e.IsActive).HasDefaultValue(true);

        builder.HasData(
            new Brand { Id = 1, Name = "Dulux", Slug = "dulux", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Brand { Id = 2, Name = "Jotun", Slug = "jotun", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Brand { Id = 3, Name = "Kova", Slug = "kova", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Brand { Id = 4, Name = "Sika", Slug = "sika", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Brand { Id = 5, Name = "Nippon Paint", Slug = "nippon-paint", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Brand { Id = 6, Name = "My Kolor", Slug = "my-kolor", IsActive = true, CreatedAt = DateTime.UtcNow }
        );
    }
}