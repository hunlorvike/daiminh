using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ProductVariant : BaseEntity<int>
{
    public int ProductId { get; set; } // Foreign Key to Product
    public string Name { get; set; } = string.Empty; // e.g., "Sơn Chống Thấm Màu Xám - 5L", "Bột Trét Cao Cấp - 20kg"
    public string Sku { get; set; } = string.Empty; // Stock Keeping Unit - Mã định danh kho duy nhất
    public decimal Price { get; set; } // Giá của biến thể này
    public int StockQuantity { get; set; } = 0; // Số lượng tồn kho
    public string? Color { get; set; } // Màu sắc (nếu áp dụng)
    public string? Size { get; set; } // Kích thước/Dung tích (e.g., "5L", "10kg", "18L")
    public string? Packaging { get; set; } // Quy cách đóng gói (e.g., "Lon", "Bao", "Thùng")
    public string? ImageUrl { get; set; } // URL hình ảnh đại diện cho biến thể (nếu khác sản phẩm gốc)
    public bool IsActive { get; set; } = true; // Trạng thái hoạt động

    public virtual Product? Product { get; set; }
}

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("product_variants");

        builder.Property(v => v.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(v => v.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(v => v.Sku).HasColumnName("sku").IsRequired().HasMaxLength(100);
        builder.Property(v => v.Price).HasColumnName("price").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(v => v.StockQuantity).HasColumnName("stock_quantity").HasDefaultValue(0);
        builder.Property(v => v.Color).HasColumnName("color").HasMaxLength(50);
        builder.Property(v => v.Size).HasColumnName("size").HasMaxLength(50);
        builder.Property(v => v.Packaging).HasColumnName("packaging").HasMaxLength(50);
        builder.Property(v => v.ImageUrl).HasColumnName("image_url").HasMaxLength(2048);
        builder.Property(v => v.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        builder.HasIndex(v => v.Sku).HasDatabaseName("idx_product_variants_sku").IsUnique();
        builder.HasIndex(v => v.ProductId).HasDatabaseName("idx_product_variants_product_id");
        builder.HasIndex(v => v.IsActive).HasDatabaseName("idx_product_variants_is_active");

        builder.HasOne(v => v.Product)
               .WithMany(p => p.Variants)
               .HasForeignKey(v => v.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
} 