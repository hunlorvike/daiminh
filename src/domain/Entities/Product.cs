using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Product : SeoEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Manufacturer { get; set; }
    public string? Origin { get; set; }
    public string? Specifications { get; set; }
    public string? Usage { get; set; }
    public int ViewCount { get; set; } = 0;
    public bool IsFeatured { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public PublishStatus Status { get; set; } = PublishStatus.Draft;
    public int? BrandId { get; set; }
    public int? CategoryId { get; set; }
    public virtual Brand? Brand { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<ProductImage>? Images { get; set; }
    public virtual ICollection<ProductTag>? ProductTags { get; set; }
    public virtual ICollection<ArticleProduct>? ArticleProducts { get; set; }
}

public class ProductConfiguration : SeoEntityConfiguration<Product, int>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.BrandId);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).IsRequired().HasMaxLength(255);
        builder.HasIndex(e => e.Slug).IsUnique();

        builder.Property(e => e.Description).HasColumnType("TEXT").IsRequired();
        builder.Property(e => e.ShortDescription).HasMaxLength(500);
        builder.Property(e => e.Manufacturer).HasMaxLength(255);
        builder.Property(e => e.Origin).HasMaxLength(100);
        builder.Property(e => e.Specifications).HasColumnType("TEXT");
        builder.Property(e => e.Usage).HasColumnType("TEXT");
        builder.Property(e => e.ViewCount).HasDefaultValue(0);
        builder.Property(e => e.IsFeatured).HasDefaultValue(false);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.CategoryId);
        builder.Property(e => e.Status)
            .IsRequired()
            .HasDefaultValue(PublishStatus.Draft);

        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}