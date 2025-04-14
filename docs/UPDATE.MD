Okay, this is a complex one involving multiple relationships (One-to-Many for Images/Variants, Many-to-Many for Categories/Tags) and nested view models. Let's refactor the Product management feature according to the established standards.

**1. Entities & Configurations**

*   **`ProductVariantConfiguration`:** Needs to inherit from `BaseEntityConfiguration`.
*   **`ProductCategoryConfiguration` / `ProductTagConfiguration`:** Should *not* inherit from `BaseEntityConfiguration` as they are simple join tables without audit fields. Define keys and relationships directly.

```csharp
// --- START OF FILE ProductImage.cs --- No Changes Needed ---
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace domain.Entities;
public class ProductImage : BaseEntity<int> { /* ... as before ... */ }
// --- END OF FILE ProductImage.cs ---

// --- START OF FILE Product.cs --- No Changes Needed ---
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
namespace domain.Entities;
public class Product : SeoEntity<int> { /* ... as before ... */ }
// --- END OF FILE Product.cs ---

// --- START OF FILE ProductCategory.cs --- Updated ---
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace domain.Entities;
// Join table - Does NOT typically need BaseEntity
public class ProductCategory
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public virtual Product? Product { get; set; }
    public virtual Category? Category { get; set; }
}
// --- END OF FILE ProductCategory.cs ---

// --- START OF FILE ProductTag.cs --- Updated ---
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace domain.Entities;
// Join table - Does NOT typically need BaseEntity
public class ProductTag
{
    public int ProductId { get; set; }
    public int TagId { get; set; }
    public virtual Product? Product { get; set; }
    public virtual Tag? Tag { get; set; }
}
// --- END OF FILE ProductTag.cs ---

// --- START OF FILE ProductVariant.cs --- No Changes Needed ---
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace domain.Entities;
public class ProductVariant : BaseEntity<int> { /* ... as before ... */ }
// --- END OF FILE ProductVariant.cs ---

// --- START OF FILE ProductType.cs --- No Changes Needed ---
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace domain.Entities;
public class ProductType : BaseEntity<int> { /* ... as before ... */ }
// --- END OF FILE ProductType.cs ---


// --- CONFIGURATIONS ---

// --- START OF FILE ProductImageConfiguration.cs --- No Changes Needed ---
using domain.Entities;
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace infrastructure;
public class ProductImageConfiguration : BaseEntityConfiguration<ProductImage, int> { /* ... as before ... */ }
// --- END OF FILE ProductImageConfiguration.cs ---

// --- START OF FILE ProductConfiguration.cs --- No Changes Needed ---
using domain.Entities;
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
namespace infrastructure;
public class ProductConfiguration : SeoEntityConfiguration<Product, int> { /* ... as before ... */ }
// --- END OF FILE ProductConfiguration.cs ---

// --- START OF FILE ProductCategoryConfiguration.cs --- Updated ---
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace infrastructure;
// Configure the JOIN TABLE directly
public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("product_categories");

        // Composite Primary Key
        builder.HasKey(pc => new { pc.ProductId, pc.CategoryId });

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        // Define Relationships
        builder.HasOne(e => e.Product)
            .WithMany(p => p.ProductCategories) // Navigation property on Product
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting Product deletes join entry

        builder.HasOne(e => e.Category)
            .WithMany(c => c.ProductCategories) // Navigation property on Category
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting Category deletes join entry
    }
}
// --- END OF FILE ProductCategoryConfiguration.cs ---

// --- START OF FILE ProductTagConfiguration.cs --- Updated ---
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace infrastructure;
// Configure the JOIN TABLE directly
public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.ToTable("product_tags");

        // Composite Primary Key
        builder.HasKey(pt => new { pt.ProductId, pt.TagId });

        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.TagId).HasColumnName("tag_id");

        // Define Relationships
        builder.HasOne(e => e.Product)
            .WithMany(p => p.ProductTags) // Navigation property on Product
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting Product deletes join entry

        builder.HasOne(e => e.Tag)
            .WithMany(t => t.ProductTags) // Navigation property on Tag
            .HasForeignKey(e => e.TagId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting Tag deletes join entry
    }
}
// --- END OF FILE ProductTagConfiguration.cs ---

// --- START OF FILE ProductVariantConfiguration.cs --- Updated ---
using domain.Entities;
using domain.Entities.Shared; // Required
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace infrastructure;
// Inherit from BaseEntityConfiguration
public class ProductVariantConfiguration : BaseEntityConfiguration<ProductVariant, int>
{
    public override void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        base.Configure(builder); // Configure BaseEntity fields

        builder.ToTable("product_variants");

        builder.Property(v => v.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(v => v.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(v => v.Sku).HasColumnName("sku").IsRequired().HasMaxLength(100);
        builder.Property(v => v.Price).HasColumnName("price").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(v => v.StockQuantity).HasColumnName("stock_quantity").HasDefaultValue(0);
        builder.Property(v => v.Color).HasColumnName("color").HasMaxLength(50);
        builder.Property(v => v.Size).HasColumnName("size").HasMaxLength(50);
        builder.Property(v => v.Packaging).HasColumnName("packaging").HasMaxLength(50);
        builder.Property(v => v.ImageUrl).HasColumnName("image_url").HasMaxLength(2048); // Store MinIO Path
        builder.Property(v => v.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        // Ensure SKU is unique across ALL variants
        builder.HasIndex(v => v.Sku).HasDatabaseName("idx_product_variants_sku").IsUnique();
        builder.HasIndex(v => v.ProductId).HasDatabaseName("idx_product_variants_product_id");
        builder.HasIndex(v => v.IsActive).HasDatabaseName("idx_product_variants_is_active");

        builder.HasOne(v => v.Product)
               .WithMany(p => p.Variants)
               .HasForeignKey(v => v.ProductId)
               .OnDelete(DeleteBehavior.Cascade); // Deleting Product deletes its Variants
    }
}
// --- END OF FILE ProductVariantConfiguration.cs ---

// --- START OF FILE ProductTypeConfiguration.cs --- No Changes Needed ---
using domain.Entities;
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace infrastructure;
public class ProductTypeConfiguration : BaseEntityConfiguration<ProductType, int> { /* ... as before ... */ }
// --- END OF FILE ProductTypeConfiguration.cs ---

// --- IMPORTANT ---
// Add DbSets for join tables to ApplicationDbContext:
// public DbSet<ProductCategory> ProductCategories { get; set; }
// public DbSet<ProductTag> ProductTags { get; set; }
// Apply new configurations in OnModelCreating:
// builder.ApplyConfiguration(new ProductCategoryConfiguration());
// builder.ApplyConfiguration(new ProductTagConfiguration());
// --- End Important ---
```

**2. ViewModels**

*   Add `[Display]` attributes. Add `UpdatedAt` to `ListItemViewModel`.

```csharp
// --- START OF FILE ProductListItemViewModel.cs --- Updated ---
using shared.Enums;
namespace web.Areas.Admin.ViewModels.Product;
public class ProductListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? MainImageUrl { get; set; } // MinIO Path
    public string? ProductTypeName { get; set; }
    public string? BrandName { get; set; }
    public PublishStatus Status { get; set; }
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public int ViewCount { get; set; }
    public DateTime? UpdatedAt { get; set; } // Changed to nullable
}
// --- END OF FILE ProductListItemViewModel.cs ---

// --- START OF FILE ProductViewModel.cs --- Updated ---
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;
using web.Areas.Admin.ViewModels.Shared; // For SEO Interface

namespace web.Areas.Admin.ViewModels.Product;
// Implement SEO Interface
public class ProductViewModel : ISeoPropertiesViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tên sản phẩm", Prompt = "Nhập tên sản phẩm")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug", Prompt = "ten-san-pham")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả chi tiết", Prompt = "Nhập mô tả đầy đủ cho sản phẩm")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Description { get; set; } = string.Empty; // Rich Text Editor

    [Display(Name = "Mô tả ngắn", Prompt = "Nhập mô tả ngắn gọn (tối đa 500 ký tự)")]
    [MaxLength(500)]
    public string? ShortDescription { get; set; }

    [Display(Name = "Nhà sản xuất", Prompt = "Tên nhà sản xuất")]
    [MaxLength(255)]
    public string? Manufacturer { get; set; }

    [Display(Name = "Xuất xứ", Prompt = "Ví dụ: Việt Nam, Mỹ, Đức")]
    [MaxLength(100)]
    public string? Origin { get; set; }

    [Display(Name = "Thông số kỹ thuật", Prompt = "Nhập thông số dưới dạng văn bản hoặc HTML")]
    public string? Specifications { get; set; } // Text Area or Rich Text

    [Display(Name = "Hướng dẫn sử dụng", Prompt = "Nhập hướng dẫn sử dụng")]
    public string? Usage { get; set; } // Text Area or Rich Text

    [Display(Name = "Tính năng nổi bật", Prompt = "Liệt kê các tính năng chính")]
    public string? Features { get; set; } // Text Area or Rich Text

    [Display(Name = "Thông tin đóng gói", Prompt = "Ví dụ: Thùng 20kg, Lon 5L")]
    public string? PackagingInfo { get; set; } // Text Area

    [Display(Name = "Hướng dẫn bảo quản", Prompt = "Cách bảo quản sản phẩm")]
    public string? StorageInstructions { get; set; } // Text Area

    [Display(Name = "Thông tin an toàn", Prompt = "Lưu ý về an toàn khi sử dụng")]
    public string? SafetyInfo { get; set; } // Text Area

    [Display(Name = "Khu vực ứng dụng", Prompt = "Các lĩnh vực hoặc nơi sản phẩm được sử dụng")]
    public string? ApplicationAreas { get; set; } // Text Area

    [Display(Name = "Tài liệu kỹ thuật (JSON)", Prompt = "[{\"name\": \"TDS\", \"url\": \"...\"}]")]
    public string? TechnicalDocuments { get; set; } // JSON - Text Area

    [Display(Name = "Nổi bật")]
    public bool IsFeatured { get; set; } = false;

    [Display(Name = "Hiển thị")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Loại sản phẩm")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public int ProductTypeId { get; set; }

    [Display(Name = "Trạng thái xuất bản")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    [Display(Name = "Thương hiệu")]
    public int? BrandId { get; set; } // Nullable

    // --- Relationships ---
    [Display(Name = "Danh mục")]
    [Required(ErrorMessage = "Vui lòng chọn ít nhất một {0}.")] // Assuming categories are required
    public List<int> SelectedCategoryIds { get; set; } = new List<int>();

    [Display(Name = "Thẻ (Tags)")]
    public List<int> SelectedTagIds { get; set; } = new List<int>();

    [Display(Name = "Hình ảnh")]
    public List<ProductImageViewModel> Images { get; set; } = new List<ProductImageViewModel>();

    [Display(Name = "Biến thể & Kho hàng")]
    public List<ProductVariantViewModel> Variants { get; set; } = new List<ProductVariantViewModel>();

    // --- SEO Fields (from ISeoPropertiesViewModel) ---
    [Display(Name = "Meta Title", Prompt = "Tiêu đề SEO")] [MaxLength(100)] public string? MetaTitle { get; set; }
    [Display(Name = "Meta Description", Prompt = "Mô tả SEO")] [MaxLength(300)] public string? MetaDescription { get; set; }
    [Display(Name = "Meta Keywords", Prompt = "Từ khóa SEO")] [MaxLength(200)] public string? MetaKeywords { get; set; }
    [Display(Name = "Canonical URL")] [MaxLength(255)] [Url] public string? CanonicalUrl { get; set; }
    [Display(Name = "No Index")] public bool NoIndex { get; set; } = false;
    [Display(Name = "No Follow")] public bool NoFollow { get; set; } = false;
    [Display(Name = "Open Graph Title")] [MaxLength(100)] public string? OgTitle { get; set; }
    [Display(Name = "Open Graph Description")] [MaxLength(300)] public string? OgDescription { get; set; }
    [Display(Name = "Open Graph Image (URL)", Prompt = "URL ảnh cho Facebook, Zalo...")] [MaxLength(255)] [Url] public string? OgImage { get; set; } // Stores full URL
    [Display(Name = "Open Graph Type")] [MaxLength(50)] public string? OgType { get; set; } = "product";
    [Display(Name = "Twitter Title")] [MaxLength(100)] public string? TwitterTitle { get; set; }
    [Display(Name = "Twitter Description")] [MaxLength(300)] public string? TwitterDescription { get; set; }
    [Display(Name = "Twitter Image (URL)", Prompt = "URL ảnh cho Twitter")] [MaxLength(255)] [Url] public string? TwitterImage { get; set; } // Stores full URL
    [Display(Name = "Twitter Card Type")] [MaxLength(50)] public string? TwitterCard { get; set; } = "summary_large_image";
    [Display(Name = "Schema Markup (JSON-LD)", Prompt = "Dữ liệu cấu trúc Schema.org")] public string? SchemaMarkup { get; set; }
    [Display(Name = "Breadcrumb JSON", Prompt = "Dữ liệu Breadcrumb JSON (nếu cần)")] public string? BreadcrumbJson { get; set; }
    [Display(Name = "Sitemap Priority (0.0 - 1.0)")] [Range(0.0, 1.0)] public double SitemapPriority { get; set; } = 0.8;
    [Display(Name = "Sitemap Change Frequency")] [Required] public string SitemapChangeFrequency { get; set; } = "weekly";

    // --- Dropdown Data ---
    public SelectList? ProductTypeList { get; set; }
    public SelectList? BrandList { get; set; }
    public SelectList? CategoryList { get; set; } // For multi-select
    public SelectList? TagList { get; set; } // For multi-select
    public SelectList? StatusList { get; set; }
}
// --- END OF FILE ProductViewModel.cs ---

// --- START OF FILE ProductImageViewModel.cs --- Updated ---
using System.ComponentModel.DataAnnotations;
namespace web.Areas.Admin.ViewModels.Product;
public class ProductImageViewModel
{
    public int Id { get; set; } // 0 for new images

    [Display(Name = "Đường dẫn ảnh (MinIO)", Prompt = "Đường dẫn file ảnh trên MinIO")]
    [Required(ErrorMessage = "Đường dẫn ảnh không được để trống")]
    public string ImageUrl { get; set; } = string.Empty; // MinIO Path

    [Display(Name = "Đường dẫn ảnh thu nhỏ (MinIO)", Prompt = "Đường dẫn thumbnail (nếu có)")]
    public string? ThumbnailUrl { get; set; } // MinIO Path

    [Display(Name = "Văn bản thay thế (Alt Text)", Prompt = "Mô tả ngắn gọn cho ảnh (SEO)")]
    [MaxLength(255)]
    public string? AltText { get; set; }

    [Display(Name = "Tiêu đề ảnh (Title)", Prompt = "Tiêu đề hiển thị khi hover (không bắt buộc)")]
     [MaxLength(255)]
    public string? Title { get; set; }

    [Display(Name = "Thứ tự")]
    [Range(0, int.MaxValue)]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Ảnh chính")]
    public bool IsMain { get; set; } = false;

    // Not displayed, used internally for Edit logic
    public bool IsDeleted { get; set; } = false;
}
// --- END OF FILE ProductImageViewModel.cs ---

// --- START OF FILE ProductVariantViewModel.cs --- Updated ---
using System.ComponentModel.DataAnnotations;
namespace web.Areas.Admin.ViewModels.Product;
public class ProductVariantViewModel
{
    public int Id { get; set; } // 0 for new variants

    [Display(Name = "Tên biến thể", Prompt = "Ví dụ: Màu Xanh - 5L")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "SKU", Prompt = "Mã định danh kho duy nhất")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [Display(Name = "Giá bán", Prompt = "Nhập giá bán cho biến thể")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [Range(0, double.MaxValue, ErrorMessage = "{0} phải là số không âm")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; } = 0;

    [Display(Name = "Số lượng tồn kho", Prompt = "Nhập số lượng hiện có")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm")]
    public int StockQuantity { get; set; } = 0;

    [Display(Name = "Màu sắc", Prompt = "Ví dụ: Xanh dương, Đỏ")]
    [MaxLength(50)]
    public string? Color { get; set; }

    [Display(Name = "Kích thước/Dung tích", Prompt = "Ví dụ: 5L, 10kg, 18L")]
    [MaxLength(50)]
    public string? Size { get; set; }

    [Display(Name = "Đóng gói", Prompt = "Ví dụ: Lon, Bao, Thùng")]
    [MaxLength(50)]
    public string? Packaging { get; set; }

    [Display(Name = "Ảnh riêng (MinIO Path)", Prompt = "Đường dẫn ảnh riêng nếu khác sản phẩm chính")]
    [MaxLength(2048)]
    public string? ImageUrl { get; set; } // MinIO Path

    [Display(Name = "Hoạt động")]
    public bool IsActive { get; set; } = true;

    // Not displayed, used internally for Edit logic
    public bool IsDeleted { get; set; } = false;
}
// --- END OF FILE ProductVariantViewModel.cs ---
```

**3. Mappers**

*   Update `ProductMappingProfile` for `UpdatedAt`.
*   Other profiles seem okay.

```csharp
// --- START OF FILE ProductImageProfile.cs --- No Changes Needed ---
using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Product;
namespace web.Areas.Admin.Mappers;
public class ProductImageProfile : Profile { /* ... as before ... */ }
// --- END OF FILE ProductImageProfile.cs ---

// --- START OF FILE ProductMappingProfile.cs --- Updated ---
using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Product, ProductListItemViewModel>()
            .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType != null ? src.ProductType.Name : null))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => // Find main image path
                src.Images != null && src.Images.Any(i => i.IsMain)
                    ? src.Images.OrderBy(i => i.OrderIndex).First(i => i.IsMain).ImageUrl
                    : src.Images != null && src.Images.Any()
                        ? src.Images.OrderBy(i => i.OrderIndex).First().ImageUrl
                        : null
            ))
             .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt)); // Map UpdatedAt


        // Entity -> ViewModel (For Edit GET)
        CreateMap<Product, ProductViewModel>()
            .ForMember(dest => dest.SelectedCategoryIds, opt => opt.MapFrom(src => src.ProductCategories != null ? src.ProductCategories.Select(pc => pc.CategoryId).ToList() : new List<int>()))
            .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ProductTags != null ? src.ProductTags.Select(pt => pt.TagId).ToList() : new List<int>()))
            // Let other profiles handle nested Images and Variants lists
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
            // Ignore dropdown lists - populated by controller
            .ForMember(dest => dest.ProductTypeList, opt => opt.Ignore())
            .ForMember(dest => dest.BrandList, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryList, opt => opt.Ignore())
            .ForMember(dest => dest.TagList, opt => opt.Ignore())
            .ForMember(dest => dest.StatusList, opt => opt.Ignore());


        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<ProductViewModel, Product>()
            // Ignore collections/navigation properties - handled manually in Controller
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Variants, opt => opt.Ignore())
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
            .ForMember(dest => dest.ProductType, opt => opt.Ignore())
            .ForMember(dest => dest.Brand, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectProducts, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore())
            // Ignore other fields not directly on ViewModel or handled by BaseEntity/SeoEntity
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore()) // ViewCount not edited here
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}
// --- END OF FILE ProductMappingProfile.cs ---

// --- START OF FILE ProductVariantProfile.cs --- No Changes Needed ---
using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Product;
namespace web.Areas.Admin.Mappers;
public class ProductVariantProfile : Profile { /* ... as before ... */ }
// --- END OF FILE ProductVariantProfile.cs ---

// Add ProductTypeProfile if not already created/standardized
// --- START OF FILE ProductTypeProfile.cs --- (Standardized from previous refactor) ---
using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.ProductType;
namespace web.Areas.Admin.Mappers;
public class ProductTypeProfile : Profile
{
    public ProductTypeProfile()
    {
        CreateMap<ProductType, ProductTypeListItemViewModel>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt));
        CreateMap<ProductType, ProductTypeViewModel>();
        CreateMap<ProductTypeViewModel, ProductType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}
// --- END OF FILE ProductTypeProfile.cs ---
```

**4. Validators**

*   Inject `DbContext` into `ProductViewModelValidator` for unique slug check.
*   Use `{PropertyName}` in messages.

```csharp
// --- START OF FILE ProductImageViewModelValidator.cs --- Updated ---
using FluentValidation;
using web.Areas.Admin.ViewModels.Product;
namespace web.Areas.Admin.Validators.Product;
public class ProductImageViewModelValidator : AbstractValidator<ProductImageViewModel>
{
    public ProductImageViewModelValidator()
    {
        When(x => !x.IsDeleted, () => { // Only validate non-deleted
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("{PropertyName} không được rỗng.").MaximumLength(2048); // Increased length
            RuleFor(x => x.ThumbnailUrl).MaximumLength(2048);
            RuleFor(x => x.AltText).MaximumLength(255);
            RuleFor(x => x.Title).MaximumLength(255);
            RuleFor(x => x.OrderIndex).GreaterThanOrEqualTo(0);
        });
    }
}
// --- END OF FILE ProductImageViewModelValidator.cs ---

// --- START OF FILE ProductVariantViewModelValidator.cs --- Updated ---
using FluentValidation;
using web.Areas.Admin.ViewModels.Product;
namespace web.Areas.Admin.Validators.Product;
public class ProductVariantViewModelValidator : AbstractValidator<ProductVariantViewModel>
{
    public ProductVariantViewModelValidator()
    {
        When(x => !x.IsDeleted, () => {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên biến thể không được rỗng.").MaximumLength(255);
            RuleFor(x => x.Sku).NotEmpty().WithMessage("SKU không được rỗng.").MaximumLength(100);
            // Unique SKU handled in Controller due to complexity (checking against other products)
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Giá phải lớn hơn hoặc bằng 0.");
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0).WithMessage("Số lượng tồn kho phải lớn hơn hoặc bằng 0.");
            RuleFor(x => x.Color).MaximumLength(50);
            RuleFor(x => x.Size).MaximumLength(50);
            RuleFor(x => x.Packaging).MaximumLength(50);
            RuleFor(x => x.ImageUrl).MaximumLength(2048);
        });
    }
}
// --- END OF FILE ProductVariantViewModelValidator.cs ---

// --- START OF FILE ProductViewModelValidator.cs --- Updated ---
using FluentValidation;
using infrastructure; // For DbContext
using Microsoft.EntityFrameworkCore; // For AnyAsync
using web.Areas.Admin.ViewModels.Product;
using web.Areas.Admin.Validators.Shared; // For SEO Validator

namespace web.Areas.Admin.Validators.Product;

public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
{
    private readonly ApplicationDbContext _context;

    public ProductViewModelValidator(ApplicationDbContext context) // Inject DbContext
    {
        _context = context;

        RuleFor(x => x.Name).NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.").MaximumLength(255);
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255)
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .MustAsync(BeUniqueSlug).WithMessage("{PropertyName} này đã tồn tại. Vui lòng chọn slug khác."); // DB check

        RuleFor(x => x.Description).NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");
        RuleFor(x => x.ShortDescription).MaximumLength(500);
        RuleFor(x => x.Manufacturer).MaximumLength(255);
        RuleFor(x => x.Origin).MaximumLength(100);
        // Length limits for other text areas if needed

        RuleFor(x => x.ProductTypeId).NotEmpty().WithMessage("Vui lòng chọn {PropertyName}.");
        RuleFor(x => x.Status).IsInEnum().WithMessage("{PropertyName} không hợp lệ.");

        // Assuming categories are required for products
        RuleFor(x => x.SelectedCategoryIds)
            .NotEmpty().WithMessage("Vui lòng chọn ít nhất một {PropertyName}.");

        // Apply validators to nested collections
        RuleForEach(x => x.Images).SetValidator(new ProductImageViewModelValidator());
        RuleForEach(x => x.Variants).SetValidator(new ProductVariantViewModelValidator());

        // Include base SEO validation rules
        Include(new SeoPropertiesValidator<ProductViewModel>());
    }

     private async Task<bool> BeUniqueSlug(ProductViewModel viewModel, string slug, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(slug)) return true;
        return !await _context.Set<domain.Entities.Product>()
                               .AnyAsync(p => p.Slug.ToLower() == slug.ToLower() && p.Id != viewModel.Id, cancellationToken);
    }
}
// --- END OF FILE ProductViewModelValidator.cs ---

// --- START OF FILE ProductTypeViewModelValidator.cs --- No Changes Needed ---
using domain.Entities; using FluentValidation; using infrastructure; using web.Areas.Admin.ViewModels.ProductType;
namespace web.Areas.Admin.Validators.ProductType;
public class ProductTypeViewModelValidator : AbstractValidator<ProductTypeViewModel> { /* ... as before ... */ }
// --- END OF FILE ProductTypeViewModelValidator.cs ---
```

**5. Controller (`ProductController.cs`)**

*   Implement pagination, logging, TempData, validation handling (middleware), `ProjectTo`.
*   Refine M-M (Category, Tag) and O-M (Image, Variant) update logic.
*   Add unique SKU checks (within submission and across DB) *before* calling main validator.
*   Standardize everything.

```csharp
// --- START OF FILE ProductController.cs --- Updated ---
using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using shared.Enums;
using shared.Extensions; // For GetDisplayName
using web.Areas.Admin.ViewModels.Product;
using X.PagedList;
using X.PagedList.EF;
using System.Linq; // Required for LINQ methods like Any, GroupBy etc.

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;
    // Remove IValidator injection

    public ProductController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProductController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Product
    public async Task<IActionResult> Index(string? searchTerm = null, int? typeId = null, int? brandId = null, PublishStatus? status = null, int page = 1, int pageSize = 15) // Added pagination
    {
        ViewData["Title"] = "Quản lý Sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Sản phẩm";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))) };

        int pageNumber = page;
        var query = _context.Set<Product>()
                            .Include(p => p.ProductType)
                            .Include(p => p.Brand)
                            .Include(p => p.Images.OrderBy(i => i.OrderIndex)) // Include ordered images
                            .AsNoTracking(); // Use AsNoTracking

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
             string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(lowerSearchTerm)
                                  || (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(lowerSearchTerm))
                                  // Optionally search SKU in variants
                                  // || p.Variants.Any(v => v.Sku.ToLower().Contains(lowerSearchTerm))
                                  );
        }
        if (typeId.HasValue && typeId > 0)
        {
            query = query.Where(p => p.ProductTypeId == typeId.Value);
        }
        if (brandId.HasValue && brandId > 0)
        {
            query = query.Where(p => p.BrandId == brandId.Value);
        }
        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        // Ordering and Pagination
        var productsPaged = await query
            .OrderByDescending(p => p.IsFeatured) // Featured first
            .ThenByDescending(p => p.CreatedAt)  // Then by creation date
            .ProjectTo<ProductListItemViewModel>(_mapper.ConfigurationProvider) // Project efficiently
            .ToPagedListAsync(pageNumber, pageSize); // Paginate

        // Load filter data
        await LoadFilterDropdownsAsync(typeId, brandId, status);

        ViewBag.SearchTerm = searchTerm;
        // Selected IDs/Status are loaded into ViewBag by LoadFilterDropdownsAsync

        return View(productsPaged); // Pass paged list
    }


    // GET: Admin/Product/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Thêm Sản phẩm mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm Sản phẩm mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ("Thêm mới", "") };

        var viewModel = new ProductViewModel
        {
            IsActive = true,
            Status = PublishStatus.Draft,
            SitemapPriority = 0.8,
            SitemapChangeFrequency = "weekly",
            OgType = "product",
            Images = new List<ProductImageViewModel>(), // Initialize lists
            Variants = new List<ProductVariantViewModel>() // Initialize lists
        };

        await LoadRelatedDataForFormAsync(viewModel); // Load dropdowns
        return View(viewModel);
    }

    // POST: Admin/Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel viewModel)
    {
        // --- Perform manual/complex checks before ModelState.IsValid ---
        await ValidateRelationsAndUniquenessAsync(viewModel, isEdit: false);

        // Rely on middleware for remaining validation (including FluentValidation DB checks like unique slug)
        if (ModelState.IsValid)
        {
            var product = _mapper.Map<Product>(viewModel);

            // --- MANUAL RELATIONSHIP HANDLING for NEW product ---
            product.ProductCategories = viewModel.SelectedCategoryIds? // Check for null
                                        .Select(catId => new ProductCategory { CategoryId = catId })
                                        .ToList() ?? new List<ProductCategory>(); // Default to empty list
            product.ProductTags = viewModel.SelectedTagIds? // Check for null
                                    .Select(tagId => new ProductTag { TagId = tagId })
                                    .ToList() ?? new List<ProductTag>(); // Default to empty list

            // Map only non-deleted Images and Variants from ViewModel
            product.Images = _mapper.Map<List<ProductImage>>(viewModel.Images?.Where(i => !i.IsDeleted).ToList() ?? new List<ProductImageViewModel>());
            product.Variants = _mapper.Map<List<ProductVariant>>(viewModel.Variants?.Where(v => !v.IsDeleted).ToList() ?? new List<ProductVariantViewModel>());

             // Set audit fields if needed
            // product.CreatedBy = User.Identity?.Name;

            _context.Products.Add(product);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product '{Name}' (ID: {ProductId}) created successfully by {User}.", product.Name, product.Id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Thêm sản phẩm '{product.Name}' thành công!"; // Standard key
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex) // Catch specific DB errors
            {
                _logger.LogError(ex, "Database error saving new product '{Name}'. Check constraints.", viewModel.Name);
                // Check inner exception for constraint violation if needed
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: products.slug", StringComparison.OrdinalIgnoreCase) ?? false) {
                     ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại.");
                } else if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: product_variants.sku", StringComparison.OrdinalIgnoreCase) ?? false) {
                     ModelState.AddModelError("Variants", "Một hoặc nhiều SKU đã tồn tại ở sản phẩm khác.");
                 } else {
                    ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi lưu sản phẩm.");
                 }
            }
             catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving new product '{Name}'.", viewModel.Name);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu sản phẩm.");
            }
        }
         else
        {
             _logger.LogWarning("Product creation failed for '{Name}'. Model state is invalid.", viewModel.Name);
        }

        // If failed, redisplay form
        await LoadRelatedDataForFormAsync(viewModel); // Reload dropdowns
        ViewData["Title"] = "Thêm Sản phẩm mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm Sản phẩm mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ("Thêm mới", "") };
        // Repopulate Images/Variants in the view model if needed after validation failure (e.g., if using hidden inputs and they get cleared)
        // viewModel.Images = viewModel.Images ?? new List<ProductImageViewModel>();
        // viewModel.Variants = viewModel.Variants ?? new List<ProductVariantViewModel>();
        return View(viewModel);
    }


    // GET: Admin/Product/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        // Eagerly load ALL related data needed for the form
        var product = await _context.Set<Product>()
            .Include(p => p.ProductType)
            .Include(p => p.Brand)
            .Include(p => p.Images.OrderBy(i => i.OrderIndex)) // Order images
            .Include(p => p.Variants.OrderBy(v => v.Name)) // Order variants
            .Include(p => p.ProductCategories)
            .Include(p => p.ProductTags)
            .AsNoTracking() // Use AsNoTracking for reading into ViewModel
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Edit GET: Product with ID {ProductId} not found.", id);
            return NotFound();
        }

        var viewModel = _mapper.Map<ProductViewModel>(product);
        // SelectedCategoryIds and SelectedTagIds are mapped by AutoMapper profile

        await LoadRelatedDataForFormAsync(viewModel); // Load dropdowns

        ViewData["Title"] = "Chỉnh sửa Sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Sản phẩm: {product.Name}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(product.Name)}", "") };

        return View(viewModel);
    }


    // POST: Admin/Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
             _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        // --- Perform manual/complex checks before ModelState.IsValid ---
        await ValidateRelationsAndUniquenessAsync(viewModel, isEdit: true);

        if (ModelState.IsValid)
        {
            // --- Load Existing TRACKED Entity with ALL Related Data for Update ---
            var product = await _context.Set<Product>()
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                 _logger.LogWarning("Edit POST: Product with ID {ProductId} not found for update.", id);
                 TempData["error"] = "Không tìm thấy sản phẩm để cập nhật.";
                 return RedirectToAction(nameof(Index));
            }

            // --- Map Scalar Properties from ViewModel to Entity ---
            _mapper.Map(viewModel, product);

            try
            {
                // --- MANUAL RELATIONSHIP HANDLING (UPDATE/ADD/DELETE) ---
                UpdateProductCategories(product, viewModel.SelectedCategoryIds);
                UpdateProductTags(product, viewModel.SelectedTagIds);
                UpdateProductImages(product, viewModel.Images);
                UpdateProductVariants(product, viewModel.Variants);

                 // Set audit fields if needed
                // product.UpdatedBy = User.Identity?.Name;

                await _context.SaveChangesAsync(); // Save all changes

                _logger.LogInformation("Product '{Name}' (ID: {ProductId}) updated successfully by {User}.", product.Name, id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật sản phẩm '{product.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }
             catch (DbUpdateConcurrencyException ex)
            {
                 _logger.LogWarning(ex, "Concurrency error occurred while editing Product ID {ProductId}.", id);
                 TempData["error"] = "Lỗi: Xung đột dữ liệu. Sản phẩm này có thể đã được cập nhật bởi người khác.";
            }
             catch (DbUpdateException ex) // Catch specific DB errors
            {
                _logger.LogError(ex, "Database error updating product ID {ProductId}. Check constraints.", id);
                 if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: products.slug", StringComparison.OrdinalIgnoreCase) ?? false) {
                     ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại.");
                 } else if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: product_variants.sku", StringComparison.OrdinalIgnoreCase) ?? false) {
                     ModelState.AddModelError("Variants", "Một hoặc nhiều SKU đã tồn tại ở sản phẩm khác.");
                 } else {
                    ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi cập nhật sản phẩm.");
                 }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product ID: {ProductId}.", id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật sản phẩm.");
            }
        }
        else
        {
            _logger.LogWarning("Product editing failed for ID: {ProductId}. Model state is invalid.", id);
        }

        // If failed, redisplay form
        await LoadRelatedDataForFormAsync(viewModel); // Reload dropdowns
        ViewData["Title"] = "Chỉnh sửa Sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Sản phẩm: {viewModel.Name}"; // Use name from VM
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(viewModel.Name)}", "") };
        // Ensure Images/Variants list is still populated in the view model
        // If using hidden fields, they should persist. If reloaded from DB, map again.
        return View(viewModel);
    }


    // POST: Admin/Product/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        // Include relations that have RESTRICT constraints if any (e.g., ProjectProduct, ArticleProduct)
        // Cascade should handle Images, Variants, Categories, Tags automatically.
        var product = await _context.Set<Product>()
                                 // .Include(p => p.ProjectProducts) // Uncomment if deletion restricted by Projects
                                 // .Include(p => p.ArticleProducts) // Uncomment if deletion restricted by Articles
                                 .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
             _logger.LogWarning("Delete POST: Product with ID {ProductId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
        }

        // --- Optional: Check for restricting relationships BEFORE attempting delete ---
        // if (product.ProjectProducts != null && product.ProjectProducts.Any())
        // {
        //     _logger.LogWarning("Attempted to delete Product ID {ProductId} referenced by Projects.", id);
        //     return Json(new { success = false, message = "Không thể xóa sản phẩm vì đang được liên kết với Dự án." });
        // }
        // if (product.ArticleProducts != null && product.ArticleProducts.Any())
        // {
        //      _logger.LogWarning("Attempted to delete Product ID {ProductId} referenced by Articles.", id);
        //     return Json(new { success = false, message = "Không thể xóa sản phẩm vì đang được liên kết với Bài viết." });
        // }
        // --- End Optional Checks ---


        try
        {
            string productName = product.Name;
            // NOTE: Deleting Images from MinIO should happen AFTER successful DB deletion
            // or via a background job/event listener to avoid orphaned files if DB delete fails.
            // We won't implement MinIO delete here for brevity.

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(); // DB delete

            _logger.LogInformation("Product '{ProductName}' (ID: {ProductId}) deleted successfully by {User}.", productName, id, User.Identity?.Name ?? "Unknown");
            // TODO: Implement logic to delete associated images from MinIO storage here or queue a job.

            return Json(new { success = true, message = $"Xóa sản phẩm '{productName}' thành công." });
        }
        catch (DbUpdateException ex) // Catch potential foreign key constraint violations (RESTRICT)
        {
            _logger.LogError(ex, "Error deleting Product ID {ProductId}. Potential RESTRICT constraint violation.", id);
            // Provide a more specific message if possible based on the exception details
             if (ex.InnerException?.Message.Contains("FOREIGN KEY constraint", StringComparison.OrdinalIgnoreCase) ?? false) {
                 return Json(new { success = false, message = "Không thể xóa sản phẩm. Sản phẩm có thể đang được liên kết với dữ liệu khác (ví dụ: Dự án, Bài viết)." });
             }
             return Json(new { success = false, message = "Lỗi cơ sở dữ liệu khi xóa sản phẩm." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Product ID {ProductId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa sản phẩm." });
        }
    }


    // --- Helper Methods ---

    // Helper to load dropdowns for Create/Edit views
    private async Task LoadRelatedDataForFormAsync(ProductViewModel viewModel)
    {
        viewModel.ProductTypeList = new SelectList(await _context.Set<ProductType>().Where(t => t.IsActive).OrderBy(t => t.Name).Select(t=> new {t.Id, t.Name}).ToListAsync(), "Id", "Name", viewModel.ProductTypeId);
        viewModel.BrandList = new SelectList(await _context.Set<Brand>().Where(t => t.IsActive).OrderBy(t => t.Name).Select(t=> new {t.Id, t.Name}).ToListAsync(), "Id", "Name", viewModel.BrandId);
        viewModel.CategoryList = new SelectList(await _context.Set<Category>().Where(c => c.IsActive && c.Type == CategoryType.Product).OrderBy(c => c.Name).Select(t=> new {t.Id, t.Name}).ToListAsync(), "Id", "Name");
        viewModel.TagList = new SelectList(await _context.Set<Tag>().Where(t => t.Type == TagType.Product).OrderBy(t => t.Name).Select(t=> new {t.Id, t.Name}).ToListAsync(), "Id", "Name");
        viewModel.StatusList = new SelectList(Enum.GetValues(typeof(PublishStatus))
                                               .Cast<PublishStatus>()
                                               .Select(e => new { Value = e, Text = e.GetDisplayName() }), // Use shared extension
                                               "Value", "Text", viewModel.Status);
    }

    // Helper to load dropdowns for Index filter controls
    private async Task LoadFilterDropdownsAsync(int? typeId, int? brandId, PublishStatus? status)
    {
        ViewBag.ProductTypes = await _context.Set<ProductType>().Where(pt => pt.IsActive).OrderBy(pt => pt.Name).Select(pt => new SelectListItem { Value = pt.Id.ToString(), Text = pt.Name, Selected = pt.Id == typeId }).ToListAsync();
        ViewBag.Brands = await _context.Set<Brand>().Where(b => b.IsActive).OrderBy(b => b.Name).Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name, Selected = b.Id == brandId }).ToListAsync();
        ViewBag.Statuses = Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(s => new SelectListItem { Value = s.ToString(), Text = s.GetDisplayName(), Selected = s == status }).ToList(); // Use shared extension
        ViewBag.SelectedTypeId = typeId;
        ViewBag.SelectedBrandId = brandId;
        ViewBag.SelectedStatus = status;
    }

     // Helper for complex validation before main model validation
    private async Task ValidateRelationsAndUniquenessAsync(ProductViewModel viewModel, bool isEdit)
    {
        // 1. Check Main Image (if images exist and none are deleted)
        if (viewModel.Images != null && viewModel.Images.Any(i => !i.IsDeleted) && !viewModel.Images.Any(i => i.IsMain && !i.IsDeleted))
        {
            ModelState.AddModelError(nameof(viewModel.Images), "Vui lòng chọn một ảnh làm ảnh đại diện chính.");
        }
         // Ensure only one image is main
        if (viewModel.Images != null && viewModel.Images.Count(i => i.IsMain && !i.IsDeleted) > 1)
        {
             ModelState.AddModelError(nameof(viewModel.Images), "Chỉ được chọn một ảnh làm ảnh đại diện chính.");
        }

        // 2. Check Variant SKU uniqueness (within submission)
        var submittedVariants = viewModel.Variants?.Where(v => !v.IsDeleted).ToList() ?? new List<ProductVariantViewModel>();
        var duplicateSkusInSubmission = submittedVariants
                               .Where(v => !string.IsNullOrWhiteSpace(v.Sku))
                               .GroupBy(v => v.Sku.Trim().ToLowerInvariant()) // Case-insensitive and trim
                               .Where(g => g.Count() > 1)
                               .Select(g => g.Key).ToList();

        if (duplicateSkusInSubmission.Any())
        {
            ModelState.AddModelError("Variants", $"Các SKU sau bị trùng lặp trong danh sách biến thể: {string.Join(", ", duplicateSkusInSubmission)}");
        }

        // 3. Check Variant SKU uniqueness (against DB, excluding own variants if editing)
        var skusToCheck = submittedVariants
                          .Where(v => !string.IsNullOrWhiteSpace(v.Sku))
                          .Select(v => v.Sku.Trim().ToLowerInvariant())
                          .Distinct()
                          .ToList();

        if (skusToCheck.Any())
        {
            // Find existing SKUs in the DB that belong to OTHER products
            var query = _context.Set<ProductVariant>().Where(v => skusToCheck.Contains(v.Sku.ToLower()));
            if(isEdit) {
                query = query.Where(v => v.ProductId != viewModel.Id); // Exclude current product's variants during edit
            }
            var existingDbSku = await query.Select(v => v.Sku).FirstOrDefaultAsync();

            if (existingDbSku != null)
            {
                ModelState.AddModelError("Variants", $"SKU '{existingDbSku}' đã tồn tại ở một sản phẩm khác.");
            }
        }
    }

     // --- Helpers for M-M and O-M Updates (called from Edit POST) ---

    private void UpdateProductCategories(Product product, List<int>? selectedCategoryIds)
    {
        selectedCategoryIds ??= new List<int>(); // Ensure list is not null

        // Remove categories no longer selected
        var categoriesToRemove = product.ProductCategories
                                       .Where(pc => !selectedCategoryIds.Contains(pc.CategoryId))
                                       .ToList();
        if (categoriesToRemove.Any())
        {
            _context.ProductCategories.RemoveRange(categoriesToRemove);
        }

        // Add newly selected categories
        var existingCategoryIds = product.ProductCategories.Select(pc => pc.CategoryId);
        var categoryIdsToAdd = selectedCategoryIds.Except(existingCategoryIds).ToList();
        if (categoryIdsToAdd.Any())
        {
            foreach (var catId in categoryIdsToAdd)
            {
                product.ProductCategories.Add(new ProductCategory { CategoryId = catId });
            }
        }
    }

    private void UpdateProductTags(Product product, List<int>? selectedTagIds)
    {
         selectedTagIds ??= new List<int>();

        // Remove tags no longer selected
        var tagsToRemove = product.ProductTags
                                 .Where(pt => !selectedTagIds.Contains(pt.TagId))
                                 .ToList();
        if (tagsToRemove.Any())
        {
            _context.ProductTags.RemoveRange(tagsToRemove);
        }

        // Add newly selected tags
        var existingTagIds = product.ProductTags.Select(pt => pt.TagId);
        var tagIdsToAdd = selectedTagIds.Except(existingTagIds).ToList();
        if (tagIdsToAdd.Any())
        {
            foreach (var tagId in tagIdsToAdd)
            {
                product.ProductTags.Add(new ProductTag { TagId = tagId });
            }
        }
    }

    private void UpdateProductImages(Product product, List<ProductImageViewModel>? imageVMs)
    {
        imageVMs ??= new List<ProductImageViewModel>();

        // Delete images marked for deletion in VM or not present in VM anymore
        var vmImageIds = imageVMs.Where(i => i.Id > 0).Select(i => i.Id).ToList();
        var dbImagesToRemove = product.Images
                                    .Where(dbImg => !vmImageIds.Contains(dbImg.Id) || imageVMs.First(vm => vm.Id == dbImg.Id).IsDeleted)
                                    .ToList();
        if(dbImagesToRemove.Any()) {
            // TODO: Queue these image URLs for deletion from MinIO storage AFTER SaveChangesAsync succeeds
            // var urlsToDelete = dbImagesToRemove.Select(img => img.ImageUrl).Where(url => !string.IsNullOrEmpty(url)).Distinct();
             _context.ProductImages.RemoveRange(dbImagesToRemove);
        }


        // Update existing or add new images
        foreach (var imgVm in imageVMs.Where(i => !i.IsDeleted))
        {
            if (imgVm.Id > 0) // Update existing
            {
                var dbImage = product.Images.FirstOrDefault(i => i.Id == imgVm.Id);
                if (dbImage != null)
                {
                    // Map only the editable fields from VM to existing DB entity
                    dbImage.AltText = imgVm.AltText;
                    dbImage.Title = imgVm.Title;
                    dbImage.OrderIndex = imgVm.OrderIndex;
                    dbImage.IsMain = imgVm.IsMain;
                    // Don't map ImageUrl/ThumbnailUrl here as they are set on upload
                }
            }
            else // Add new
            {
                 // Only add if ImageUrl is actually set (meaning upload was successful or path provided)
                 if (!string.IsNullOrWhiteSpace(imgVm.ImageUrl)) {
                     var newImage = _mapper.Map<ProductImage>(imgVm);
                     // ProductId is set automatically by EF Core when adding to navigation property
                     product.Images.Add(newImage);
                 } else {
                     _logger.LogWarning("Skipping adding new image because ImageUrl is empty. VM data: {@ImageViewModel}", imgVm);
                 }
            }
        }
    }

    private void UpdateProductVariants(Product product, List<ProductVariantViewModel>? variantVMs)
    {
        variantVMs ??= new List<ProductVariantViewModel>();

        // Delete variants marked for deletion in VM or not present in VM anymore
        var vmVariantIds = variantVMs.Where(v => v.Id > 0).Select(v => v.Id).ToList();
         var dbVariantsToRemove = product.Variants
                                    .Where(dbVar => !vmVariantIds.Contains(dbVar.Id) || variantVMs.First(vm => vm.Id == dbVar.Id).IsDeleted)
                                    .ToList();
         if(dbVariantsToRemove.Any()) {
            _context.ProductVariants.RemoveRange(dbVariantsToRemove);
         }


        // Update existing or add new variants
        foreach (var varVm in variantVMs.Where(v => !v.IsDeleted))
        {
            if (varVm.Id > 0) // Update existing
            {
                var dbVariant = product.Variants.FirstOrDefault(v => v.Id == varVm.Id);
                if (dbVariant != null)
                {
                    // Map all editable fields
                    _mapper.Map(varVm, dbVariant);
                }
            }
            else // Add new
            {
                var newVariant = _mapper.Map<ProductVariant>(varVm);
                // ProductId is set automatically by EF Core
                product.Variants.Add(newVariant);
            }
        }
    }

    private string Truncate(string value, int maxLength = 30) // Helper for short display
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }

    // Remove ProductExists check
}
// --- END OF FILE ProductController.cs ---
```

**7. Views**

*   **`_ProductFormFields.txt`:** Add standard SEO fields section (copied from Category/Brand). Ensure IDs are present on key inputs.
*   **`Create`/`Edit`:** Ensure client-side JS for TomSelect, Summernote, Slug, Images, and Variants is correctly implemented and uses element IDs. Add `_ValidationScriptsPartial`. Standardize card structure/footer.
*   **`Index.txt`:** Implement pagination, item count, standard filter/delete controls. Use `MediaUrlHelper`.

```html
@* --- START OF FILE _ProductFormFields.txt --- Updated --- *@
@using shared.Helpers
@using web.Areas.Admin.ViewModels.Product
@model ProductViewModel

@* --- TABS --- *@
<ul class="nav nav-tabs nav-fill mb-3" data-bs-toggle="tabs" role="tablist">
	<li class="nav-item" role="presentation">
		<a href="#tab-basic-info" class="nav-link active" data-bs-toggle="tab" aria-selected="true" role="tab">
			<i class="ti ti-info-circle me-2"></i> Thông tin cơ bản
		</a>
	</li>
	<li class="nav-item" role="presentation">
		<a href="#tab-descriptions" class="nav-link" data-bs-toggle="tab" aria-selected="false" tabindex="-1" role="tab">
			<i class="ti ti-file-text me-2"></i> Mô tả & Chi tiết
		</a>
	</li>
	<li class="nav-item" role="presentation">
		<a href="#tab-images" class="nav-link" data-bs-toggle="tab" aria-selected="false" tabindex="-1" role="tab">
			<i class="ti ti-photo me-2"></i> Hình ảnh
		</a>
	</li>
	<li class="nav-item" role="presentation">
		<a href="#tab-variants" class="nav-link" data-bs-toggle="tab" aria-selected="false" tabindex="-1" role="tab">
			<i class="ti ti-adjustments-alt me-2"></i> Biến thể & Kho
		</a>
	</li>
	<li class="nav-item" role="presentation">
		<a href="#tab-relations" class="nav-link" data-bs-toggle="tab" aria-selected="false" tabindex="-1" role="tab">
			<i class="ti ti-tags me-2"></i> Phân loại
		</a>
	</li>
	<li class="nav-item" role="presentation">
		<a href="#tab-seo" class="nav-link" data-bs-toggle="tab" aria-selected="false" tabindex="-1" role="tab">
			<i class="ti ti-search me-2"></i> Tối ưu SEO
		</a>
	</li>
</ul>

@* --- TAB CONTENT --- *@
<div class="tab-content">
	@* --- Tab: Basic Info --- *@
	<div class="tab-pane active show" id="tab-basic-info" role="tabpanel">
		<div class="row">
			<div class="col-md-8">
				<div class="form-group mb-3">
					<label asp-for="Name" class="form-label required"></label>
					<input asp-for="Name" class="form-control" id="productName" /> @* Use ID *@
					<span asp-validation-for="Name" class="text-danger"></span>
				</div>
				<div class="form-group mb-3">
					<label asp-for="Slug" class="form-label required"></label>
					<div class="input-group">
						<input asp-for="Slug" class="form-control" id="productSlug" /> @* Use ID *@
						<button class="btn btn-outline-secondary" type="button" id="generateSlugButton"> <i class="ti ti-refresh"></i> </button>
					</div>
					<span asp-validation-for="Slug" class="text-danger"></span>
					<small class="form-hint">Dùng cho URL (chữ thường, số, dấu gạch ngang)</small>
				</div>
				<div class="form-group mb-3">
					<label asp-for="ShortDescription" class="form-label"></label>
					<textarea asp-for="ShortDescription" class="form-control" rows="3"></textarea>
					<span asp-validation-for="ShortDescription" class="text-danger"></span>
				</div>
				<div class="form-group mb-3">
					<label asp-for="Manufacturer" class="form-label"></label>
					<input asp-for="Manufacturer" class="form-control" />
					<span asp-validation-for="Manufacturer" class="text-danger"></span>
				</div>
				<div class="form-group mb-3 mb-md-0"> @* Adjust margin *@
					<label asp-for="Origin" class="form-label"></label>
					<input asp-for="Origin" class="form-control" />
					<span asp-validation-for="Origin" class="text-danger"></span>
				</div>
			</div>
			<div class="col-md-4">
				<div class="form-group mb-3">
					<label asp-for="ProductTypeId" class="form-label required"></label>
					<select asp-for="ProductTypeId" asp-items="@Model.ProductTypeList" class="form-select tom-select-single" id="ProductTypeId"> @* Use TomSelect for single *@
						<option value="">-- Chọn loại --</option>
					</select>
					<span asp-validation-for="ProductTypeId" class="text-danger"></span>
				</div>
				<div class="form-group mb-3">
					<label asp-for="BrandId" class="form-label"></label>
					<select asp-for="BrandId" asp-items="@Model.BrandList" class="form-select tom-select-single" id="BrandId"> @* Use TomSelect for single *@
						<option value="">-- Chọn thương hiệu (nếu có) --</option>
					</select>
					<span asp-validation-for="BrandId" class="text-danger"></span>
				</div>
				<div class="form-group mb-3">
					<label asp-for="Status" class="form-label required"></label>
					<select asp-for="Status" asp-items="@Model.StatusList" class="form-select tom-select-single" id="Status"></select> @* Use TomSelect for single *@
					<span asp-validation-for="Status" class="text-danger"></span>
				</div>
				<hr class="my-3">
				<div class="form-check form-switch mb-3">
					<input class="form-check-input" type="checkbox" asp-for="IsActive" role="switch" id="IsActive"> @* Use ID *@
					<label class="form-check-label" asp-for="IsActive"></label>
					<span asp-validation-for="IsActive" class="text-danger"></span>
				</div>
				<div class="form-check form-switch mb-0">
					<input class="form-check-input" type="checkbox" asp-for="IsFeatured" role="switch" id="IsFeatured"> @* Use ID *@
					<label class="form-check-label" asp-for="IsFeatured"></label>
					<span asp-validation-for="IsFeatured" class="text-danger"></span>
				</div>
			</div>
		</div>
	</div>

	@* --- Tab: Descriptions --- *@
	<div class="tab-pane" id="tab-descriptions" role="tabpanel">
		<div class="form-group mb-3">
			<label asp-for="Description" class="form-label required"></label>
			<textarea asp-for="Description" class="form-control summernote-editor" rows="15" id="productDescriptionEditor"></textarea> @* Add specific class/ID *@
			<span asp-validation-for="Description" class="text-danger"></span>
		</div>
		<div class="row">
			<div class="col-md-6">
				<div class="form-group mb-3">
					<label asp-for="Specifications" class="form-label"></label>
					<textarea asp-for="Specifications" class="form-control summernote-editor" rows="8" id="Specifications"></textarea> @* Add ID *@
					<span asp-validation-for="Specifications" class="text-danger"></span>
				</div>
				<div class="form-group mb-3">
					<label asp-for="Features" class="form-label"></label>
					<textarea asp-for="Features" class="form-control summernote-editor" rows="8" id="Features"></textarea> @* Add ID *@
					<span asp-validation-for="Features" class="text-danger"></span>
				</div>
				<div class="form-group mb-3 mb-md-0"> @* Adjust margin *@
					<label asp-for="ApplicationAreas" class="form-label"></label>
					<textarea asp-for="ApplicationAreas" class="form-control" rows="5" id="ApplicationAreas"></textarea> @* Add ID *@
					<span asp-validation-for="ApplicationAreas" class="text-danger"></span>
				</div>
			</div>
			<div class="col-md-6">
				<div class="form-group mb-3">
					<label asp-for="Usage" class="form-label"></label>
					<textarea asp-for="Usage" class="form-control summernote-editor" rows="8" id="Usage"></textarea> @* Add ID *@
					<span asp-validation-for="Usage" class="text-danger"></span>
				</div>
				<div class="form-group mb-3">
					<label asp-for="PackagingInfo" class="form-label"></label>
					<textarea asp-for="PackagingInfo" class="form-control" rows="5" id="PackagingInfo"></textarea> @* Add ID *@
					<span asp-validation-for="PackagingInfo" class="text-danger"></span>
				</div>
				<div class="form-group mb-3">
					<label asp-for="StorageInstructions" class="form-label"></label>
					<textarea asp-for="StorageInstructions" class="form-control" rows="5" id="StorageInstructions"></textarea> @* Add ID *@
					<span asp-validation-for="StorageInstructions" class="text-danger"></span>
				</div>
				<div class="form-group mb-3">
					<label asp-for="SafetyInfo" class="form-label"></label>
					<textarea asp-for="SafetyInfo" class="form-control" rows="5" id="SafetyInfo"></textarea> @* Add ID *@
					<span asp-validation-for="SafetyInfo" class="text-danger"></span>
				</div>
			</div>
		</div>
		<div class="form-group mb-0"> @* Adjust margin *@
			<label asp-for="TechnicalDocuments" class="form-label"></label>
			<textarea asp-for="TechnicalDocuments" class="form-control" rows="5" id="TechnicalDocuments"></textarea> @* Add ID *@
			<span asp-validation-for="TechnicalDocuments" class="text-danger"></span>
			<small class="form-hint">Nhập cấu trúc JSON cho các tài liệu liên quan. Ví dụ: [{"name": "TDS", "url": "..."}, {"name": "MSDS", "url": "..."}]</small>
		</div>
	</div>

	@* --- Tab: Images --- *@
	<div class="tab-pane" id="tab-images" role="tabpanel">
		<div class="d-flex justify-content-between align-items-center mb-3">
			<h3 class="m-0">Quản lý Hình ảnh</h3>
			<button type="button" class="btn btn-outline-primary" id="selectProductImagesBtn">
				<i class="ti ti-photo-plus me-2"></i> Chọn ảnh từ thư viện
			</button>
		</div>
		<span asp-validation-for="Images" class="text-danger d-block mb-2"></span> @* General validation message *@

		<div id="product-image-list" class="row g-3">
			@* JS will render images here using a template *@
			@for (int i = 0; i < Model.Images.Count; i++)
			{
				var imgId = $"image_{i}"; // Base ID for this item
				<div class="col-md-6 col-lg-4 col-xl-3 product-image-item" data-index="@i" data-image-path="@Model.Images[i].ImageUrl">
					<input type="hidden" asp-for="Images[i].Id" />
					<input type="hidden" asp-for="Images[i].ImageUrl" class="image-url-input" />
					<input type="hidden" asp-for="Images[i].ThumbnailUrl" class="thumb-url-input" />
					<input type="hidden" asp-for="Images[i].IsDeleted" class="is-deleted-input" value="false" />
					<div class="card">
						<div class="img-responsive img-responsive-16x9 card-img-top image-preview"
							 style="background-image: url(@MediaUrlHelper.GetMinioUrl(Model.Images[i].ThumbnailUrl ?? Model.Images[i].ImageUrl, "/img/placeholder.svg"))"></div>
						<div class="card-body">
							<div class="form-group mb-2">
								<label asp-for="Images[i].AltText" class="form-label form-label-sm"></label>
								<input asp-for="Images[i].AltText" class="form-control form-control-sm" id="@($"{imgId}_AltText")"/>
								 <span asp-validation-for="Images[i].AltText" class="text-danger"></span>
							</div>
							<div class="form-group mb-2">
								<label asp-for="Images[i].Title" class="form-label form-label-sm"></label>
								<input asp-for="Images[i].Title" class="form-control form-control-sm" id="@($"{imgId}_Title")" />
								 <span asp-validation-for="Images[i].Title" class="text-danger"></span>
							</div>
							<div class="row g-2 align-items-center">
								<div class="col">
									<div class="form-check form-switch">
										@* Use unique name per item for radio group behavior within the item *@
										<input class="form-check-input is-main-input" type="radio" name="ImagesMainRadio" id="@($"{imgId}_IsMain")" value="@i" checked="@Model.Images[i].IsMain">
										<input type="hidden" asp-for="Images[i].IsMain" class="is-main-hidden" />
										<label class="form-check-label form-label-sm" asp-for="Images[i].IsMain" for="@($"{imgId}_IsMain")"></label>
									</div>
								</div>
								<div class="col-auto">
									<input type="number" asp-for="Images[i].OrderIndex" class="form-control form-control-sm order-index-input" style="width: 60px;" title="Thứ tự" min="0" id="@($"{imgId}_OrderIndex")"/>
									 <span asp-validation-for="Images[i].OrderIndex" class="text-danger"></span>
								</div>
								<div class="col-auto">
									<button type="button" class="btn btn-sm btn-outline-danger remove-image-btn" title="Gỡ ảnh này"><i class="ti ti-trash"></i></button>
								</div>
							</div>
						</div>
					</div>
				</div>
			}
		</div>

		@* Template for NEWLY SELECTED images (hidden) *@
		<template id="product-image-template">
			<div class="col-md-6 col-lg-4 col-xl-3 product-image-item" data-index="__INDEX__" data-image-path="__IMAGE_PATH__">
				<input type="hidden" name="Images[__INDEX__].Id" value="0" />
				<input type="hidden" name="Images[__INDEX__].ImageUrl" class="image-url-input" value="__IMAGE_PATH__" />
				<input type="hidden" name="Images[__INDEX__].ThumbnailUrl" class="thumb-url-input" value="__THUMB_PATH__" />
				<input type="hidden" name="Images[__INDEX__].IsDeleted" class="is-deleted-input" value="false" />
				<div class="card">
					<div class="img-responsive img-responsive-16x9 card-img-top image-preview" style="background-image: url(__DISPLAY_URL__)"></div>
					<div class="card-body">
						<div class="form-group mb-2">
							<label name="Images[__INDEX__].AltText" class="form-label form-label-sm">Alt Text</label>
							<input name="Images[__INDEX__].AltText" class="form-control form-control-sm" id="Images___INDEX___AltText" value="__ALT_TEXT__"/>
						</div>
						<div class="form-group mb-2">
							<label name="Images[__INDEX__].Title" class="form-label form-label-sm">Title</label>
							<input name="Images[__INDEX__].Title" class="form-control form-control-sm" id="Images___INDEX___Title"/>
						</div>
						<div class="row g-2 align-items-center">
							<div class="col">
								<div class="form-check form-switch">
									<input class="form-check-input is-main-input" type="radio" name="ImagesMainRadio" id="Images___INDEX___IsMain" value="__INDEX__">
									<input type="hidden" name="Images[__INDEX__].IsMain" class="is-main-hidden" value="false" />
									<label class="form-check-label form-label-sm" for="Images___INDEX___IsMain">Ảnh chính</label>
								</div>
							</div>
							<div class="col-auto">
								<input type="number" name="Images[__INDEX__].OrderIndex" class="form-control form-control-sm order-index-input" value="0" style="width: 60px;" title="Thứ tự" min="0" id="Images___INDEX___OrderIndex"/>
							</div>
							<div class="col-auto">
								<button type="button" class="btn btn-sm btn-outline-danger remove-image-btn" title="Gỡ ảnh này"><i class="ti ti-trash"></i></button>
							</div>
						</div>
					</div>
				</div>
			</div>
		</template>
	</div>

	@* --- Tab: Variants --- *@
	<div class="tab-pane" id="tab-variants" role="tabpanel">
		<div class="d-flex justify-content-between align-items-center mb-3">
            <h3 class="m-0">Biến thể & Kho hàng</h3>
            <button type="button" class="btn btn-outline-primary" id="add-variant-btn">
				<i class="ti ti-plus me-1"></i> Thêm biến thể
			</button>
        </div>
		<span asp-validation-for="Variants" class="text-danger d-block mb-2"></span>

		<div id="product-variant-list">
			@* JS will render variants here using a template *@
			@for (int i = 0; i < Model.Variants.Count; i++)
			{
                var varId = $"variant_{i}"; // Base ID
				<div class="card mb-3 product-variant-item" data-index="@i">
					<input type="hidden" asp-for="Variants[i].Id" />
					<input type="hidden" asp-for="Variants[i].IsDeleted" class="is-deleted-input" value="false" />
					<div class="card-body">
						<div class="row g-3">
							<div class="col-md-4">
								<label asp-for="Variants[i].Name" class="form-label required"></label>
								<input asp-for="Variants[i].Name" class="form-control" id="@($"{varId}_Name")"/>
								<span asp-validation-for="Variants[i].Name" class="text-danger"></span>
							</div>
							<div class="col-md-3">
								<label asp-for="Variants[i].Sku" class="form-label required"></label>
								<input asp-for="Variants[i].Sku" class="form-control" id="@($"{varId}_Sku")"/>
								<span asp-validation-for="Variants[i].Sku" class="text-danger"></span>
							</div>
							<div class="col-md-2">
								<label asp-for="Variants[i].Price" class="form-label required"></label>
								<input type="number" asp-for="Variants[i].Price" class="form-control" step="any" min="0" id="@($"{varId}_Price")"/>
								<span asp-validation-for="Variants[i].Price" class="text-danger"></span>
							</div>
							<div class="col-md-2">
								<label asp-for="Variants[i].StockQuantity" class="form-label"></label>
								<input type="number" asp-for="Variants[i].StockQuantity" class="form-control" min="0" id="@($"{varId}_StockQuantity")"/>
								<span asp-validation-for="Variants[i].StockQuantity" class="text-danger"></span>
							</div>
							<div class="col-md-1 d-flex align-items-end">
								<button type="button" class="btn btn-outline-danger remove-variant-btn" title="Xóa biến thể này"><i class="ti ti-trash"></i></button>
							</div>
							<div class="col-md-3">
								<label asp-for="Variants[i].Color" class="form-label"></label>
								<input asp-for="Variants[i].Color" class="form-control" id="@($"{varId}_Color")"/>
                                <span asp-validation-for="Variants[i].Color" class="text-danger"></span>
							</div>
							<div class="col-md-3">
								<label asp-for="Variants[i].Size" class="form-label"></label>
								<input asp-for="Variants[i].Size" class="form-control" id="@($"{varId}_Size")"/>
                                <span asp-validation-for="Variants[i].Size" class="text-danger"></span>
							</div>
							<div class="col-md-3">
								<label asp-for="Variants[i].Packaging" class="form-label"></label>
								<input asp-for="Variants[i].Packaging" class="form-control" id="@($"{varId}_Packaging")"/>
                                <span asp-validation-for="Variants[i].Packaging" class="text-danger"></span>
							</div>
							<div class="col-md-3 d-flex align-items-center">
								<div class="form-check form-switch mt-3"> @* Add margin top for alignment *@
									<input class="form-check-input" type="checkbox" asp-for="Variants[i].IsActive" role="switch" id="@($"{varId}_IsActive")">
									<label class="form-check-label" asp-for="Variants[i].IsActive"></label>
                                     <span asp-validation-for="Variants[i].IsActive" class="text-danger"></span>
								</div>
							</div>
						</div>
					</div>
				</div>
			}
		</div>

		@* Template for new variants (hidden) *@
		<template id="product-variant-template">
			<div class="card mb-3 product-variant-item" data-index="__INDEX__">
				<input type="hidden" name="Variants[__INDEX__].Id" value="0" />
				<input type="hidden" name="Variants[__INDEX__].IsDeleted" class="is-deleted-input" value="false" />
				<div class="card-body">
					<div class="row g-3">
						<div class="col-md-4"><label name="Variants[__INDEX__].Name" class="form-label required">Tên biến thể</label><input name="Variants[__INDEX__].Name" class="form-control" id="Variants___INDEX___Name"/></div>
						<div class="col-md-3"><label name="Variants[__INDEX__].Sku" class="form-label required">SKU</label><input name="Variants[__INDEX__].Sku" class="form-control" id="Variants___INDEX___Sku"/></div>
						<div class="col-md-2"><label name="Variants[__INDEX__].Price" class="form-label required">Giá</label><input type="number" name="Variants[__INDEX__].Price" class="form-control" step="any" min="0" value="0" id="Variants___INDEX___Price"/></div>
						<div class="col-md-2"><label name="Variants[__INDEX__].StockQuantity" class="form-label">Tồn kho</label><input type="number" name="Variants[__INDEX__].StockQuantity" class="form-control" min="0" value="0" id="Variants___INDEX___StockQuantity"/></div>
						<div class="col-md-1 d-flex align-items-end"><button type="button" class="btn btn-outline-danger remove-variant-btn" title="Xóa biến thể này"><i class="ti ti-trash"></i></button></div>
						<div class="col-md-3"><label name="Variants[__INDEX__].Color" class="form-label">Màu sắc</label><input name="Variants[__INDEX__].Color" class="form-control" id="Variants___INDEX___Color"/></div>
						<div class="col-md-3"><label name="Variants[__INDEX__].Size" class="form-label">Kích thước/Dung tích</label><input name="Variants[__INDEX__].Size" class="form-control" id="Variants___INDEX___Size"/></div>
						<div class="col-md-3"><label name="Variants[__INDEX__].Packaging" class="form-label">Đóng gói</label><input name="Variants[__INDEX__].Packaging" class="form-control" id="Variants___INDEX___Packaging"/></div>
						<div class="col-md-3 d-flex align-items-center">
							<div class="form-check form-switch mt-3">
								<input class="form-check-input" type="checkbox" name="Variants[__INDEX__].IsActive" value="true" role="switch" id="Variants___INDEX___IsActive" checked>
								<label class="form-check-label" for="Variants___INDEX___IsActive">Hoạt động</label>
							</div>
						</div>
					</div>
				</div>
			</div>
		</template>
	</div>

	@* --- Tab: Relations (Categories & Tags) --- *@
	<div class="tab-pane" id="tab-relations" role="tabpanel">
		<div class="row">
			<div class="col-md-6">
				<div class="form-group mb-3">
					<label asp-for="SelectedCategoryIds" class="form-label required"></label>
					<select asp-for="SelectedCategoryIds" asp-items="@Model.CategoryList" class="form-select tom-select-multiple" multiple="multiple" id="SelectedCategoryIds"></select> @* Use TomSelect *@
					<span asp-validation-for="SelectedCategoryIds" class="text-danger"></span>
					<small class="form-hint">Chọn một hoặc nhiều danh mục.</small>
				</div>
			</div>
			<div class="col-md-6">
				<div class="form-group mb-3">
					<label asp-for="SelectedTagIds" class="form-label"></label>
					<select asp-for="SelectedTagIds" asp-items="@Model.TagList" class="form-select tom-select-multiple" multiple="multiple" id="SelectedTagIds"></select> @* Use TomSelect *@
					<span asp-validation-for="SelectedTagIds" class="text-danger"></span>
					<small class="form-hint">Chọn các thẻ liên quan.</small>
				</div>
			</div>
		</div>
	</div>

	@* --- Tab: SEO --- *@
	<div class="tab-pane" id="tab-seo" role="tabpanel">
		<h3 class="card-title mb-3">Tối ưu SEO</h3>
		<div class="form-group mb-3">
			<label asp-for="MetaTitle" class="form-label"></label>
			<input asp-for="MetaTitle" class="form-control" />
			<span asp-validation-for="MetaTitle" class="text-danger"></span>
			<small class="form-hint">Tiêu đề hiển thị trên kết quả tìm kiếm (tối ưu < 60 ký tự).</small>
		</div>
		<div class="form-group mb-3">
			<label asp-for="MetaDescription" class="form-label"></label>
			<textarea asp-for="MetaDescription" class="form-control" rows="3"></textarea>
			<span asp-validation-for="MetaDescription" class="text-danger"></span>
			<small class="form-hint">Mô tả ngắn gọn hiển thị trên kết quả tìm kiếm (tối ưu < 160 ký tự).</small>
		</div>
		<div class="form-group mb-3">
			<label asp-for="MetaKeywords" class="form-label"></label>
			<input asp-for="MetaKeywords" class="form-control" />
			<span asp-validation-for="MetaKeywords" class="text-danger"></span>
			<small class="form-hint">Các từ khóa liên quan, cách nhau bởi dấu phẩy (,).</small>
		</div>
		<hr class="my-3" />
		<div class="form-group mb-3">
			<label asp-for="CanonicalUrl" class="form-label"></label>
			<input asp-for="CanonicalUrl" class="form-control" type="url" />
			<span asp-validation-for="CanonicalUrl" class="text-danger"></span>
			<small class="form-hint">URL chuẩn cho trang này nếu có nội dung trùng lặp.</small>
		</div>
		<div class="row mb-3">
			<div class="col-auto">
				<div class="form-check form-switch">
					<input class="form-check-input" type="checkbox" asp-for="NoIndex" role="switch" id="seoNoIndexSwitch">
					<label class="form-check-label" asp-for="NoIndex"></label>
				</div>
				<small class="form-hint d-block ms-4 ps-2">Ngăn máy tìm kiếm index trang này.</small>
			</div>
			<div class="col-auto">
				<div class="form-check form-switch">
					<input class="form-check-input" type="checkbox" asp-for="NoFollow" role="switch" id="seoNoFollowSwitch">
					<label class="form-check-label" asp-for="NoFollow"></label>
				</div>
				<small class="form-hint d-block ms-4 ps-2">Ngăn máy tìm kiếm theo các liên kết trên trang.</small>
			</div>
		</div>
		<hr class="my-3" />
		<h4 class="mb-3">Open Graph (Mạng xã hội)</h4>
		<div class="form-group mb-3">
			<label asp-for="OgTitle" class="form-label"></label>
			<input asp-for="OgTitle" class="form-control" />
			<span asp-validation-for="OgTitle" class="text-danger"></span>
		</div>
		<div class="form-group mb-3">
			<label asp-for="OgDescription" class="form-label"></label>
			<textarea asp-for="OgDescription" class="form-control" rows="2"></textarea>
			<span asp-validation-for="OgDescription" class="text-danger"></span>
		</div>
		<div class="form-group mb-3">
			<label asp-for="OgImage" class="form-label"></label>
			<div class="input-group">
				<input type="text" asp-for="OgImage" class="form-control" id="ogImagePathInput" /> @* Stores URL *@
				<button class="btn btn-outline-secondary" type="button" id="selectOgImageBtn" title="Chọn ảnh từ thư viện"> <i class="ti ti-photo-search"></i> </button>
				<button class="btn btn-outline-danger @(string.IsNullOrEmpty(Model.OgImage) ? "d-none" : "")" type="button" id="removeOgImageBtn" title="Xóa ảnh"> <i class="ti ti-x"></i> </button>
			</div>
			<span asp-validation-for="OgImage" class="text-danger"></span>
			<img id="ogImagePreview" src="@(Model.OgImage ?? "/img/placeholder.svg")" class="img-thumbnail mt-2" style="max-height: 80px; display: @(string.IsNullOrEmpty(Model.OgImage) ? "none" : "block");" />
		</div>
		<div class="form-group mb-3">
			<label asp-for="OgType" class="form-label"></label>
			<input asp-for="OgType" class="form-control" />
			<span asp-validation-for="OgType" class="text-danger"></span>
		</div>
		<hr class="my-3" />
		<h4 class="mb-3">Twitter Card</h4>
		@* Add Twitter fields similarly *@
		<hr class="my-3" />
		<h4 class="mb-3">Dữ liệu cấu trúc (Schema & Breadcrumbs)</h4>
		@* Add Schema/Breadcrumb fields similarly *@
		<hr class="my-3" />
		<h4 class="mb-3">Cấu hình Sitemap</h4>
		<div class="row">
			<div class="col-md-6">
				<div class="form-group mb-3">
					<label asp-for="SitemapPriority" class="form-label"></label>
					<input type="number" asp-for="SitemapPriority" class="form-control" step="0.1" min="0.0" max="1.0" />
					<span asp-validation-for="SitemapPriority" class="text-danger"></span>
				</div>
			</div>
			<div class="col-md-6">
				<div class="form-group mb-3">
					<label asp-for="SitemapChangeFrequency" class="form-label required"></label>
					<select asp-for="SitemapChangeFrequency" class="form-select tom-select-single"> @* Use TomSelect *@
						<option value="">-- Chọn tần suất --</option>
						<option value="always">Luôn luôn (Always)</option>
						<option value="hourly">Hàng giờ (Hourly)</option>
						<option value="daily">Hàng ngày (Daily)</option>
						<option value="weekly">Hàng tuần (Weekly)</option>
						<option value="monthly">Hàng tháng (Monthly)</option>
						<option value="yearly">Hàng năm (Yearly)</option>
						<option value="never">Không bao giờ (Never)</option>
					</select>
					<span asp-validation-for="SitemapChangeFrequency" class="text-danger"></span>
				</div>
			</div>
		</div>
	</div>
</div>
@* --- END OF FILE _ProductFormFields.txt --- *@


@* --- START OF FILE Create.txt --- Updated --- *@
@using web.Areas.Admin.ViewModels.Product
@model ProductViewModel
@{
	ViewData["Title"] = "Thêm Sản phẩm mới - Hệ thống quản trị";
    // Breadcrumbs set in Controller
}

@section Styles {
	@* Include TomSelect CSS if not global *@
	@* <link rel="stylesheet" href="~/lib/tom-select/dist/css/tom-select.bootstrap5.min.css" asp-append-version="true"> *@
	<style>
		.product-image-item .card-img-top { min-height: 150px; background-color: #f8f9fa; background-size: contain; background-repeat: no-repeat; background-position: center; }
		.product-variant-item .form-label { font-size: 0.8rem; margin-bottom: 0.25rem; }
		.ts-control .item[data-value=""] { display: none; } /* Hide empty option in TomSelect */
	</style>
}

<form asp-action="Create" method="post" id="product-form">
	<div class="card">
		<div class="card-header">
			<h3 class="card-title">Thông tin Sản phẩm</h3>
		</div>
		<div class="card-body">
			@Html.AntiForgeryToken()
			<div asp-validation-summary="ModelOnly" class="text-danger alert alert-danger mb-3"></div>
			<partial name="_ProductFormFields" model="Model" />
		</div>
		<div class="card-footer text-end bg-transparent border-top pt-3">
			<a asp-action="Index" class="btn btn-link">Hủy</a>
			<button type="submit" class="btn btn-primary ms-2">
				<i class="ti ti-device-floppy me-2"></i> Lưu Sản phẩm
			</button>
		</div>
	</div>
</form>

@section Scripts {
	<script src="~/js/slug.js" asp-append-version="true"></script>
	@* Ensure Summernote is loaded if not global *@
	@* <script src="~/lib/summernote/dist/summernote-bs5.min.js" asp-append-version="true"></script> *@
    @* Ensure TomSelect is loaded if not global *@
    @* <script src="~/lib/tom-select/dist/js/tom-select.complete.min.js" asp-append-version="true"></script> *@

	<script>
		$(document).ready(function () {
             // --- Initialize TomSelect ---
             // Use a more specific selector if needed
             document.querySelectorAll('.tom-select-single').forEach((el) => {
                new TomSelect(el, { create: false, placeholder: el.options[0].text || 'Chọn...' });
             });
            document.querySelectorAll('.tom-select-multiple').forEach((el) => {
                 new TomSelect(el, { plugins: ['remove_button'], create: false, placeholder: 'Chọn...' });
            });

			// --- Initialize Summernote ---
			$('.summernote-editor').summernote({
                lang: 'vi-VN',
                height: 200, // Default height, can override per element
                toolbar: [ /* Standard Toolbar */
                    ['style', ['style']],
                    ['font', ['bold', 'italic', 'underline', 'clear']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['insert', ['link', 'picture']], // Remove video if not needed
                    ['view', ['fullscreen', 'codeview', 'help']]
                ]
                // Add image upload callbacks if needed
            });
             $('#productDescriptionEditor').summernote('height', 350); // Specific height for main description


			// --- Slug Generation ---
			const nameInput = $('#productName');
			const slugInput = $('#productSlug');
			const generateBtn = $('#generateSlugButton');
			nameInput.on('input', function() {
				if (slugInput.val() === '') { slugInput.val(generateVietnameseSlug(nameInput.val())); }
			});
			generateBtn.on('click', function() { slugInput.val(generateVietnameseSlug(nameInput.val())); });


			// --- Image Management ---
            const selectImagesBtn = $('#selectProductImagesBtn');
            const imageListContainer = $('#product-image-list');
            const imageTemplate = $('#product-image-template').html();
            let imageNextIndex = imageListContainer.find('.product-image-item').length; // Should be 0 on Create

            function reindexImages() {
                let isAnyMainSet = false;
                imageListContainer.find('.product-image-item').each(function(index) {
                    $(this).attr('data-index', index);
                    $(this).find('input, label, select, textarea').each(function() {
                        const oldName = $(this).attr('name');
                        if (oldName) { $(this).attr('name', oldName.replace(/Images\[\d+\]/, `Images[${index}]`)); }
                        const oldId = $(this).attr('id');
                        if (oldId) {
                            const newId = oldId.replace(/Images___?\d+__?/, `Images_${index}_`);
                             $(this).attr('id', newId);
                            if ($(this).is(':radio') || $(this).is(':checkbox')) {
                                $(this).closest('.form-check').find('label').attr('for', newId);
                            }
                        }
                         if($(this).hasClass('is-main-input')) { $(this).attr('name', `ImagesMainRadio`).val(index); } // Use index as value
                    });
                    if ($(this).find('.is-main-input').is(':checked')) { isAnyMainSet = true; }
                });
                 imageNextIndex = imageListContainer.find('.product-image-item').length;
                updateMainImageHiddenInput(); // Ensure hidden inputs are synced
                 // Auto-select first as main if none selected
                if (!isAnyMainSet && imageListContainer.find('.product-image-item:visible').length > 0) {
                     const firstRadio = imageListContainer.find('.product-image-item:visible:first .is-main-input');
                     firstRadio.prop('checked', true);
                     updateMainImageHiddenInput();
                }
            }

            function updateMainImageHiddenInput() {
                imageListContainer.find('.product-image-item').each(function(index) {
                     const radio = $(this).find('.is-main-input');
                     const hidden = $(this).find('.is-main-hidden');
                    // Check based on value matching current index
                     hidden.val(radio.is(':checked') ? 'true' : 'false');
                });
            }

            imageListContainer.on('change', '.is-main-input', function() {
                 // When a radio is checked, its value (the index) identifies the main image
                 // All hidden inputs need to be updated
                 updateMainImageHiddenInput();
            });

            function handleProductImageSelection(selectedFile) {
                if (!selectedFile || !selectedFile.path) return;
                const imagePath = selectedFile.path;
                 let existingItem = null;
                imageListContainer.find('.product-image-item').each(function() { if ($(this).data('image-path') === imagePath) { existingItem = $(this); return false; } });
                 if (existingItem && existingItem.length > 0) {
                    if (existingItem.find('.is-deleted-input').val() === 'true') {
                        existingItem.find('.is-deleted-input').val('false'); existingItem.show(); reindexImages(); toastr.info(`Đã thêm lại ảnh '${selectedFile.name}'.`);
                    } else { toastr.warning(`Ảnh '${selectedFile.name}' đã có trong danh sách.`); } return;
                }
                 // Add New
                const displayUrl = `@(MediaUrlHelper.GetMinioUrl("__PATH__", "/img/placeholder.svg"))`.replace('__PATH__', imagePath);
                const newItemHtml = imageTemplate
                    .replace(/__INDEX__/g, imageNextIndex)
                    .replace(/__IMAGE_PATH__/g, imagePath)
                    .replace(/__THUMB_PATH__/g, imagePath) // Assuming thumb=image for now
                    .replace(/__DISPLAY_URL__/g, displayUrl)
                    .replace(/__ALT_TEXT__/g, selectedFile.alt || '');
                const newItem = $(newItemHtml);
                 imageListContainer.append(newItem);
                 imageNextIndex++; reindexImages(); toastr.success(`Đã thêm ảnh '${selectedFile.name}'.`);
            }

            selectImagesBtn.on('click', function() {
                if (typeof window.openMediaSelectionModal === 'function') {
                    window.openMediaSelectionModal(handleProductImageSelection, 'Image', true); // Allow multi-select
                } else { console.error('openMediaSelectionModal is not defined.'); alert('Không thể mở thư viện media.'); }
            });

            imageListContainer.on('click', '.remove-image-btn', function() {
                 const item = $(this).closest('.product-image-item');
                 const idInput = item.find('input[name$=".Id"]');
                if (idInput && parseInt(idInput.val()) > 0) { item.find('.is-deleted-input').val('true'); item.hide();
                } else { item.remove(); }
                reindexImages(); // Re-index after remove/hide
            });
             // Initial indexing
            reindexImages();

			// --- Variant Management ---
			const variantListContainer = $('#product-variant-list');
			const variantTemplate = $('#product-variant-template').html();
			let variantNextIndex = $('#product-variant-list .product-variant-item').length;

			 function reindexVariants() {
				variantListContainer.find('.product-variant-item').each(function(index) {
					$(this).attr('data-index', index);
					$(this).find('input, label, select, textarea').each(function() {
						 const oldName = $(this).attr('name'); if (oldName) $(this).attr('name', oldName.replace(/Variants\[\d+\]/, `Variants[${index}]`));
						 const oldId = $(this).attr('id'); if (oldId) { const newId = oldId.replace(/Variants___?\d+__?/, `Variants_${index}_`); $(this).attr('id', newId); if ($(this).is(':checkbox')) $(this).closest('.form-check').find('label').attr('for', newId); }
					});
				}); variantNextIndex = variantListContainer.find('.product-variant-item').length;
			}

			 $('#add-variant-btn').on('click', function() {
				 const newItemHtml = variantTemplate.replace(/__INDEX__/g, variantNextIndex); variantListContainer.append(newItemHtml); reindexVariants();
			 });

			 variantListContainer.on('click', '.remove-variant-btn', function() {
				 const item = $(this).closest('.product-variant-item'); const idInput = item.find('input[name$=".Id"]');
				  if (idInput && parseInt(idInput.val()) > 0) { item.find('.is-deleted-input').val('true'); item.hide(); } else { item.remove(); }
				 reindexVariants();
			 });
            // Initial indexing
            reindexVariants();

			 // --- OG/Twitter Image Selection ---
			 function setupOgImageSelection() {
                 const ogInput = $('#ogImagePathInput');
                 if(!ogInput.length) return; // Only run if the field exists
                 const selectBtn = $('#selectOgImageBtn');
                 const removeBtn = $('#removeOgImageBtn');
                 const preview = $('#ogImagePreview');
                 const defaultSrc = '/img/placeholder.svg';

                  function handleSelection(selectedFile) {
                     if (selectedFile && selectedFile.url) { preview.attr('src', selectedFile.url).show(); ogInput.val(selectedFile.url).trigger('change'); removeBtn.removeClass('d-none');
                    } else { preview.attr('src', defaultSrc).hide(); ogInput.val('').trigger('change'); removeBtn.addClass('d-none'); }
                 }
                  selectBtn.on('click', function() { if (typeof window.openMediaSelectionModal === 'function') window.openMediaSelectionModal(handleSelection, 'Image'); });
                 removeBtn.on('click', function() { preview.attr('src', defaultSrc).hide(); ogInput.val('').trigger('change'); $(this).addClass('d-none'); });
                 if (ogInput.val() === '') preview.hide(); else preview.show();
             }
              setupOgImageSelection(); // Initialize
             // Add similar setup for Twitter image if needed

			 // Trigger validation re-parse if using jQuery unobtrusive
			 // $.validator.unobtrusive.parse("#product-form");
		});
	</script>
	@{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
@* --- END OF FILE Create.txt --- *@


@* --- START OF FILE Edit.txt --- Updated --- *@
@using shared.Helpers
@using web.Areas.Admin.ViewModels.Product
@model ProductViewModel
@{
	ViewData["Title"] = "Chỉnh sửa Sản phẩm - Hệ thống quản trị";
    // Breadcrumbs set in Controller
}

@section Styles {
	@* <link rel="stylesheet" href="~/lib/tom-select/dist/css/tom-select.bootstrap5.min.css" asp-append-version="true"> *@
	<style>
		.product-image-item .card-img-top { min-height: 150px; background-color: #f8f9fa; background-size: contain; background-repeat: no-repeat; background-position: center; }
		.product-variant-item .form-label { font-size: 0.8rem; margin-bottom: 0.25rem; }
        .ts-control .item[data-value=""] { display: none; }
	</style>
}

<form asp-action="Edit" asp-route-id="@Model.Id" method="post" id="product-form">
	<div class="card">
		<div class="card-header">
			<h3 class="card-title">Thông tin Sản phẩm</h3>
		</div>
		<div class="card-body">
            @Html.AntiForgeryToken()
			<input type="hidden" asp-for="Id" />
			<div asp-validation-summary="ModelOnly" class="text-danger alert alert-danger mb-3"></div>
			<partial name="_ProductFormFields" model="Model" />
		</div>
		<div class="card-footer text-end bg-transparent border-top pt-3">
			<a asp-action="Index" class="btn btn-link">Hủy</a>
			<button type="submit" class="btn btn-primary ms-2">
				<i class="ti ti-device-floppy me-2"></i> Cập nhật Sản phẩm
			</button>
		</div>
	</div>
</form>

@section Scripts {
	<script src="~/js/slug.js" asp-append-version="true"></script>
	@* <script src="~/lib/summernote/dist/summernote-bs5.min.js" asp-append-version="true"></script> *@
    @* <script src="~/lib/tom-select/dist/js/tom-select.complete.min.js" asp-append-version="true"></script> *@

	<script>
        // Duplicating the JS from Create view - consider moving common parts to a separate JS file
		$(document).ready(function () {
			 // --- Initialize TomSelect ---
             document.querySelectorAll('.tom-select-single').forEach((el) => { new TomSelect(el, { create: false, placeholder: el.options[0].text || 'Chọn...' }); });
             document.querySelectorAll('.tom-select-multiple').forEach((el) => { new TomSelect(el, { plugins: ['remove_button'], create: false, placeholder: 'Chọn...' }); });

			// --- Initialize Summernote ---
			$('.summernote-editor').summernote({ lang: 'vi-VN', height: 200, toolbar: [ ['style', ['style']], ['font', ['bold', 'italic', 'underline', 'clear']], ['para', ['ul', 'ol', 'paragraph']], ['insert', ['link', 'picture']], ['view', ['fullscreen', 'codeview', 'help']] ] });
            $('#productDescriptionEditor').summernote('height', 350);

			// --- Slug Generation (Edit) ---
			const nameInput = $('#productName'); const slugInput = $('#productSlug'); const generateBtn = $('#generateSlugButton');
			let userModifiedSlug = (slugInput.val() !== generateVietnameseSlug(nameInput.val())) && (slugInput.val() !== '');
			slugInput.on('input', function() { userModifiedSlug = true; });
			nameInput.on('input', function() { if (!userModifiedSlug) { slugInput.val(generateVietnameseSlug(nameInput.val())); } });
			generateBtn.on('click', function() { userModifiedSlug = false; slugInput.val(generateVietnameseSlug(nameInput.val())); });

			// --- Image Management ---
            const selectImagesBtn = $('#selectProductImagesBtn'); const imageListContainer = $('#product-image-list'); const imageTemplate = $('#product-image-template').html();
            let imageNextIndex = imageListContainer.find('.product-image-item').length;

            function reindexImages() { /* ... (Same as Create view) ... */
                let isAnyMainSet = false;
                imageListContainer.find('.product-image-item').each(function(index) {
                    $(this).attr('data-index', index);
                    $(this).find('input, label, select, textarea').each(function() {
                        const oldName = $(this).attr('name'); if (oldName) $(this).attr('name', oldName.replace(/Images\[\d+\]/, `Images[${index}]`));
                        const oldId = $(this).attr('id'); if (oldId) { const newId = oldId.replace(/Images___?\d+__?/, `Images_${index}_`); $(this).attr('id', newId); if ($(this).is(':radio') || $(this).is(':checkbox')) $(this).closest('.form-check').find('label').attr('for', newId); }
                         if($(this).hasClass('is-main-input')) { $(this).attr('name', `ImagesMainRadio`).val(index); } // Use index as value
                    });
                    if ($(this).find('.is-main-input').is(':checked')) { isAnyMainSet = true; }
                }); imageNextIndex = imageListContainer.find('.product-image-item').length;
                updateMainImageHiddenInput();
                 if (!isAnyMainSet && imageListContainer.find('.product-image-item:visible').length > 0) { const firstRadio = imageListContainer.find('.product-image-item:visible:first .is-main-input'); firstRadio.prop('checked', true); updateMainImageHiddenInput(); }
            }
            function updateMainImageHiddenInput() { /* ... (Same as Create view) ... */
                 imageListContainer.find('.product-image-item').each(function(index) { const radio = $(this).find('.is-main-input'); const hidden = $(this).find('.is-main-hidden'); hidden.val(radio.is(':checked') ? 'true' : 'false'); });
            }
            imageListContainer.on('change', '.is-main-input', function() { updateMainImageHiddenInput(); });
            function handleProductImageSelection(selectedFile) { /* ... (Same as Create view) ... */
                 if (!selectedFile || !selectedFile.path) return; const imagePath = selectedFile.path; let existingItem = null;
                imageListContainer.find('.product-image-item').each(function() { if ($(this).data('image-path') === imagePath) { existingItem = $(this); return false; } });
                 if (existingItem && existingItem.length > 0) { if (existingItem.find('.is-deleted-input').val() === 'true') { existingItem.find('.is-deleted-input').val('false'); existingItem.show(); reindexImages(); toastr.info(`Đã thêm lại ảnh '${selectedFile.name}'.`); } else { toastr.warning(`Ảnh '${selectedFile.name}' đã có trong danh sách.`); } return; }
                 const displayUrl = `@(MediaUrlHelper.GetMinioUrl("__PATH__", "/img/placeholder.svg"))`.replace('__PATH__', imagePath);
                const newItemHtml = imageTemplate.replace(/__INDEX__/g, imageNextIndex).replace(/__IMAGE_PATH__/g, imagePath).replace(/__THUMB_PATH__/g, imagePath).replace(/__DISPLAY_URL__/g, displayUrl).replace(/__ALT_TEXT__/g, selectedFile.alt || '');
                const newItem = $(newItemHtml); imageListContainer.append(newItem); imageNextIndex++; reindexImages(); toastr.success(`Đã thêm ảnh '${selectedFile.name}'.`);
            }
            selectImagesBtn.on('click', function() { if (typeof window.openMediaSelectionModal === 'function') window.openMediaSelectionModal(handleProductImageSelection, 'Image', true); else { console.error('openMediaSelectionModal is not defined.'); alert('Không thể mở thư viện media.'); } });
            imageListContainer.on('click', '.remove-image-btn', function() { /* ... (Same as Create view) ... */
                 const item = $(this).closest('.product-image-item'); const idInput = item.find('input[name$=".Id"]');
                 if (idInput && parseInt(idInput.val()) > 0) { item.find('.is-deleted-input').val('true'); item.hide(); } else { item.remove(); }
                reindexImages();
            });
            reindexImages(); // Initial index on load

			// --- Variant Management ---
			const variantListContainer = $('#product-variant-list'); const variantTemplate = $('#product-variant-template').html(); let variantNextIndex = $('#product-variant-list .product-variant-item').length;
			 function reindexVariants() { /* ... (Same as Create view) ... */
                 variantListContainer.find('.product-variant-item').each(function(index) { $(this).attr('data-index', index); $(this).find('input, label, select, textarea').each(function() { const oldName = $(this).attr('name'); if (oldName) $(this).attr('name', oldName.replace(/Variants\[\d+\]/, `Variants[${index}]`)); const oldId = $(this).attr('id'); if (oldId) { const newId = oldId.replace(/Variants___?\d+__?/, `Variants_${index}_`); $(this).attr('id', newId); if ($(this).is(':checkbox')) $(this).closest('.form-check').find('label').attr('for', newId); } }); }); variantNextIndex = variantListContainer.find('.product-variant-item').length;
             }
			 $('#add-variant-btn').on('click', function() { const newItemHtml = variantTemplate.replace(/__INDEX__/g, variantNextIndex); variantListContainer.append(newItemHtml); reindexVariants(); });
			 variantListContainer.on('click', '.remove-variant-btn', function() { /* ... (Same as Create view) ... */
                 const item = $(this).closest('.product-variant-item'); const idInput = item.find('input[name$=".Id"]'); if (idInput && parseInt(idInput.val()) > 0) { item.find('.is-deleted-input').val('true'); item.hide(); } else { item.remove(); } reindexVariants();
             });
             reindexVariants(); // Initial index

			 // --- OG/Twitter Image Selection ---
             function setupOgImageSelection() { /* ... (Same as Create view) ... */
                  const ogInput = $('#ogImagePathInput'); if(!ogInput.length) return; const selectBtn = $('#selectOgImageBtn'); const removeBtn = $('#removeOgImageBtn'); const preview = $('#ogImagePreview'); const defaultSrc = '/img/placeholder.svg';
                  function handleSelection(selectedFile) { if (selectedFile && selectedFile.url) { preview.attr('src', selectedFile.url).show(); ogInput.val(selectedFile.url).trigger('change'); removeBtn.removeClass('d-none'); } else { preview.attr('src', defaultSrc).hide(); ogInput.val('').trigger('change'); removeBtn.addClass('d-none'); } }
                  selectBtn.on('click', function() { if (typeof window.openMediaSelectionModal === 'function') window.openMediaSelectionModal(handleSelection, 'Image'); }); removeBtn.on('click', function() { preview.attr('src', defaultSrc).hide(); ogInput.val('').trigger('change'); $(this).addClass('d-none'); });
                 if (ogInput.val() === '') preview.hide(); else preview.show();
             }
             setupOgImageSelection();

			 // $.validator.unobtrusive.parse("#product-form");
		});
	</script>
	@{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
@* --- END OF FILE Edit.txt --- *@


@* --- START OF FILE Index.txt --- Updated --- *@
@using X.PagedList.Mvc.Core
@using X.PagedList
@using X.PagedList.Web.Common
@using shared.Helpers
@using web.Areas.Admin.ViewModels.Product
@using shared.Enums
@using shared.Extensions // For GetDisplayName
@model IPagedList<ProductListItemViewModel> @* Changed model type *@
@{
	ViewData["Title"] = "Quản lý Sản phẩm - Hệ thống quản trị";
     // Breadcrumbs set in Controller
}

@section PageActions {
	<div class="btn-list">
		<a asp-action="Create" class="btn btn-primary d-sm-inline-block">
			<i class="ti ti-plus me-2"></i> Thêm sản phẩm
		</a>
         @* Optional Export Button *@
        @* <a asp-action="ExportExcel" asp-all-route-data="@Context.Request.Query.ToDictionary(q => q.Key, q=> q.Value.ToString())" class="btn btn-outline-success d-none d-sm-inline-block"> <i class="ti ti-download me-2"></i> Export Excel </a> *@
	</div>
}

<div class="card mb-3">
	<div class="card-body">
		<form method="get" asp-action="Index" id="filterForm" class="row g-3 align-items-end">
			<div class="col-md-3 col-sm-6">
				<label class="form-label">Tìm kiếm</label>
                 <div class="input-group">
				    <input type="text" class="form-control" placeholder="Tên sản phẩm..." name="searchTerm" value="@ViewBag.SearchTerm">
                     <button class="btn btn-icon" type="button" id="clearSearch" title="Xóa tìm kiếm"> <i class="ti ti-x"></i> </button>
                 </div>
			</div>
			<div class="col-md-2 col-sm-6">
				<label class="form-label">Loại SP</label>
                @* Use SelectTagHelper for easier selection handling *@
				<select class="form-select tom-select-filter" name="typeId" asp-items="@ViewBag.ProductTypes">
					<option value="">-- Tất cả --</option>
				</select>
			</div>
			<div class="col-md-2 col-sm-6">
				<label class="form-label">Thương hiệu</label>
				<select class="form-select tom-select-filter" name="brandId" asp-items="@ViewBag.Brands">
					<option value="">-- Tất cả --</option>
				</select>
			</div>
			<div class="col-md-2 col-sm-6">
				<label class="form-label">Trạng thái</label>
				<select class="form-select tom-select-filter" name="status" asp-items="@ViewBag.Statuses">
					<option value="">-- Tất cả --</option>
				</select>
			</div>
			<div class="col-md-1 col-sm-6">
				<button type="submit" class="btn btn-primary w-100"> <i class="ti ti-filter me-1 d-none d-sm-inline-block"></i> Lọc</button>
			</div>
			<div class="col-md-2 col-sm-6">
				<a asp-action="Index" class="btn btn-outline-secondary w-100"> <i class="ti ti-reload me-1 d-none d-sm-inline-block"></i> Đặt lại</a>
			</div>
		</form>
	</div>
</div>

<!-- Content Table -->
<div class="card">
	 <div class="card-header d-flex justify-content-between align-items-center">
        <h3 class="card-title mb-0">Danh sách Sản phẩm</h3>
         @if (Model.TotalItemCount > 0)
		{
			<small class="text-muted">Hiển thị @Model.FirstItemOnPage-@Model.LastItemOnPage trên tổng số @Model.TotalItemCount sản phẩm</small>
		}
    </div>
	<div class="card-body p-0">
		@if (!Model.Any())
		{
			<div class="empty">
				<div class="empty-icon"><i class="ti ti-package fa-3x text-muted"></i></div>
				<p class="empty-title">Không tìm thấy sản phẩm nào</p>
				<p class="empty-subtitle text-muted">Hãy thêm sản phẩm đầu tiên.</p>
				<div class="empty-action">
					<a asp-action="Create" class="btn btn-primary"> <i class="ti ti-plus me-2"></i> Thêm sản phẩm </a>
				</div>
			</div>
		}
		else
		{
			<div class="table-responsive">
				<table class="table table-vcenter card-table table-striped">
					<thead>
						<tr>
							<th class="w-1">ID</th>
							<th style="width: 7%;">Ảnh</th>
							<th>Tên sản phẩm</th>
							<th>Loại</th>
							<th>Thương hiệu</th>
							<th class="text-center">Trạng thái</th>
							<th class="text-center">Nổi bật</th>
							<th class="text-center">Lượt xem</th>
                             <th style="width: 15%;">Cập nhật</th> @* Added UpdatedAt *@
							<th class="w-1">Thao tác</th>
						</tr>
					</thead>
					<tbody>
						@foreach (var item in Model)
						{
							<tr>
								<td class="text-muted">@item.Id</td>
								<td>
									<img src="@MediaUrlHelper.GetMinioUrl(item.MainImageUrl, "/img/placeholder.svg")"
										 alt="@item.Name"
										 class="avatar avatar-md object-fit-contain bg-light" /> @* Added bg-light *@
								</td>
								<td>
									<a asp-action="Edit" asp-route-id="@item.Id" class="fw-medium">@item.Name</a>
                                    @* Optional: <small class="text-muted d-block">@item.ShortDescription</small> *@
								</td>
								<td class="text-muted">@(item.ProductTypeName ?? "-")</td>
								<td class="text-muted">@(item.BrandName ?? "-")</td>
								<td class="text-center">
                                     @* Use GetDisplayName extension *@
									<span class="badge bg-@(GetStatusBadgeClass(item.Status))-lt">@item.Status.GetDisplayName()</span>
								</td>
								<td class="text-center">
									@if (item.IsFeatured) { <i class="ti ti-star-filled text-warning" title="Nổi bật"></i> }
                                    else { <i class="ti ti-star text-muted" title="Không nổi bật"></i> }
								</td>
								<td class="text-center text-muted">@item.ViewCount</td>
                                <td class="text-muted" title="@item.UpdatedAt?.ToString("dd/MM/yyyy HH:mm:ss")">@item.UpdatedAt?.ToString("dd/MM/yyyy HH:mm")</td> @* Show UpdatedAt *@
								<td>
									<div class="btn-list flex-nowrap">
										<a asp-action="Edit" asp-route-id="@item.Id" class="btn btn-sm btn-icon btn-outline-primary" title="Chỉnh sửa">
											<i class="ti ti-pencil"></i>
										</a>
										<button class="btn btn-sm btn-icon btn-outline-danger delete-item-btn"
												title="Xóa"
												data-id="@item.Id"
												data-name="@item.Name"
												data-delete-url="@Url.Action("Delete", "Product")">
											<i class="ti ti-trash"></i>
										</button>
									</div>
								</td>
							</tr>
						}
					</tbody>
				</table>
			</div>
		}
	</div>
     @* Pagination Footer *@
	@if (Model.PageCount > 1)
	{
		<div class="card-footer">
			<div class="d-flex align-items-center justify-content-center">
                 @Html.PagedListPager(
								Model,
								page => Url.Action("Index", new {
                                    page,
                                    searchTerm = ViewBag.SearchTerm,
                                    typeId = ViewBag.SelectedTypeId,
                                    brandId = ViewBag.SelectedBrandId,
                                    status = ViewBag.SelectedStatus
                                    }), // Pass all filters
								new PagedListRenderOptions
                                {
                                    UlElementClasses = new[] { "pagination mb-0" },
                                    LiElementClasses = new[] { "page-item" },
                                    PageClasses = new[] { "page-link" },
                                    DisplayLinkToFirstPage = PagedListDisplayMode.IfNeeded,
                                    DisplayLinkToLastPage = PagedListDisplayMode.IfNeeded,
                                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                                })
			</div>
		</div>
	}
</div>

@* Status Badge Helper *@
@functions {
    string GetStatusBadgeClass(PublishStatus status) => status switch {
        PublishStatus.Published => "success", PublishStatus.Draft => "secondary", PublishStatus.Scheduled => "warning",
        PublishStatus.Archived => "dark", _ => "secondary"
    };
}

@section Scripts {
	<script>
		$(document).ready(function () {
			$('#clearSearch').on('click', function() {
				$('input[name="searchTerm"]').val('');
                // Optional: $('#filterForm').submit();
			});
            // Initialize TomSelect for filters
            document.querySelectorAll('.tom-select-filter').forEach((el) => {
                 new TomSelect(el, { create: false, allowEmptyOption: true });
            });
            // Global delete script handles .delete-item-btn
		});
	</script>
 }
@* --- END OF FILE Index.txt --- *@
```

**Summary of Product Refactoring:**

1.  **Entities/Configurations:** Corrected inheritance for `ProductVariantConfiguration` and join table configurations (`ProductCategoryConfiguration`, `ProductTagConfiguration`). Ensured necessary DbSets and configurations are applied in `ApplicationDbContext`.
2.  **ViewModels:** Added `[Display]` attributes. Implemented `ISeoPropertiesViewModel`. Added `UpdatedAt` to `ListItemViewModel`.
3.  **Mappers:** Updated `ListItemViewModel` mapping. Verified other mappings, ensuring collections/navigation properties are ignored where handled manually. Added `ProductTypeProfile`.
4.  **Validators:** Injected `DbContext` for unique slug check in `ProductViewModelValidator`. Added unique slug check to `ProductTypeViewModelValidator`. Noted complexity of cross-product unique SKU validation (basic checks implemented in controller). Included `SeoPropertiesValidator`.
5.  **Controller:**
    *   Implemented standard patterns: pagination, logging, `TempData`, validation middleware, `AsNoTracking`, `ProjectTo`.
    *   Implemented complex relationship handling for Create/Edit POST actions (Categories, Tags, Images, Variants).
    *   Added manual checks for complex validation (Main Image selection, unique SKUs) *before* `ModelState.IsValid`.
    *   Added standard `Delete` action with basic checks.
    *   Refined helper methods (`LoadRelatedDataForFormAsync`, `LoadFilterDropdownsAsync`).
6.  **Views:**
    *   `_ProductFormFields`: Added IDs, standard SEO fields.
    *   `Create`/`Edit`: Implemented standard card structure, added necessary JS for TomSelect, Summernote, Slug, and the complex Image/Variant list management. Added validation scripts partial.
    *   `Index`: Implemented pagination, item count, standard filter/delete controls, used `MediaUrlHelper`, added `UpdatedAt` column. Initialized TomSelect for filters.

This is a large and complex feature. Thorough testing of the relationship handling (especially the add/update/delete logic for Images and Variants in the `Edit` POST action) and the client-side JavaScript is highly recommended. Remember to handle the actual MinIO image deletion appropriately (likely after successful DB deletion).