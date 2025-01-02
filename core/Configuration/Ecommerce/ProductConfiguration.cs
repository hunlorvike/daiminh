using core.Entities.CMS;
using core.Entities.Ecommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace core.Configuration.Ecommerce;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Slug)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.SKU)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Discount)
            .HasPrecision(18, 2);

        builder.Property(x => x.MetaTitle)
            .HasMaxLength(200);

        builder.Property(x => x.MetaDescription)
            .HasMaxLength(500);

        builder.Property(x => x.MetaKeywords)
            .HasMaxLength(200);

        builder.HasOne(x => x.Manufacturer)
            .WithMany()
            .HasForeignKey(x => x.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Tags)
            .WithMany(x => x.Products)
            .UsingEntity<Dictionary<string, object>>(
                "ProductTags",
                j => j
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId"),
                j => j
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
            );

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Products_Name");

        builder.HasIndex(x => x.Slug)
            .HasDatabaseName("IX_Products_Slug");

        builder.HasIndex(x => x.CategoryId)
            .HasDatabaseName("IX_Products_CategoryId");

        builder.HasIndex(x => x.ManufacturerId)
            .HasDatabaseName("IX_Products_ManufacturerId");
    }
}