using domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ProductType : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<ProductFieldDefinition>? FieldDefinitions { get; set; }
    public virtual ICollection<Product>? Products { get; set; }
}

public class ProductTypeConfiguration : BaseEntityConfiguration<ProductType, int>
{
    public override void Configure(EntityTypeBuilder<ProductType> builder)
    {
        base.Configure(builder);

        builder.ToTable("product_types");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(50);

        builder.HasIndex(e => e.Name).HasDatabaseName("idx_product_types_name");
        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_product_types_slug");

        builder.HasData(new ProductTypeSeeder().DataSeeder());
    }
}

public class ProductTypeSeeder : ISeeder<ProductType>
{
    public IEnumerable<ProductType> DataSeeder()
    {
        return
        [
            new ProductType { Id = 1, Name = "Sơn Nước", Slug = "son-nuoc" },
            new ProductType { Id = 2, Name = "Sơn Dầu", Slug = "son-dau" },
            new ProductType { Id = 3, Name = "Sơn Acrylic", Slug = "son-acrylic" },
            new ProductType { Id = 4, Name = "Sơn Epoxy", Slug = "son-epoxy" },
            new ProductType { Id = 5, Name = "Sơn Alkyd", Slug = "son-alkyd" },
            new ProductType { Id = 6, Name = "Sơn Lót", Slug = "son-lot" },
            new ProductType { Id = 7, Name = "Sơn Chống Thấm", Slug = "son-chong-tham" },
            new ProductType { Id = 8, Name = "Sơn Gỗ", Slug = "son-go" },
            new ProductType { Id = 9, Name = "Sơn Kim Loại", Slug = "son-kim-loai" }
        ];
    }
}