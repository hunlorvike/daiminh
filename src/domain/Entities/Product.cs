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
    public string? Origin { get; set; } // Xuất xứ
    public string? Specifications { get; set; } // Thông số kỹ thuật
    public string? Usage { get; set; } // Hướng dẫn sử dụng
    public string? Features { get; set; } // Tính năng nổi bật
    public string? PackagingInfo { get; set; } // Thông tin đóng gói
    public string? StorageInstructions { get; set; } // Hướng dẫn bảo quản
    public string? SafetyInfo { get; set; } // Thông tin an toàn
    public string? ApplicationAreas { get; set; } // Khu vực ứng dụng
    public string? TechnicalDocuments { get; set; } // Tài liệu kỹ thuật (JSON)
    public int ViewCount { get; set; } = 0;
    public bool IsFeatured { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public PublishStatus Status { get; set; } = PublishStatus.Draft;
    public int? BrandId { get; set; } // Nullable Foreign Key
    public int? CategoryId { get; set; }

    // Navigation properties
    public virtual Brand? Brand { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<ProductImage>? Images { get; set; }
    public virtual ICollection<ProductTag>? ProductTags { get; set; }
    public virtual ICollection<ProjectProduct>? ProjectProducts { get; set; }
    public virtual ICollection<ArticleProduct>? ArticleProducts { get; set; }
    public virtual ICollection<ProductVariant>? Variants { get; set; }
}

public class ProductConfiguration : SeoEntityConfiguration<Product, int>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable("products");

        builder.Property(e => e.BrandId).HasColumnName("brand_id");
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
        builder.Property(e => e.ShortDescription).HasColumnName("short_description").HasMaxLength(500);
        builder.Property(e => e.Manufacturer).HasColumnName("manufacturer").HasMaxLength(255);
        builder.Property(e => e.Origin).HasColumnName("origin").HasMaxLength(100);
        builder.Property(e => e.Specifications).HasColumnName("specifications").HasColumnType("text");
        builder.Property(e => e.Usage).HasColumnName("usage").HasColumnType("text");
        builder.Property(e => e.Features).HasColumnName("features").HasColumnType("text");
        builder.Property(e => e.PackagingInfo).HasColumnName("packaging_info").HasColumnType("text");
        builder.Property(e => e.StorageInstructions).HasColumnName("storage_instructions").HasColumnType("text");
        builder.Property(e => e.SafetyInfo).HasColumnName("safety_info").HasColumnType("text");
        builder.Property(e => e.ApplicationAreas).HasColumnName("application_areas").HasColumnType("text");
        builder.Property(e => e.TechnicalDocuments).HasColumnName("technical_documents").HasColumnType("json");
        builder.Property(e => e.ViewCount).HasColumnName("view_count").HasDefaultValue(0);
        builder.Property(e => e.IsFeatured).HasColumnName("is_featured").HasDefaultValue(false);
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<PublishStatus>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(PublishStatus.Draft);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_products_slug").IsUnique();
        builder.HasIndex(e => e.Status).HasDatabaseName("idx_products_status");
        builder.HasIndex(e => e.ViewCount).HasDatabaseName("idx_products_view_count");
        builder.HasIndex(e => e.IsFeatured).HasDatabaseName("idx_products_is_featured");
        builder.HasIndex(e => e.BrandId).HasDatabaseName("idx_products_brand_id");

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

