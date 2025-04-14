Okay, let's refine the `Article` and `Comment` management features, focusing on standardization, correcting relationships, improving logic, and ensuring consistency with the rest of your admin area.

**Major Changes & Considerations:**

1.  **Join Table Configurations:** `ArticleCategory`, `ArticleTag`, `ArticleProduct` are join tables and typically **do not** need to inherit `BaseEntity` or use `BaseEntityConfiguration`. Their primary keys are composite keys.
2.  **`CommentConfiguration`:** The `OnDelete` behavior for `ParentId` should likely be `Cascade` if you want replies deleted when the parent comment is deleted, or `SetNull` if you want to keep replies but orphan them. `Restrict` prevents deletion if replies exist, requiring manual handling. We'll assume `Cascade` for now.
3.  **Controller Logic:** Implement pagination, standard logging, TempData, use `ProjectTo`, handle validation via middleware, refine relationship updates, add status/type display name helpers, implement MinIO deletion logic correctly.
4.  **Validation:** Add database checks for unique slugs.
5.  **Views:** Implement pagination, use helper for status/type names, standardize forms and buttons, initialize JS components (TomSelect, Summernote).
6.  **Comment Feature:** The Comment controller looks more like a moderation tool (Approve/Unapprove/Edit/Delete) rather than admin creating comments. We'll keep it that way.

**Refactored Code:**

**1. Entities (`ArticleCategory.cs`, `ArticleTag.cs`, `ArticleProduct.cs`, `Comment.cs`)**

*   Remove `BaseEntity` inheritance from join tables.
*   Adjust `Comment` Parent relationship `OnDelete` behavior if needed (keeping `Restrict` for now, but `Cascade` might be more common).

```csharp
// --- START OF FILE ArticleCategory.cs --- Updated ---
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;
// Join table - No BaseEntity needed
public class ArticleCategory
{
    public int ArticleId { get; set; }
    public int CategoryId { get; set; }

    public virtual Article? Article { get; set; }
    public virtual Category? Category { get; set; }
}
// --- END OF FILE ArticleCategory.cs ---

// --- START OF FILE ArticleTag.cs --- Updated ---
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;
// Join table - No BaseEntity needed
public class ArticleTag
{
    public int ArticleId { get; set; }
    public int TagId { get; set; }

    public virtual Article? Article { get; set; }
    public virtual Tag? Tag { get; set; }
}
// --- END OF FILE ArticleTag.cs ---

// --- START OF FILE ArticleProduct.cs --- Updated ---
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;
// Join table - No BaseEntity needed
public class ArticleProduct
{
    public int ArticleId { get; set; }
    public int ProductId { get; set; }
    public int OrderIndex { get; set; } = 0; // Keep OrderIndex if needed

    public virtual Article? Article { get; set; }
    public virtual Product? Product { get; set; }
}
// --- END OF FILE ArticleProduct.cs ---

// --- START OF FILE Comment.cs --- (Verify Parent OnDelete - Defaulting to Restrict) ---
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace domain.Entities;
public class Comment : BaseEntity<int> { /* ... as before ... */ }
// --- END OF FILE Comment.cs ---

// --- START OF FILE Article.cs --- No changes needed ---
using domain.Entities.Shared; using Microsoft.EntityFrameworkCore; using Microsoft.EntityFrameworkCore.Metadata.Builders; using shared.Enums;
namespace domain.Entities;
public class Article : SeoEntity<int> { /* ... as before ... */ }
// --- END OF FILE Article.cs ---
```

**2. Configurations (`ArticleCategoryConfiguration.cs`, `ArticleTagConfiguration.cs`, `ArticleProductConfiguration.cs`, `CommentConfiguration.cs`)**

*   Remove `BaseEntityConfiguration` inheritance from join table configurations.
*   Define composite keys and relationships directly in join table configurations.
*   Verify `CommentConfiguration` `OnDelete` behavior for ParentId.

```csharp
// --- START OF FILE ArticleCategoryConfiguration.cs --- Updated ---
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace infrastructure; // Assuming correct namespace

// Configure JOIN TABLE directly
public class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
{
    public void Configure(EntityTypeBuilder<ArticleCategory> builder)
    {
        builder.ToTable("article_categories");

        // Composite Primary Key
        builder.HasKey(ac => new { ac.ArticleId, ac.CategoryId });

        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        // Relationships
        builder.HasOne(e => e.Article)
            .WithMany(a => a.ArticleCategories)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting article deletes join entry

        builder.HasOne(e => e.Category)
            .WithMany(c => c.ArticleCategories)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting category deletes join entry
    }
}
// --- END OF FILE ArticleCategoryConfiguration.cs ---

// --- START OF FILE ArticleTagConfiguration.cs --- Updated ---
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace infrastructure; // Assuming correct namespace

// Configure JOIN TABLE directly
public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
{
    public void Configure(EntityTypeBuilder<ArticleTag> builder)
    {
        builder.ToTable("article_tags");

        // Composite Primary Key
        builder.HasKey(at => new { at.ArticleId, at.TagId });

        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.TagId).HasColumnName("tag_id");

        // Relationships
        builder.HasOne(e => e.Article)
            .WithMany(a => a.ArticleTags)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Tag)
            .WithMany(t => t.ArticleTags)
            .HasForeignKey(e => e.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
// --- END OF FILE ArticleTagConfiguration.cs ---

// --- START OF FILE ArticleProductConfiguration.cs --- Updated ---
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace infrastructure; // Assuming correct namespace

// Configure JOIN TABLE directly
public class ArticleProductConfiguration : IEntityTypeConfiguration<ArticleProduct>
{
    public void Configure(EntityTypeBuilder<ArticleProduct> builder)
    {
        builder.ToTable("article_products");

        // Composite Primary Key
        builder.HasKey(ap => new { ap.ArticleId, ap.ProductId });

        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);

        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_article_products_order_index");
         // Remove other redundant indexes if composite key covers them

        // Relationships
        builder.HasOne(e => e.Article)
            .WithMany(a => a.ArticleProducts)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Product)
            .WithMany(p => p.ArticleProducts)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade); // Or Restrict if needed
    }
}
// --- END OF FILE ArticleProductConfiguration.cs ---

// --- START OF FILE CommentConfiguration.cs --- (Verify Parent OnDelete) ---
using domain.Entities;
using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace infrastructure;
public class CommentConfiguration : BaseEntityConfiguration<Comment, int>
{
    public override void Configure(EntityTypeBuilder<Comment> builder)
    {
        base.Configure(builder);
        builder.ToTable("comments");
        // ... other properties ...
        builder.Property(e => e.ParentId).HasColumnName("parent_id");
        builder.Property(e => e.ArticleId).HasColumnName("article_id");
        // ... other indexes ...
        builder.HasIndex(e => e.ParentId).HasDatabaseName("idx_comments_parent_id");

        builder.HasOne(e => e.Article)
            .WithMany(a => a.Comments)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting article deletes comments

        builder.HasOne(e => e.Parent)
            .WithMany(c => c.Replies)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict); // <<< VERIFY THIS: Default is Restrict. Use Cascade to delete replies with parent, SetNull to orphan them.
    }
}
// --- END OF FILE CommentConfiguration.cs ---

// --- START OF FILE ArticleConfiguration.cs --- No changes needed ---
using domain.Entities; using domain.Entities.Shared; using Microsoft.EntityFrameworkCore; using Microsoft.EntityFrameworkCore.Metadata.Builders; using shared.Enums;
namespace infrastructure;
public class ArticleConfiguration : SeoEntityConfiguration<Article, int> { /* ... as before ... */ }
// --- END OF FILE ArticleConfiguration.cs ---

// --- IMPORTANT ---
// Make sure DbContext has:
// DbSet<Article> Articles { get; set; }
// DbSet<ArticleCategory> ArticleCategories { get; set; }
// DbSet<ArticleTag> ArticleTags { get; set; }
// DbSet<ArticleProduct> ArticleProducts { get; set; }
// DbSet<Comment> Comments { get; set; }
// And apply configurations:
// builder.ApplyConfiguration(new ArticleCategoryConfiguration());
// builder.ApplyConfiguration(new ArticleTagConfiguration());
// builder.ApplyConfiguration(new ArticleProductConfiguration());
// builder.ApplyConfiguration(new CommentConfiguration());
// --- End Important ---
```

**3. ViewModels**

*   Add `UpdatedAt` to list view models. Add `[Display]` attributes.

```csharp
// --- START OF FILE ArticleListItemViewModel.cs --- Updated ---
using shared.Enums;
namespace web.Areas.Admin.ViewModels.Article;
public class ArticleListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ThumbnailImage { get; set; } // MinIO Path
    public string? AuthorName { get; set; }
    public string? CategoryNames { get; set; }
    public ArticleType Type { get; set; }
    public PublishStatus Status { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime? UpdatedAt { get; set; } // Changed to nullable
}
// --- END OF FILE ArticleListItemViewModel.cs ---

// --- START OF FILE ArticleViewModel.cs --- Updated ---
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.ViewModels.Article;
// Add SEO Interface
public class ArticleViewModel : ISeoPropertiesViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tiêu đề", Prompt = "Nhập tiêu đề bài viết")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Slug", Prompt = "tieu-de-bai-viet")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Nội dung chi tiết", Prompt = "Soạn thảo nội dung bài viết...")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Content { get; set; } = string.Empty; // Summernote

    [Display(Name = "Tóm tắt / Mô tả ngắn", Prompt = "Nhập mô tả ngắn gọn (hiển thị ở danh sách, tối đa 500 ký tự)")]
    [MaxLength(500)]
    public string? Summary { get; set; }

    [Display(Name = "Ảnh đại diện (Featured)", Prompt = "Chọn hoặc nhập đường dẫn ảnh MinIO")]
    [MaxLength(2048)]
    public string? FeaturedImage { get; set; } // MinIO Path

    [Display(Name = "Ảnh thu nhỏ (Thumbnail)", Prompt = "Chọn hoặc nhập đường dẫn ảnh MinIO (nếu khác ảnh Featured)")]
    [MaxLength(2048)]
    public string? ThumbnailImage { get; set; } // MinIO Path

    [Display(Name = "Nổi bật")]
    public bool IsFeatured { get; set; } = false;

    // Removed IsActive - Use Status instead for articles
    // public bool IsActive { get; set; }

    [Display(Name = "Ước tính phút đọc")]
    public int EstimatedReadingMinutes { get; set; } // Calculated field

    [Display(Name = "Ngày xuất bản", Prompt = "Để trống nếu muốn xuất bản ngay khi chuyển trạng thái")]
    public DateTime? PublishedAt { get; set; } // DateTime picker

    [Display(Name = "ID Tác giả (nếu có)")]
    [MaxLength(50)]
    public string? AuthorId { get; set; } // Optional

    [Display(Name = "Tên tác giả hiển thị", Prompt = "Tên sẽ hiển thị cùng bài viết")]
    [MaxLength(100)]
    public string? AuthorName { get; set; }

    [Display(Name = "Avatar tác giả (MinIO Path)", Prompt = "Chọn hoặc nhập đường dẫn ảnh MinIO")]
    [MaxLength(255)]
    public string? AuthorAvatar { get; set; } // MinIO Path

    [Display(Name = "Loại bài viết")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public ArticleType Type { get; set; } = ArticleType.Knowledge;

    [Display(Name = "Trạng thái")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    // --- Relationships ---
    [Display(Name = "Danh mục")]
    [Required(ErrorMessage = "Vui lòng chọn ít nhất một {0}.")]
    public List<int> SelectedCategoryIds { get; set; } = new List<int>();

    [Display(Name = "Thẻ (Tags)")]
    public List<int> SelectedTagIds { get; set; } = new List<int>();

    [Display(Name = "Sản phẩm liên quan")]
    public List<int> SelectedProductIds { get; set; } = new List<int>();

    // --- SEO Fields (from ISeoPropertiesViewModel) ---
    [Display(Name = "Meta Title")] [MaxLength(100)] public string? MetaTitle { get; set; }
    [Display(Name = "Meta Description")] [MaxLength(300)] public string? MetaDescription { get; set; }
    [Display(Name = "Meta Keywords")] [MaxLength(200)] public string? MetaKeywords { get; set; }
    [Display(Name = "Canonical URL")] [MaxLength(255)] [Url] public string? CanonicalUrl { get; set; }
    [Display(Name = "No Index")] public bool NoIndex { get; set; } = false;
    [Display(Name = "No Follow")] public bool NoFollow { get; set; } = false;
    [Display(Name = "Open Graph Title")] [MaxLength(100)] public string? OgTitle { get; set; }
    [Display(Name = "Open Graph Description")] [MaxLength(300)] public string? OgDescription { get; set; }
    [Display(Name = "Open Graph Image (URL/Path)")] [MaxLength(255)] public string? OgImage { get; set; } // Store Path or URL? Decide convention
    [Display(Name = "Open Graph Type")] [MaxLength(50)] public string? OgType { get; set; } = "article";
    [Display(Name = "Twitter Title")] [MaxLength(100)] public string? TwitterTitle { get; set; }
    [Display(Name = "Twitter Description")] [MaxLength(300)] public string? TwitterDescription { get; set; }
    [Display(Name = "Twitter Image (URL/Path)")] [MaxLength(255)] public string? TwitterImage { get; set; }
    [Display(Name = "Twitter Card Type")] [MaxLength(50)] public string? TwitterCard { get; set; } = "summary_large_image";
    [Display(Name = "Schema Markup (JSON-LD)")] public string? SchemaMarkup { get; set; }
    [Display(Name = "Breadcrumb JSON")] public string? BreadcrumbJson { get; set; }
    [Display(Name = "Sitemap Priority (0.0 - 1.0)")] [Range(0.0, 1.0)] public double SitemapPriority { get; set; } = 0.7;
    [Display(Name = "Sitemap Change Frequency")] [Required] public string SitemapChangeFrequency { get; set; } = "weekly";

    // --- Dropdown Data ---
    public SelectList? CategoryList { get; set; }
    public SelectList? TagList { get; set; }
    public SelectList? ProductList { get; set; }
    public SelectList? StatusList { get; set; }
    public SelectList? TypeList { get; set; }
}
// --- END OF FILE ArticleViewModel.cs ---

// --- START OF FILE CommentListItemViewModel.cs --- Updated ---
namespace web.Areas.Admin.ViewModels.Comment;
public class CommentListItemViewModel
{
    public int Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorEmail { get; set; }
    public string ContentExcerpt { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; } // Added
    public int ArticleId { get; set; }
    public string? ArticleTitle { get; set; }
    public int ReplyCount { get; set; }
}
// --- END OF FILE CommentListItemViewModel.cs ---

// --- START OF FILE CommentViewModel.cs --- Updated ---
using System.ComponentModel.DataAnnotations;
namespace web.Areas.Admin.ViewModels.Comment;
public class CommentViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tên tác giả")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(100)]
    public string AuthorName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "{0} không hợp lệ")]
    [MaxLength(100)]
    public string? AuthorEmail { get; set; }

    [Display(Name = "Website")]
    [MaxLength(255)]
    [Url(ErrorMessage = "{0} không hợp lệ")]
    public string? AuthorWebsite { get; set; }

    [Display(Name = "Nội dung bình luận")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Đã duyệt")]
    public bool IsApproved { get; set; }

    // Display Only Fields
    [Display(Name = "Ngày gửi")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Bài viết")]
    public string? ArticleTitle { get; set; }
    public int ArticleId { get; set; }
}
// --- END OF FILE CommentViewModel.cs ---

// --- START OF FILE ArticleFilterViewModel.cs --- No changes needed ---
// --- START OF FILE CommentFilterViewModel.cs --- No changes needed ---
```

**4. Mappers**

*   Update `ListItemViewModel` mappings for `UpdatedAt`. Verify join table mappings.

```csharp
// --- START OF FILE ArticleMappingProfile.cs --- Updated ---
using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Article;
namespace web.Areas.Admin.Mappers;
public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Article, ArticleListItemViewModel>()
            .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src => src.ThumbnailImage ?? src.FeaturedImage)) // Use Thumb, fallback to Featured
            .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src =>
                 src.ArticleCategories != null && src.ArticleCategories.Any()
                 ? string.Join(", ", src.ArticleCategories
                    .Where(ac => ac.Category != null) // Ensure category is loaded
                    .Select(ac => ac.Category!.Name) // Use null-forgiving
                    .OrderBy(name => name))
                 : null // Return null or "" if no categories
            ))
             .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt)); // Map UpdatedAt

        // Entity -> ViewModel (For Edit GET)
        CreateMap<Article, ArticleViewModel>()
            .ForMember(dest => dest.SelectedCategoryIds, opt => opt.MapFrom(src => src.ArticleCategories != null ? src.ArticleCategories.Select(ac => ac.CategoryId).ToList() : new List<int>()))
            .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ArticleTags != null ? src.ArticleTags.Select(at => at.TagId).ToList() : new List<int>()))
            .ForMember(dest => dest.SelectedProductIds, opt => opt.MapFrom(src => src.ArticleProducts != null ? src.ArticleProducts.Select(ap => ap.ProductId).ToList() : new List<int>()))
            // Map nested view models
            .ForMember(dest => dest.Images, opt => opt.Ignore()) // Images not typically edited on article form
            .ForMember(dest => dest.Variants, opt => opt.Ignore()) // Variants don't belong here
            // Ignore dropdowns
            .ForMember(dest => dest.CategoryList, opt => opt.Ignore())
            .ForMember(dest => dest.TagList, opt => opt.Ignore())
            .ForMember(dest => dest.ProductList, opt => opt.Ignore())
            .ForMember(dest => dest.StatusList, opt => opt.Ignore())
            .ForMember(dest => dest.TypeList, opt => opt.Ignore());

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<ArticleViewModel, Article>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore()) // Not edited by admin
            // EstimatedReadingMinutes calculated in Controller
            .ForMember(dest => dest.ArticleCategories, opt => opt.Ignore()) // Manual handling
            .ForMember(dest => dest.ArticleTags, opt => opt.Ignore()) // Manual handling
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore()) // Manual handling
            .ForMember(dest => dest.Comments, opt => opt.Ignore()) // Not managed here
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}
// --- END OF FILE ArticleMappingProfile.cs ---

// --- START OF FILE CommentMappingProfile.cs --- Updated ---
using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Comment;
namespace web.Areas.Admin.Mappers;
public class CommentProfile : Profile
{
    public CommentProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Comment, CommentListItemViewModel>()
            .ForMember(dest => dest.ContentExcerpt, opt => opt.MapFrom(src => // Safer excerpt
                 src.Content != null && src.Content.Length > 100 ? src.Content.Substring(0, 100) + "..." : src.Content))
            .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : null))
            .ForMember(dest => dest.ReplyCount, opt => opt.MapFrom(src => src.Replies != null ? src.Replies.Count : 0))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt)); // Map UpdatedAt

        // Entity -> ViewModel
        CreateMap<Comment, CommentViewModel>()
             .ForMember(dest => dest.ArticleTitle, opt => opt.MapFrom(src => src.Article != null ? src.Article.Title : null));

        // ViewModel -> Entity (For Update from Admin Edit)
        CreateMap<CommentViewModel, Comment>()
             // Map only fields editable by Admin
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(dest => dest.AuthorEmail, opt => opt.MapFrom(src => src.AuthorEmail))
            .ForMember(dest => dest.AuthorWebsite, opt => opt.MapFrom(src => src.AuthorWebsite))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved))
            // Ignore non-editable fields
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleId, opt => opt.Ignore())
            .ForMember(dest => dest.ParentId, opt => opt.Ignore())
            .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
            .ForMember(dest => dest.AuthorAvatar, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Article, opt => opt.Ignore())
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Replies, opt => opt.Ignore());
    }
}
// --- END OF FILE CommentMappingProfile.cs ---
```

**5. Validators**

*   Inject `DbContext` into `ArticleViewModelValidator` for unique slug check. Use `{PropertyName}`.

```csharp
// --- START OF FILE ArticleViewModelValidator.cs --- Updated ---
using FluentValidation;
using infrastructure; // For DbContext
using Microsoft.EntityFrameworkCore; // For AnyAsync
using web.Areas.Admin.ViewModels.Article;
using web.Areas.Admin.Validators.Shared; // For SEO Validator

namespace web.Areas.Admin.Validators.Article;

public class ArticleViewModelValidator : AbstractValidator<ArticleViewModel>
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    public ArticleViewModelValidator(ApplicationDbContext context)
    {
        _context = context; // Assign context

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255)
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ chứa chữ thường, số, dấu gạch ngang.")
            .MustAsync(BeUniqueSlug).WithMessage("{PropertyName} này đã tồn tại. Vui lòng chọn slug khác."); // Add DB check

        RuleFor(x => x.Content).NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");
        RuleFor(x => x.Summary).MaximumLength(500);
        RuleFor(x => x.FeaturedImage).MaximumLength(2048); // Increased length for path
        RuleFor(x => x.ThumbnailImage).MaximumLength(2048); // Increased length for path
        RuleFor(x => x.AuthorName).MaximumLength(100);
        RuleFor(x => x.AuthorAvatar).MaximumLength(255);
        RuleFor(x => x.Type).IsInEnum().WithMessage("{PropertyName} không hợp lệ.");
        RuleFor(x => x.Status).IsInEnum().WithMessage("{PropertyName} không hợp lệ.");

        RuleFor(x => x.SelectedCategoryIds)
            .NotEmpty().WithMessage("Vui lòng chọn ít nhất một {PropertyName}.");

        // Include base SEO validation rules
        Include(new SeoPropertiesValidator<ArticleViewModel>()); // Assuming this exists
    }

    private async Task<bool> BeUniqueSlug(ArticleViewModel viewModel, string slug, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(slug)) return true;
        return !await _context.Set<domain.Entities.Article>()
                               .AnyAsync(a => a.Slug.ToLower() == slug.ToLower() && a.Id != viewModel.Id, cancellationToken);
    }
}
// --- END OF FILE ArticleViewModelValidator.cs ---

// --- START OF FILE CommentViewModelValidator.cs --- Updated ---
using FluentValidation;
using web.Areas.Admin.ViewModels.Comment;
using System.Text.RegularExpressions;

namespace web.Areas.Admin.Validators.Comment;

public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
{
    public CommentViewModelValidator()
    {
        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("{PropertyName} không được rỗng.")
            .MaximumLength(100);

        RuleFor(x => x.AuthorEmail)
            .EmailAddress().WithMessage("{PropertyName} không hợp lệ.").When(x => !string.IsNullOrWhiteSpace(x.AuthorEmail))
            .MaximumLength(100);

        RuleFor(x => x.AuthorWebsite)
            .MaximumLength(255)
            // Basic URL validation (optional, could be more strict)
            .Matches(@"^(https?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$").WithMessage("{PropertyName} không hợp lệ.").When(x => !string.IsNullOrWhiteSpace(x.AuthorWebsite));

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("{PropertyName} không được rỗng.");

        // IsApproved is a boolean, usually no validation needed unless specific rules apply
    }
}
// --- END OF FILE CommentViewModelValidator.cs ---

```

**6. Controllers (`ArticleController.cs`, `CommentController.cs`)**

*   Implement pagination, logging, TempData, validation handling (middleware), `ProjectTo`.
*   Refine relationship update logic in `ArticleController`.
*   Implement AJAX Approve/Unapprove in `CommentController`.
*   Standardize everything.

```csharp
// --- START OF FILE ArticleController.cs --- Updated ---
using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions; // For GetDisplayName
using web.Areas.Admin.Services; // Assuming Minio service is here
using web.Areas.Admin.ViewModels.Article;
using X.PagedList;
using X.PagedList.EF;
using System.Text.RegularExpressions; // For Regex word count

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ArticleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ArticleController> _logger;
    private readonly IMinioStorageService _minioService; // Assuming injection
    // Remove IValidator

    public ArticleController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ArticleController> logger,
        IMinioStorageService minioService) // Inject Minio Service
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _minioService = minioService ?? throw new ArgumentNullException(nameof(minioService));
    }

    // GET: Admin/Article
    public async Task<IActionResult> Index(string? searchTerm = null, int? categoryId = null, ArticleType? type = null, PublishStatus? status = null, int page = 1, int pageSize = 15) // Add pagination
    {
        ViewData["Title"] = "Quản lý Bài viết - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Bài viết";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))) };

        int pageNumber = page;
        var query = _context.Set<Article>()
                            .Include(a => a.ArticleCategories).ThenInclude(ac => ac.Category)
                            .AsNoTracking();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
             string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(a => a.Title.ToLower().Contains(lowerSearchTerm)
                                  || (a.Summary != null && a.Summary.ToLower().Contains(lowerSearchTerm)));
        }
        if (categoryId.HasValue && categoryId > 0)
        {
            query = query.Where(a => a.ArticleCategories.Any(ac => ac.CategoryId == categoryId.Value));
        }
        if (type.HasValue)
        {
            query = query.Where(a => a.Type == type.Value);
        }
        if (status.HasValue)
        {
            query = query.Where(a => a.Status == status.Value);
        }

        // Sorting & Pagination
        var articlesPaged = await query
            .OrderByDescending(a => a.IsFeatured)
            .ThenByDescending(a => a.PublishedAt ?? a.CreatedAt) // Order by publish date, fallback to creation
            .ProjectTo<ArticleListItemViewModel>(_mapper.ConfigurationProvider) // Project
            .ToPagedListAsync(pageNumber, pageSize); // Paginate

        // Load filter data
        await LoadFilterDropdownsAsync(categoryId, type, status);

        ViewBag.SearchTerm = searchTerm;
        // Selected values loaded into ViewBag by LoadFilterDropdownsAsync

        return View(articlesPaged); // Pass paged list
    }


    // GET: Admin/Article/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Viết Bài mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Viết Bài mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ("Viết bài mới", "") };

        var viewModel = new ArticleViewModel
        {
            //IsActive = true, // Removed, use Status
            Status = PublishStatus.Draft,
            PublishedAt = null, // Let it be null initially
            SitemapPriority = 0.7,
            SitemapChangeFrequency = "weekly",
            OgType = "article",
            Images = new List<ProductImageViewModel>(), // Initialize lists
            Variants = new List<ProductVariantViewModel>(), // Initialize lists
            SelectedCategoryIds = new List<int>(),
            SelectedTagIds = new List<int>(),
            SelectedProductIds = new List<int>(),
        };

        // Set default Author from current logged-in user if possible
        // viewModel.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get ID if needed
        // viewModel.AuthorName = User.Identity?.Name; // Get display name
        // viewModel.AuthorAvatar = await GetUserAvatarPathAsync(viewModel.AuthorId); // Fetch avatar path

        await LoadRelatedDataForFormAsync(viewModel); // Load dropdowns
        return View(viewModel);
    }

    // POST: Admin/Article/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleViewModel viewModel)
    {
        // --- Manual Checks & Logic ---
        await ValidateRelationsAndUniquenessAsync(viewModel, isEdit: false); // Check unique slug
        viewModel.EstimatedReadingMinutes = CalculateReadingTime(viewModel.Content);
        HandlePublishDate(viewModel); // Set/clear PublishedAt based on status

        if (ModelState.IsValid)
        {
            var article = _mapper.Map<Article>(viewModel);

            // --- Handle Relationships ---
            article.ArticleCategories = viewModel.SelectedCategoryIds?
                .Select(catId => new ArticleCategory { CategoryId = catId }).ToList() ?? new List<ArticleCategory>();
            article.ArticleTags = viewModel.SelectedTagIds?
                .Select(tagId => new ArticleTag { TagId = tagId }).ToList() ?? new List<ArticleTag>();
            article.ArticleProducts = viewModel.SelectedProductIds?
                .Select((prodId, index) => new ArticleProduct { ProductId = prodId, OrderIndex = index }).ToList() ?? new List<ArticleProduct>();

             // Set AuthorId if needed (example)
             // if (string.IsNullOrEmpty(article.AuthorId)) {
             //     article.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             // }

            _context.Articles.Add(article);

            try
            {
                await _context.SaveChangesAsync();
                 _logger.LogInformation("Article '{Title}' (ID: {ArticleId}) created successfully by {User}.", article.Title, article.Id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Tạo bài viết '{Truncate(article.Title)}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex) { HandleDbUpdateException(ex, viewModel.Slug); } // Handle unique constraints etc.
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating article '{Title}'.", viewModel.Title);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi tạo bài viết.");
            }
        }
        else {
            _logger.LogWarning("Article creation failed for '{Title}'. Model state is invalid.", viewModel.Title);
        }


        // If failed, redisplay form
        await LoadRelatedDataForFormAsync(viewModel); // Reload dropdowns
        ViewData["Title"] = "Viết Bài mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Viết Bài mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ("Viết bài mới", "") };
        return View(viewModel);
    }


    // GET: Admin/Article/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var article = await _context.Set<Article>()
            .Include(a => a.ArticleCategories)
            .Include(a => a.ArticleTags)
            .Include(a => a.ArticleProducts) // No need to ThenInclude Product unless displaying product names here
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null) { _logger.LogWarning("Edit GET: Article ID {ArticleId} not found.", id); return NotFound(); }

        var viewModel = _mapper.Map<ArticleViewModel>(article);
        await LoadRelatedDataForFormAsync(viewModel);

        ViewData["Title"] = "Chỉnh sửa Bài viết - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Bài viết: {Truncate(article.Title)}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(article.Title)}", "") };

        return View(viewModel);
    }


    // POST: Admin/Article/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ArticleViewModel viewModel)
    {
        if (id != viewModel.Id) { _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id); return BadRequest(); }

        // --- Manual Checks & Logic ---
        await ValidateRelationsAndUniquenessAsync(viewModel, isEdit: true); // Check unique slug
        viewModel.EstimatedReadingMinutes = CalculateReadingTime(viewModel.Content);
        HandlePublishDate(viewModel, await GetOriginalStatusAsync(id)); // Set/clear PublishedAt

        if (ModelState.IsValid)
        {
            var article = await _context.Set<Article>()
                .Include(a => a.ArticleCategories)
                .Include(a => a.ArticleTags)
                .Include(a => a.ArticleProducts)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null) { _logger.LogWarning("Edit POST: Article ID {ArticleId} not found for update.", id); TempData["error"] = "Không tìm thấy bài viết."; return RedirectToAction(nameof(Index)); }

            // --- Map Scalar Properties ---
            _mapper.Map(viewModel, article);

            try
            {
                // --- Update Relationships ---
                UpdateJunctionTable(article.ArticleCategories, viewModel.SelectedCategoryIds, id, (catId) => new ArticleCategory { ArticleId = id, CategoryId = catId }, fc => fc.CategoryId);
                UpdateJunctionTable(article.ArticleTags, viewModel.SelectedTagIds, id, (tagId) => new ArticleTag { ArticleId = id, TagId = tagId }, ft => ft.TagId);
                UpdateJunctionTable(article.ArticleProducts, viewModel.SelectedProductIds, id, (prodId) => new ArticleProduct { ArticleId = id, ProductId = prodId }, fp => fp.ProductId);

                 // Set audit fields if needed
                // article.UpdatedBy = User.Identity?.Name;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Article ID {ArticleId} ('{Title}') updated successfully by {User}.", id, article.Title, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật bài viết '{Truncate(article.Title)}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex) { _logger.LogWarning(ex, "Concurrency error updating article ID: {ArticleId}", id); TempData["error"] = "Lỗi: Xung đột dữ liệu."; }
            catch (DbUpdateException ex) { HandleDbUpdateException(ex, viewModel.Slug); } // Handle unique constraints etc.
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating article ID: {ArticleId}", id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật.");
            }
        }
         else
        {
            _logger.LogWarning("Article editing failed for ID: {ArticleId}. Model state is invalid.", id);
        }

        // If failed, redisplay form
        await LoadRelatedDataForFormAsync(viewModel); // Reload dropdowns
        ViewData["Title"] = "Chỉnh sửa Bài viết - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Bài viết: {Truncate(viewModel.Title)}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(viewModel.Title)}", "") };
        return View(viewModel);
    }


    // POST: Admin/Article/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _context.Set<Article>()
                                 // No need to include related data if using Cascade delete
                                 .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
             _logger.LogWarning("Delete POST: Article ID {ArticleId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy bài viết." });
        }

        try
        {
            string title = article.Title;
            // --- Get image paths BEFORE removing entity ---
             var imagesToDelete = new List<string?> { article.FeaturedImage, article.ThumbnailImage, article.OgImage, article.TwitterImage }
                                        .Where(url => !string.IsNullOrEmpty(url) && !url.StartsWith("http")) // Select only non-null paths (not URLs)
                                        .Distinct()
                                        .ToList();

            _context.Articles.Remove(article); // Cascade should handle junction tables
            await _context.SaveChangesAsync(); // Save DB changes

            _logger.LogInformation("Article '{Title}' (ID: {ArticleId}) deleted successfully by {User}.", title, id, User.Identity?.Name ?? "Unknown");

             // --- Delete images from storage AFTER DB delete succeeds ---
            foreach (var path in imagesToDelete)
            {
                if(path == null) continue;
                try
                {
                    await _minioService.DeleteFileAsync(path); // Use injected service
                     _logger.LogInformation("Deleted image '{ImagePath}' from MinIO for deleted Article ID {ArticleId}.", path, id);
                }
                catch (Exception minioEx)
                {
                    _logger.LogError(minioEx, "Failed to delete image '{ImagePath}' from MinIO after deleting Article ID {ArticleId}.", path, id);
                    // Don't fail the entire operation, just log the error
                }
            }

            return Json(new { success = true, message = $"Xóa bài viết '{Truncate(title)}' thành công." });
        }
        catch (DbUpdateException ex) {
             _logger.LogError(ex, "Error deleting Article ID {ArticleId}. Potential RESTRICT constraint violation.", id);
             return Json(new { success = false, message = "Không thể xóa bài viết. Có thể có lỗi ràng buộc dữ liệu." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Article ID {ArticleId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa bài viết." });
        }
    }


    // --- Helper Methods ---

    // Loads dropdowns for Index Filters
    private async Task LoadFilterDropdownsAsync(int? categoryId, ArticleType? type, PublishStatus? status)
    {
        ViewBag.Categories = await _context.Set<Category>().Where(c => c.Type == CategoryType.Article && c.IsActive).OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = c.Id == categoryId }).ToListAsync();
        ViewBag.Types = Enum.GetValues(typeof(ArticleType)).Cast<ArticleType>().Select(t => new SelectListItem { Value = t.ToString(), Text = t.GetDisplayName(), Selected = t == type }).ToList();
        ViewBag.Statuses = Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(s => new SelectListItem { Value = s.ToString(), Text = s.GetDisplayName(), Selected = s == status }).ToList();
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SelectedType = type;
        ViewBag.SelectedStatus = status;
    }

    // Loads dropdowns/select lists for Create/Edit Form
    private async Task LoadRelatedDataForFormAsync(ArticleViewModel viewModel)
    {
        viewModel.CategoryList = new SelectList(await _context.Set<Category>().Where(c => c.IsActive && c.Type == CategoryType.Article).OrderBy(c => c.Name).Select(c=> new {c.Id, c.Name}).ToListAsync(), "Id", "Name");
        viewModel.TagList = new SelectList(await _context.Set<Tag>().Where(t => t.Type == TagType.Article).OrderBy(t => t.Name).Select(t=> new {t.Id, t.Name}).ToListAsync(), "Id", "Name");
        viewModel.ProductList = new SelectList(await _context.Set<Product>().Where(p => p.IsActive && p.Status == PublishStatus.Published).OrderBy(p => p.Name).Select(p=> new {p.Id, p.Name}).ToListAsync(), "Id", "Name");
        viewModel.StatusList = new SelectList(Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(e => new { Value = e, Text = e.GetDisplayName() }), "Value", "Text", viewModel.Status);
        viewModel.TypeList = new SelectList(Enum.GetValues(typeof(ArticleType)).Cast<ArticleType>().Select(e => new { Value = e, Text = e.GetDisplayName() }), "Value", "Text", viewModel.Type);
    }

    // Handles setting/clearing PublishedAt date based on status
    private void HandlePublishDate(ArticleViewModel viewModel, PublishStatus? originalStatus = null)
    {
        if (viewModel.Status == PublishStatus.Published && originalStatus != PublishStatus.Published && !viewModel.PublishedAt.HasValue)
        {
            viewModel.PublishedAt = DateTime.UtcNow;
        }
        else if (viewModel.Status != PublishStatus.Published)
        {
            viewModel.PublishedAt = null; // Clear if not published
        }
        // If status is Published and date is already set, leave it as is (allows manual setting/scheduling)
    }

    // Gets original status for comparison in Edit POST
    private async Task<PublishStatus?> GetOriginalStatusAsync(int articleId) {
        return await _context.Articles
                             .Where(a => a.Id == articleId)
                             .Select(a => (PublishStatus?)a.Status) // Cast to nullable
                             .FirstOrDefaultAsync();
    }

     // Generic helper to update Many-to-Many junction tables
    private void UpdateJunctionTable<TEntity, TKey>(ICollection<TEntity> currentCollection, IEnumerable<TKey>? selectedIds, int parentId, Func<TKey, TEntity> createEntity, Func<TEntity, TKey> getKey)
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        selectedIds ??= new List<TKey>(); // Ensure list is not null

        var currentIds = currentCollection.Select(getKey).ToList();
        var idsToAdd = selectedIds.Except(currentIds).ToList();
        var entitiesToRemove = currentCollection.Where(e => !selectedIds.Contains(getKey(e))).ToList();

        if(entitiesToRemove.Any()) _context.RemoveRange(entitiesToRemove);

        if(idsToAdd.Any()) {
            foreach (var idToAdd in idsToAdd)
            {
                currentCollection.Add(createEntity(idToAdd));
            }
        }
    }

    // Calculates estimated reading time
     private int CalculateReadingTime(string content, int wordsPerMinute = 200)
    {
        if (string.IsNullOrWhiteSpace(content)) return 0;
        // Remove HTML tags before counting words
        var plainText = Regex.Replace(content, "<.*?>", string.Empty);
        var wordCount = plainText.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        if (wordsPerMinute <= 0) wordsPerMinute = 200; // Avoid division by zero
        return Math.Max(1, (int)Math.Ceiling(wordCount / (double)wordsPerMinute)); // Ensure at least 1 minute
    }

    // Handles DbUpdateException, adding specific ModelErrors
    private void HandleDbUpdateException(DbUpdateException ex, string? slug) {
        _logger.LogError(ex, "DbUpdateException occurred.");
        if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: articles.slug", StringComparison.OrdinalIgnoreCase) ?? false) {
            ModelState.AddModelError(nameof(ArticleViewModel.Slug), "Slug này đã tồn tại.");
        } else {
            ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi lưu.");
        }
    }

    // Truncates string for display
    private string Truncate(string value, int maxLength = 40) {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }
}
// --- END OF FILE ArticleController.cs ---


// --- START OF FILE CommentController.cs --- Updated ---
using AutoMapper;
using AutoMapper.QueryableExtensions; // Added
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Added
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Comment;
using X.PagedList; // Added
using X.PagedList.EF; // Added

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CommentController> _logger;
    // Remove IValidator

    public CommentController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<CommentController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Comment
    public async Task<IActionResult> Index(string? searchTerm = null, bool? isApproved = null, int? articleId = null, int page = 1, int pageSize = 20) // Added pagination
    {
        ViewData["Title"] = "Quản lý Bình luận - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bình luận", Url.Action(nameof(Index))) };

        int pageNumber = page;
        var query = _context.Set<Comment>()
                            .Include(c => c.Article) // For ArticleTitle
                            .Include(c => c.Replies) // For ReplyCount
                            .Where(c => c.ParentId == null) // Only show top-level comments
                            .AsNoTracking(); // Use AsNoTracking

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
             string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(c => c.AuthorName.ToLower().Contains(lowerSearchTerm)
                                  || (c.AuthorEmail != null && c.AuthorEmail.ToLower().Contains(lowerSearchTerm))
                                  || c.Content.ToLower().Contains(lowerSearchTerm));
        }
        if (isApproved.HasValue)
        {
            query = query.Where(c => c.IsApproved == isApproved.Value);
        }
        if (articleId.HasValue && articleId > 0)
        {
            query = query.Where(c => c.ArticleId == articleId.Value);
        }

        // Sorting & Pagination
        var commentsPaged = await query
            .OrderByDescending(c => c.IsApproved == false) // Unapproved first
            .ThenByDescending(c => c.CreatedAt) // Then by date
            .ProjectTo<CommentListItemViewModel>(_mapper.ConfigurationProvider) // Project efficiently
            .ToPagedListAsync(pageNumber, pageSize); // Paginate

        // Load filter dropdowns
        await LoadArticleFilterDropdownAsync(articleId); // Load articles for filter
        ViewBag.ApprovalStatusList = new List<SelectListItem> {
             new SelectListItem { Value = "true", Text = "Đã duyệt"},
             new SelectListItem { Value = "false", Text = "Chưa duyệt"}
        };

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedApprovedStatus = isApproved;
        // SelectedArticleId loaded by helper

        return View(commentsPaged); // Pass paged list
    }

    // GET: Admin/Comment/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var comment = await _context.Set<Comment>()
                                 .Include(c => c.Article) // Include article for title display
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null) { _logger.LogWarning("Edit GET: Comment ID {CommentId} not found.", id); return NotFound(); }

        var viewModel = _mapper.Map<CommentViewModel>(comment);

        ViewData["Title"] = "Chỉnh sửa Bình luận - Hệ thống quản trị";
        ViewData["PageTitle"] = "Chỉnh sửa Bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bình luận", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };

        return View(viewModel);
    }

    // POST: Admin/Comment/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CommentViewModel viewModel)
    {
        if (id != viewModel.Id) { _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id); return BadRequest(); }

        // Rely on middleware for validation
        if (ModelState.IsValid)
        {
            var comment = await _context.Set<Comment>().FindAsync(id);
            if (comment == null) { _logger.LogWarning("Edit POST: Comment ID {CommentId} not found for update.", id); TempData["error"] = "Không tìm thấy bình luận."; return RedirectToAction(nameof(Index)); }

            // Map editable fields: AuthorName, AuthorEmail, AuthorWebsite, Content, IsApproved
            _mapper.Map(viewModel, comment);
             // Set audit fields
            // comment.UpdatedBy = User.Identity?.Name;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Comment ID {CommentId} updated successfully by {User}.", id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = "Cập nhật bình luận thành công!";
                return RedirectToAction(nameof(Index)); // Redirect to list after successful edit
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Comment ID {CommentId}", id);
                ModelState.AddModelError("", "Không thể lưu thay đổi.");
            }
        }
        else {
            _logger.LogWarning("Comment editing failed for ID: {CommentId}. Model state is invalid.", id);
        }

        // If failed, redisplay form
        // Reload ArticleTitle if needed as it's not posted back
        var originalArticle = await _context.Articles.AsNoTracking().Select(a => new { a.Id, a.Title }).FirstOrDefaultAsync(a => a.Id == viewModel.ArticleId);
        viewModel.ArticleTitle = originalArticle?.Title;

        ViewData["Title"] = "Chỉnh sửa Bình luận - Hệ thống quản trị";
        ViewData["PageTitle"] = "Chỉnh sửa Bình luận";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bình luận", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };
        return View(viewModel);
    }

    // POST: Admin/Comment/Approve/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        return await UpdateApprovalStatus(id, true);
    }

    // POST: Admin/Comment/Unapprove/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unapprove(int id)
    {
        return await UpdateApprovalStatus(id, false);
    }


    // POST: Admin/Comment/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _context.Set<Comment>()
                                // Include replies only if needed due to RESTRICT constraint or custom logic
                                // .Include(c => c.Replies)
                                .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null) { _logger.LogWarning("Delete POST: Comment ID {CommentId} not found.", id); return Json(new { success = false, message = "Không tìm thấy bình luận." }); }

        try
        {
            string author = comment.AuthorName;
            // If ParentId has RESTRICT and replies exist, this will fail. Handle by changing constraint to Cascade/SetNull or deleting replies first.
            _context.Remove(comment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Comment ID {CommentId} by '{Author}' deleted successfully by {User}.", id, author, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = $"Xóa bình luận của '{author}' thành công." });
        }
        catch (DbUpdateException ex) { // Catch FK errors
            _logger.LogError(ex, "Error deleting Comment ID {CommentId}. Potential constraint violation (e.g., replies exist with RESTRICT).", id);
             return Json(new { success = false, message = "Không thể xóa bình luận. Có thể do bình luận này có trả lời." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Comment ID {CommentId}", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa bình luận." });
        }
    }

    // --- Helper: Update Approval ---
    private async Task<IActionResult> UpdateApprovalStatus(int id, bool approve)
    {
        var comment = await _context.Set<Comment>().FindAsync(id);
        if (comment == null) { _logger.LogWarning("UpdateApprovalStatus: Comment ID {CommentId} not found.", id); return Json(new { success = false, message = "Không tìm thấy bình luận." }); }

        if (comment.IsApproved == approve) {
            return Json(new { success = true, message = approve ? "Bình luận đã được duyệt." : "Bình luận đã ở trạng thái chờ duyệt." }); // No change needed
        }

        comment.IsApproved = approve;
        // comment.UpdatedBy = User.Identity?.Name; // Set audit field

        try
        {
            await _context.SaveChangesAsync();
             _logger.LogInformation("Comment ID {CommentId} approval status set to {Status} by {User}.", id, approve, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = approve ? "Duyệt bình luận thành công." : "Bỏ duyệt bình luận thành công.", isApproved = approve }); // Return new status
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating approval status for Comment ID {CommentId} to {Status}.", id, approve);
            return Json(new { success = false, message = "Lỗi cập nhật trạng thái duyệt." });
        }
    }

     // Helper to load Article filter dropdown
    private async Task LoadArticleFilterDropdownAsync(int? selectedArticleId)
    {
        // Load only articles that actually have comments? Maybe too slow. Load recent/popular articles?
        // For simplicity, load all published articles for now. Optimize later if needed.
         ViewBag.ArticleList = await _context.Articles
                                       .Where(a => a.Status == PublishStatus.Published)
                                       .OrderBy(a => a.Title)
                                       .Select(a => new SelectListItem {
                                           Value = a.Id.ToString(),
                                           Text = a.Title.Length > 50 ? a.Title.Substring(0, 50) + "..." : a.Title, // Truncate long titles
                                           Selected = a.Id == selectedArticleId
                                        })
                                       .ToListAsync();
        ViewBag.SelectedArticleId = selectedArticleId;
    }
}
// --- END OF FILE CommentController.cs ---

```

**7. Views**

*   Implement pagination, item counts, standardized filters/buttons.
*   Use TomSelect/Summernote where appropriate.
*   Add AJAX handling for Approve/Unapprove buttons in Comment Index.
*   Add `UpdatedAt` columns.

```html
@* --- START OF FILE _ArticleFormFields.txt --- Updated --- *@
@using shared.Helpers; @using web.Areas.Admin.ViewModels.Article; @model ArticleViewModel; @using shared.Enums; @using shared.Extensions;

@* --- TABS --- *@
<ul class="nav nav-tabs nav-fill mb-3" data-bs-toggle="tabs" role="tablist">
	<li class="nav-item" role="presentation"><a href="#article-tab-content" class="nav-link active" data-bs-toggle="tab" aria-selected="true" role="tab"><i class="ti ti-file-text me-2"></i> Nội dung chính</a></li>
	<li class="nav-item" role="presentation"><a href="#article-tab-images" class="nav-link" data-bs-toggle="tab" aria-selected="false" role="tab" tabindex="-1"><i class="ti ti-photo me-2"></i> Hình ảnh & Tác giả</a></li>
	<li class="nav-item" role="presentation"><a href="#article-tab-relations" class="nav-link" data-bs-toggle="tab" aria-selected="false" role="tab" tabindex="-1"><i class="ti ti-tags me-2"></i> Phân loại & Liên kết</a></li>
	<li class="nav-item" role="presentation"><a href="#article-tab-publishing" class="nav-link" data-bs-toggle="tab" aria-selected="false" role="tab" tabindex="-1"><i class="ti ti-calendar-event me-2"></i> Xuất bản</a></li>
	<li class="nav-item" role="presentation"><a href="#article-tab-seo" class="nav-link" data-bs-toggle="tab" aria-selected="false" role="tab" tabindex="-1"><i class="ti ti-search me-2"></i> Tối ưu SEO</a></li>
</ul>

@* --- TAB CONTENT --- *@
<div class="tab-content">
	@* --- Tab: Content --- *@
	<div class="tab-pane active show" id="article-tab-content" role="tabpanel">
		<div class="mb-3"><label asp-for="Title" class="form-label required"></label><input asp-for="Title" class="form-control form-control-lg" id="articleTitle" /><span asp-validation-for="Title" class="text-danger"></span></div>
		<div class="mb-3"><label asp-for="Slug" class="form-label required"></label><div class="input-group"><input asp-for="Slug" class="form-control" id="articleSlug" /><button class="btn btn-outline-secondary" type="button" id="generateSlugButton"><i class="ti ti-refresh"></i></button></div><span asp-validation-for="Slug" class="text-danger"></span></div>
		<div class="mb-3"><label asp-for="Summary" class="form-label"></label><textarea asp-for="Summary" class="form-control" rows="3"></textarea><span asp-validation-for="Summary" class="text-danger"></span><small class="form-hint">Mô tả ngắn gọn cho bài viết (hiển thị ở danh sách).</small></div>
		<div class="mb-0"><label asp-for="Content" class="form-label required"></label><textarea asp-for="Content" class="form-control summernote-editor" id="articleContentEditor" rows="20"></textarea><span asp-validation-for="Content" class="text-danger"></span></div>
	</div>

	@* --- Tab: Images & Author --- *@
	<div class="tab-pane" id="article-tab-images" role="tabpanel">
		<div class="row">
			<div class="col-md-6">
				<h4 class="mb-3">Hình ảnh</h4>
				<div class="form-group mb-3" id="featured-image-area">
					<label asp-for="FeaturedImage" class="form-label"></label>
					<div><img id="featuredImagePreview" src="@MediaUrlHelper.GetMinioUrl(Model.FeaturedImage, "/img/placeholder.svg")" class="img-thumbnail mb-2" style="max-height: 200px; max-width: 100%; object-fit: contain; min-height: 100px; background-color: #f8f9fa;" /></div>
					<input type="hidden" asp-for="FeaturedImage" id="featuredImagePathInput" />
					<div><button type="button" class="btn btn-sm btn-outline-primary select-media-btn" data-target-input="#featuredImagePathInput" data-target-preview="#featuredImagePreview"><i class="ti ti-photo-search me-1"></i> Chọn ảnh Featured</button><button type="button" class="btn btn-sm btn-outline-danger remove-media-btn ms-2 @(string.IsNullOrEmpty(Model.FeaturedImage) ? "d-none" : "")" data-target-input="#featuredImagePathInput" data-target-preview="#featuredImagePreview" data-placeholder="/img/placeholder.svg"><i class="ti ti-x me-1"></i> Xóa</button></div>
					<span asp-validation-for="FeaturedImage" class="text-danger"></span>
				</div>
				<div class="form-group mb-3" id="thumbnail-image-area">
					<label asp-for="ThumbnailImage" class="form-label"></label>
					<div><img id="thumbnailImagePreview" src="@MediaUrlHelper.GetMinioUrl(Model.ThumbnailImage, "/img/placeholder.svg")" class="img-thumbnail mb-2" style="max-height: 100px; max-width: 100%; object-fit: contain; min-height: 60px; background-color: #f8f9fa;" /></div>
					<input type="hidden" asp-for="ThumbnailImage" id="thumbnailImagePathInput" />
					<div><button type="button" class="btn btn-sm btn-outline-primary select-media-btn" data-target-input="#thumbnailImagePathInput" data-target-preview="#thumbnailImagePreview"><i class="ti ti-photo-search me-1"></i> Chọn ảnh Thumbnail</button><button type="button" class="btn btn-sm btn-outline-danger remove-media-btn ms-2 @(string.IsNullOrEmpty(Model.ThumbnailImage) ? "d-none" : "")" data-target-input="#thumbnailImagePathInput" data-target-preview="#thumbnailImagePreview" data-placeholder="/img/placeholder.svg"><i class="ti ti-x me-1"></i> Xóa</button></div>
					<span asp-validation-for="ThumbnailImage" class="text-danger"></span>
					<small class="form-hint">Nếu bỏ trống sẽ dùng ảnh Featured làm thumbnail.</small>
				</div>
			</div>
			<div class="col-md-6">
				<h4 class="mb-3">Thông tin tác giả</h4>
				<div class="form-group mb-3"><label asp-for="AuthorName" class="form-label"></label><input asp-for="AuthorName" class="form-control" /><span asp-validation-for="AuthorName" class="text-danger"></span><small class="form-hint">Tên hiển thị cùng bài viết.</small></div>
				<div class="form-group mb-3" id="author-avatar-area">
					<label asp-for="AuthorAvatar" class="form-label"></label>
					<div><img id="authorAvatarPreview" src="@MediaUrlHelper.GetMinioUrl(Model.AuthorAvatar, "/img/placeholder.svg")" class="avatar avatar-xl mb-2" /></div>
					<input type="hidden" asp-for="AuthorAvatar" id="authorAvatarPathInput" />
					<div><button type="button" class="btn btn-sm btn-outline-primary select-media-btn" data-target-input="#authorAvatarPathInput" data-target-preview="#authorAvatarPreview"><i class="ti ti-photo-search me-1"></i> Chọn Avatar</button><button type="button" class="btn btn-sm btn-outline-danger remove-media-btn ms-2 @(string.IsNullOrEmpty(Model.AuthorAvatar) ? "d-none" : "")" data-target-input="#authorAvatarPathInput" data-target-preview="#authorAvatarPreview" data-placeholder="/img/placeholder.svg"><i class="ti ti-x me-1"></i> Xóa</button></div>
					<span asp-validation-for="AuthorAvatar" class="text-danger"></span>
				</div>
			</div>
		</div>
	</div>

	@* --- Tab: Relations --- *@
	<div class="tab-pane" id="article-tab-relations" role="tabpanel">
		<div class="row">
			<div class="col-md-6">
				<div class="form-group mb-3"><label asp-for="SelectedCategoryIds" class="form-label required"></label><select asp-for="SelectedCategoryIds" asp-items="@Model.CategoryList" class="form-select tom-select-multiple" multiple="multiple"></select><span asp-validation-for="SelectedCategoryIds" class="text-danger"></span><small class="form-hint">Chọn ít nhất một danh mục.</small></div>
			</div>
			<div class="col-md-6">
				<div class="form-group mb-3"><label asp-for="SelectedTagIds" class="form-label"></label><select asp-for="SelectedTagIds" asp-items="@Model.TagList" class="form-select tom-select-multiple" multiple="multiple" id="SelectedTagIds"></select><span asp-validation-for="SelectedTagIds" class="text-danger"></span><small class="form-hint">Chọn hoặc nhập thẻ mới.</small></div>
			</div>
			<div class="col-12">
				<div class="form-group mb-0"><label asp-for="SelectedProductIds" class="form-label"></label><select asp-for="SelectedProductIds" asp-items="@Model.ProductList" class="form-select tom-select-multiple" multiple="multiple" id="SelectedProductIds"></select><span asp-validation-for="SelectedProductIds" class="text-danger"></span><small class="form-hint">Chọn các sản phẩm được đề cập (nếu có).</small></div>
			</div>
		</div>
	</div>

	@* --- Tab: Publishing --- *@
	<div class="tab-pane" id="article-tab-publishing" role="tabpanel">
		<div class="row">
			<div class="col-md-6">
				<div class="form-group mb-3"><label asp-for="Status" class="form-label required"></label><select asp-for="Status" asp-items="@Model.StatusList" class="form-select tom-select-single"></select><span asp-validation-for="Status" class="text-danger"></span></div>
			</div>
			<div class="col-md-6">
				<div class="form-group mb-3"><label asp-for="PublishedAt" class="form-label"></label><input asp-for="PublishedAt" type="datetime-local" class="form-control" /><span asp-validation-for="PublishedAt" class="text-danger"></span><small class="form-hint">Để trống nếu là 'Bản nháp'. Nếu chọn 'Đã xuất bản' mà để trống thì sẽ xuất bản ngay.</small></div>
			</div>
			<div class="col-md-6">
				<div class="form-check form-switch mb-3"><input class="form-check-input" type="checkbox" asp-for="IsFeatured" role="switch" id="articleIsFeaturedSwitch"><label class="form-check-label" asp-for="IsFeatured"></label></div>
			</div>
            <div class="col-md-6">
                <div class="form-group mb-3"><label asp-for="EstimatedReadingMinutes" class="form-label"></label><input asp-for="EstimatedReadingMinutes" class="form-control" readonly disabled /><small class="form-hint">Tự động tính toán dựa trên nội dung.</small></div>
            </div>
		</div>
	</div>

	@* --- Tab: SEO --- *@
	<div class="tab-pane" id="article-tab-seo" role="tabpanel">
		<h3 class="card-title mb-3">Tối ưu SEO</h3>
		<div class="mb-3"><label asp-for="MetaTitle" class="form-label"></label><input asp-for="MetaTitle" class="form-control" /><span asp-validation-for="MetaTitle" class="text-danger"></span><small class="form-hint">Tiêu đề SEO (tối ưu < 60 ký tự).</small></div>
		<div class="mb-3"><label asp-for="MetaDescription" class="form-label"></label><textarea asp-for="MetaDescription" class="form-control" rows="3"></textarea><span asp-validation-for="MetaDescription" class="text-danger"></span><small class="form-hint">Mô tả SEO (tối ưu < 160 ký tự).</small></div>
		<div class="mb-3"><label asp-for="MetaKeywords" class="form-label"></label><input asp-for="MetaKeywords" class="form-control" /><span asp-validation-for="MetaKeywords" class="text-danger"></span><small class="form-hint">Từ khóa SEO (cách nhau bởi dấu phẩy).</small></div>
		<hr class="my-3" />
		<div class="mb-3"><label asp-for="CanonicalUrl" class="form-label"></label><input asp-for="CanonicalUrl" class="form-control" type="url" /><span asp-validation-for="CanonicalUrl" class="text-danger"></span><small class="form-hint">URL gốc nếu trùng nội dung.</small></div>
		<div class="row mb-3"><div class="col-auto"><div class="form-check form-switch"><input class="form-check-input" type="checkbox" asp-for="NoIndex" role="switch" id="seoNoIndexSwitch"><label class="form-check-label" asp-for="NoIndex"></label></div><small class="form-hint d-block ms-4 ps-2">Ngăn index.</small></div><div class="col-auto"><div class="form-check form-switch"><input class="form-check-input" type="checkbox" asp-for="NoFollow" role="switch" id="seoNoFollowSwitch"><label class="form-check-label" asp-for="NoFollow"></label></div><small class="form-hint d-block ms-4 ps-2">Ngăn follow link.</small></div></div>
		<hr class="my-3" />
		<h4 class="mb-3">Open Graph (Mạng xã hội)</h4>
		<div class="mb-3"><label asp-for="OgTitle" class="form-label"></label><input asp-for="OgTitle" class="form-control" /><span asp-validation-for="OgTitle" class="text-danger"></span></div>
		<div class="mb-3"><label asp-for="OgDescription" class="form-label"></label><textarea asp-for="OgDescription" class="form-control" rows="2"></textarea><span asp-validation-for="OgDescription" class="text-danger"></span></div>
		<div class="mb-3">
            <label asp-for="OgImage" class="form-label"></label>
            <div class="input-group">
                <input type="text" asp-for="OgImage" class="form-control" id="ogImagePathInput" placeholder="Nhập URL hoặc chọn ảnh MinIO Path" /> @* Adjusted placeholder *@
                <button class="btn btn-outline-secondary select-media-btn" type="button" data-target-input="#ogImagePathInput" data-target-preview="#ogImagePreview" title="Chọn ảnh"><i class="ti ti-photo-search"></i></button>
                <button class="btn btn-outline-danger remove-media-btn @(string.IsNullOrEmpty(Model.OgImage) ? "d-none" : "")" type="button" data-target-input="#ogImagePathInput" data-target-preview="#ogImagePreview" data-placeholder="/img/placeholder.svg" title="Xóa"><i class="ti ti-x"></i></button>
            </div>
            <span asp-validation-for="OgImage" class="text-danger"></span>
            <img id="ogImagePreview" src="@(Model.OgImage?.StartsWith("http") ?? false ? Model.OgImage : MediaUrlHelper.GetMinioUrl(Model.OgImage, "/img/placeholder.svg"))" class="img-thumbnail mt-2" style="max-height: 80px; display: @(string.IsNullOrEmpty(Model.OgImage) ? "none" : "block");" />
             <small class="form-hint">Nhập URL hoặc MinIO Path. Ảnh tỷ lệ 1.91:1.</small>
        </div>
		<div class="mb-3"><label asp-for="OgType" class="form-label"></label><input asp-for="OgType" class="form-control" /><span asp-validation-for="OgType" class="text-danger"></span></div>
		<hr class="my-3" />
		@* Add Twitter Card fields similar to Open Graph *@
		<hr class="my-3" />
		<h4 class="mb-3">Dữ liệu cấu trúc & Sitemap</h4>
        <div class="mb-3"><label asp-for="SchemaMarkup" class="form-label"></label><textarea asp-for="SchemaMarkup" class="form-control" rows="5"></textarea><span asp-validation-for="SchemaMarkup" class="text-danger"></span><small class="form-hint">JSON-LD Schema.org.</small></div>
        <div class="mb-3"><label asp-for="BreadcrumbJson" class="form-label"></label><textarea asp-for="BreadcrumbJson" class="form-control" rows="3"></textarea><span asp-validation-for="BreadcrumbJson" class="text-danger"></span><small class="form-hint">JSON cho Breadcrumbs.</small></div>
		<div class="row">
			<div class="col-md-6"><div class="form-group mb-3"><label asp-for="SitemapPriority" class="form-label"></label><input type="number" asp-for="SitemapPriority" class="form-control" step="0.1" min="0.0" max="1.0" /><span asp-validation-for="SitemapPriority" class="text-danger"></span></div></div>
			<div class="col-md-6"><div class="form-group mb-3"><label asp-for="SitemapChangeFrequency" class="form-label required"></label><select asp-for="SitemapChangeFrequency" class="form-select tom-select-single"><option value="always">Always</option><option value="hourly">Hourly</option><option value="daily">Daily</option><option value="weekly">Weekly</option><option value="monthly">Monthly</option><option value="yearly">Yearly</option><option value="never">Never</option></select><span asp-validation-for="SitemapChangeFrequency" class="text-danger"></span></div></div>
		</div>
	</div>
</div>
@* --- END OF FILE _ArticleFormFields.txt --- *@


@* --- START OF FILE Create.txt --- Updated --- *@
@using web.Areas.Admin.ViewModels.Article
@model ArticleViewModel
@{
    ViewData["Title"] = "Viết Bài mới - Hệ thống quản trị";
    // Breadcrumbs set in Controller
}

@section Styles { @* Combine styles *@
	@*<link rel="stylesheet" href="~/lib/tom-select/dist/css/tom-select.bootstrap5.min.css" asp-append-version="true"> *@
	@*<link rel="stylesheet" href="~/lib/summernote/dist/summernote-bs5.min.css" asp-append-version="true">*@
    <style>.select2-container--default .select2-selection--multiple { border-color: #d9dbde; } /* Style TomSelect */ </style>
}

<form asp-action="Create" method="post" id="article-form">
    <div class="card">
        <div class="card-header">
            <h3 class="card-title">Nội dung Bài viết</h3>
        </div>
        <div class="card-body">
            @Html.AntiForgeryToken()
            <div asp-validation-summary="ModelOnly" class="text-danger alert alert-danger mb-3"></div>
            <partial name="_ArticleFormFields" model="Model" />
        </div>
        <div class="card-footer text-end bg-transparent border-top pt-3">
            <a asp-action="Index" class="btn btn-link">Hủy</a>
            <button type="submit" class="btn btn-primary ms-2"> <i class="ti ti-device-floppy me-2"></i> Lưu bài viết </button>
        </div>
    </div>
</form>

@section Scripts {
    <script src="~/js/slug.js" asp-append-version="true"></script>
	@* Load Summernote & TomSelect if not global *@
	@* <script src="~/lib/summernote/dist/summernote-bs5.min.js" asp-append-version="true"></script> *@
	@* <script src="~/lib/summernote/dist/lang/summernote-vi-VN.min.js" asp-append-version="true"></script> *@
    @* <script src="~/lib/tom-select/dist/js/tom-select.complete.min.js" asp-append-version="true"></script> *@

    <script>
        $(document).ready(function() {
            // --- Initialize Plugins ---
            $('.tom-select-single').each(function() { new TomSelect(this, { create: false, placeholder: this.options[0].text || 'Chọn...' }); });
            $('.tom-select-multiple').each(function() { new TomSelect(this, { plugins: ['remove_button'], create: $(this).is('#SelectedTagIds'), placeholder: 'Chọn...' }); });
            $('.summernote-editor').summernote({ lang: 'vi-VN', height: 250, toolbar: [ ['style', ['style']], ['font', ['bold', 'italic', 'underline', 'clear']], ['para', ['ul', 'ol', 'paragraph']], ['insert', ['link', 'picture']], ['view', ['fullscreen', 'codeview', 'help']] ] });
             $('#articleContentEditor').summernote('height', 400); // Taller editor for main content

            // --- Slug Generation ---
            const titleInput = $('#articleTitle'); const slugInput = $('#articleSlug'); const generateBtn = $('#generateSlugButton');
            titleInput.on('input', function() { if (slugInput.val() === '') { slugInput.val(generateVietnameseSlug(titleInput.val())); } });
            generateBtn.on('click', function() { slugInput.val(generateVietnameseSlug(titleInput.val())); });

            // --- General Media Selection ---
            function handleMediaSelection(selectedFile, inputSelector, previewSelector) { /* ... (Same as Edit view JS) ... */
                 if (selectedFile && inputSelector && previewSelector) {
                     const inputField = $(inputSelector); const previewImg = $(previewSelector); const removeBtn = $(`button[data-target-input="${inputSelector}"]`).filter('.remove-media-btn');
                     const valueToStore = (inputSelector === '#ogImagePathInput' || inputSelector === '#twitterImagePathInput') ? selectedFile.url : selectedFile.path;
                     // Use path for generating preview URL, store path or URL based on input target
                     const displayUrl = '@(MediaUrlHelper.GetMinioUrl("__PATH__", "/img/placeholder.svg"))'.replace('__PATH__', selectedFile.path);
                     previewImg.attr('src', displayUrl).show();
                     inputField.val(valueToStore || '').trigger('change');
                     if(removeBtn.length) removeBtn.removeClass('d-none');
                 }
            }
             $(document).on('click', '.select-media-btn', function() { /* ... (Same as Edit view JS) ... */
                  const targetInput = $(this).data('target-input'); const targetPreview = $(this).data('target-preview');
                 if (typeof window.openMediaSelectionModal === 'function') { const cb = (sf) => handleMediaSelection(sf, targetInput, targetPreview); window.openMediaSelectionModal(cb, 'Image'); } else { console.error('openMediaSelectionModal is not defined.'); }
             });
              $(document).on('click', '.remove-media-btn', function() { /* ... (Same as Edit view JS) ... */
                  const targetInput = $(this).data('target-input'); const targetPreview = $(this).data('target-preview'); const placeholder = $(this).data('placeholder') || '/img/placeholder.svg';
                 if (targetInput && targetPreview) { $(targetPreview).attr('src', placeholder); if ($(targetPreview).is('#ogImagePreview')) $(targetPreview).hide(); $(targetInput).val('').trigger('change'); $(this).addClass('d-none'); }
              });

              // Initialize visibility for OG image preview
              if ($('#ogImagePathInput').val() === '') $('#ogImagePreview').hide(); else $('#ogImagePreview').show();
              // Add similar for Twitter if used
        });
    </script>
     @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
@* --- END OF FILE Create.txt --- *@


@* --- START OF FILE Edit.txt --- Updated --- *@
@using shared.Helpers
@using web.Areas.Admin.ViewModels.Article
@model ArticleViewModel
@{
    ViewData["Title"] = "Chỉnh sửa Bài viết - Hệ thống quản trị";
    // Breadcrumbs set in Controller
}

@section Styles {
	@* <link rel="stylesheet" href="~/lib/tom-select/dist/css/tom-select.bootstrap5.min.css" asp-append-version="true"> *@
	@* <link rel="stylesheet" href="~/lib/summernote/dist/summernote-bs5.min.css" asp-append-version="true"> *@
    <style>.select2-container--default .select2-selection--multiple { border-color: #d9dbde; } </style>
}

<form asp-action="Edit" asp-route-id="@Model.Id" method="post" id="article-form">
    <div class="card">
        <div class="card-header">
            <h3 class="card-title">Nội dung Bài viết</h3>
        </div>
        <div class="card-body">
            @Html.AntiForgeryToken()
            <input type="hidden" asp-for="Id" />
            <div asp-validation-summary="ModelOnly" class="text-danger alert alert-danger mb-3"></div>
            <partial name="_ArticleFormFields" model="Model" />
        </div>
        <div class="card-footer text-end bg-transparent border-top pt-3">
            <a asp-action="Index" class="btn btn-link">Hủy</a>
            <button type="submit" class="btn btn-primary ms-2">
                <i class="ti ti-device-floppy me-2"></i> Cập nhật bài viết
            </button>
        </div>
    </div>
</form>

@section Scripts {
    <script src="~/js/slug.js" asp-append-version="true"></script>
	@* <script src="~/lib/summernote/dist/summernote-bs5.min.js" asp-append-version="true"></script> *@
	@* <script src="~/lib/summernote/dist/lang/summernote-vi-VN.min.js" asp-append-version="true"></script> *@
    @* <script src="~/lib/tom-select/dist/js/tom-select.complete.min.js" asp-append-version="true"></script> *@

    <script>
        $(document).ready(function() {
             // --- Initialize Plugins ---
            $('.tom-select-single').each(function() { new TomSelect(this, { create: false, placeholder: this.options[0].text || 'Chọn...' }); });
            $('.tom-select-multiple').each(function() { new TomSelect(this, { plugins: ['remove_button'], create: $(this).is('#SelectedTagIds'), placeholder: 'Chọn...' }); });
            $('.summernote-editor').summernote({ lang: 'vi-VN', height: 250, toolbar: [ ['style', ['style']], ['font', ['bold', 'italic', 'underline', 'clear']], ['para', ['ul', 'ol', 'paragraph']], ['insert', ['link', 'picture']], ['view', ['fullscreen', 'codeview', 'help']] ] });
            $('#articleContentEditor').summernote('height', 400);

            // --- Slug Generation (Edit) ---
            const titleInput = $('#articleTitle'); const slugInput = $('#articleSlug'); const generateBtn = $('#generateSlugButton');
            let userModifiedSlug = (slugInput.val() !== generateVietnameseSlug(titleInput.val())) && (slugInput.val() !== '');
            slugInput.on('input', function() { userModifiedSlug = true; });
            titleInput.on('input', function() { if (!userModifiedSlug) { slugInput.val(generateVietnameseSlug(titleInput.val())); } });
            generateBtn.on('click', function() { userModifiedSlug = false; slugInput.val(generateVietnameseSlug(titleInput.val())); });

             // --- General Media Selection ---
            function handleMediaSelection(selectedFile, inputSelector, previewSelector) { /* ... (Same as Create view JS) ... */
                if (selectedFile && inputSelector && previewSelector) {
                     const inputField = $(inputSelector); const previewImg = $(previewSelector); const removeBtn = $(`button[data-target-input="${inputSelector}"]`).filter('.remove-media-btn');
                     const valueToStore = (inputSelector === '#ogImagePathInput' || inputSelector === '#twitterImagePathInput') ? selectedFile.url : selectedFile.path;
                     const displayUrl = '@(MediaUrlHelper.GetMinioUrl("__PATH__", "/img/placeholder.svg"))'.replace('__PATH__', selectedFile.path);
                     previewImg.attr('src', displayUrl).show(); inputField.val(valueToStore || '').trigger('change'); if(removeBtn.length) removeBtn.removeClass('d-none');
                }
            }
            $(document).on('click', '.select-media-btn', function() { /* ... (Same as Create view JS) ... */
                  const targetInput = $(this).data('target-input'); const targetPreview = $(this).data('target-preview');
                 if (typeof window.openMediaSelectionModal === 'function') { const cb = (sf) => handleMediaSelection(sf, targetInput, targetPreview); window.openMediaSelectionModal(cb, 'Image'); } else { console.error('openMediaSelectionModal is not defined.'); }
            });
             $(document).on('click', '.remove-media-btn', function() { /* ... (Same as Create view JS) ... */
                  const targetInput = $(this).data('target-input'); const targetPreview = $(this).data('target-preview'); const placeholder = $(this).data('placeholder') || '/img/placeholder.svg';
                 if (targetInput && targetPreview) { $(targetPreview).attr('src', placeholder); if ($(targetPreview).is('#ogImagePreview')) $(targetPreview).hide(); $(targetInput).val('').trigger('change'); $(this).addClass('d-none'); }
            });
             // Initialize visibility for OG image preview
             if ($('#ogImagePathInput').val() === '') $('#ogImagePreview').hide(); else $('#ogImagePreview').show();
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
@using web.Areas.Admin.ViewModels.Article
@using shared.Enums
@using shared.Extensions // For GetDisplayName
@model IPagedList<ArticleListItemViewModel> @* Changed model type *@
@{
    ViewData["Title"] = "Quản lý Bài viết - Hệ thống quản trị";
    // Breadcrumbs set in Controller
}

@section PageActions {
    <div class="btn-list">
        <a asp-action="Create" class="btn btn-primary d-sm-inline-block"> <i class="ti ti-file-plus me-2"></i> Viết bài mới </a>
         @* Optional Export Button *@
        @* <a asp-action="ExportExcel" asp-all-route-data="@Context.Request.Query.ToDictionary(q => q.Key, q=> q.Value.ToString())" class="btn btn-outline-success d-none d-sm-inline-block"> <i class="ti ti-download me-2"></i> Export Excel </a> *@
    </div>
}

<!-- Filter Form -->
<div class="card mb-3">
    <div class="card-body">
        <form method="get" asp-action="Index" id="filterForm" class="row g-3 align-items-end">
            <div class="col-md-3 col-sm-6">
                <label class="form-label">Tìm kiếm</label>
                <div class="input-group">
                    <input type="text" class="form-control" placeholder="Tiêu đề, tóm tắt..." name="searchTerm" value="@ViewBag.SearchTerm">
                     <button class="btn btn-icon" type="button" id="clearSearch" title="Xóa tìm kiếm"> <i class="ti ti-x"></i> </button>
                 </div>
            </div>
            <div class="col-md-2 col-sm-6">
                <label class="form-label">Danh mục</label>
                <select class="form-select tom-select-filter" name="categoryId" asp-items="@ViewBag.Categories"> @* Use TomSelect *@
                    <option value="">-- Tất cả --</option>
                </select>
            </div>
            <div class="col-md-2 col-sm-6">
                <label class="form-label">Loại bài viết</label>
                <select class="form-select tom-select-filter" name="type" asp-items="@ViewBag.Types"> @* Use TomSelect *@
                    <option value="">-- Tất cả --</option>
                </select>
            </div>
            <div class="col-md-2 col-sm-6">
                <label class="form-label">Trạng thái</label>
                <select class="form-select tom-select-filter" name="status" asp-items="@ViewBag.Statuses"> @* Use TomSelect *@
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
        <h3 class="card-title mb-0">Danh sách Bài viết</h3>
         @if (Model.TotalItemCount > 0) { <small class="text-muted">Hiển thị @Model.FirstItemOnPage-@Model.LastItemOnPage trên tổng số @Model.TotalItemCount bài viết</small> }
    </div>
    <div class="card-body p-0">
        @if (!Model.Any()) { <div class="empty"><div class="empty-icon"><i class="ti ti-file-off fa-3x text-muted"></i></div><p class="empty-title">Không tìm thấy bài viết nào</p><p class="empty-subtitle text-muted">Hãy tạo bài viết đầu tiên.</p><div class="empty-action"> <a asp-action="Create" class="btn btn-primary"> <i class="ti ti-plus me-2"></i> Viết bài mới </a> </div></div> }
        else {
            <div class="table-responsive">
                <table class="table table-vcenter card-table table-striped">
                    <thead>
                        <tr>
                            <th style="width: 5%;">Ảnh</th>
                            <th>Tiêu đề</th>
                            <th>Tác giả</th>
                            <th>Danh mục</th>
                            <th class="text-center">Loại</th>
                            <th class="text-center">Trạng thái</th>
                            <th class="text-center">Lượt xem</th>
                            <th>Ngày xuất bản</th>
                            <th class="w-1">Thao tác</th>
                        </tr>
                    </thead>
                    <tbody>
                        @foreach (var item in Model) {
                            <tr>
                                <td><img src="@MediaUrlHelper.GetMinioUrl(item.ThumbnailImage, "/img/placeholder.svg")" alt="@item.Title" class="avatar avatar-md object-fit-cover bg-light" /></td>
                                <td><a asp-action="Edit" asp-route-id="@item.Id" class="fw-medium">@item.Title</a> @if (item.IsFeatured) { <i class="ti ti-star-filled text-warning ms-1" title="Nổi bật"></i> }</td>
                                <td class="text-muted">@(item.AuthorName ?? "-")</td>
                                <td class="text-muted text-truncate" style="max-width: 150px;" title="@item.CategoryNames">@(item.CategoryNames ?? "-")</td>
                                <td class="text-center text-muted">@item.Type.GetDisplayName()</td>
                                <td class="text-center"><span class="badge bg-@(GetStatusBadgeClass(item.Status))-lt">@item.Status.GetDisplayName()</span></td>
                                <td class="text-center text-muted">@item.ViewCount</td>
                                <td class="text-muted" title="@item.PublishedAt?.ToString("dd/MM/yyyy HH:mm:ss")">@(item.PublishedAt?.ToString("dd/MM/yyyy") ?? "-")</td>
                                <td>
                                    <div class="btn-list flex-nowrap">
                                        <a asp-action="Edit" asp-route-id="@item.Id" class="btn btn-sm btn-icon btn-outline-primary" title="Chỉnh sửa"><i class="ti ti-pencil"></i></a>
                                        <a asp-controller="Comment" asp-action="Index" asp-route-articleId="@item.Id" class="btn btn-sm btn-icon btn-outline-secondary" title="Xem bình luận"><i class="ti ti-message-circle"></i></a>
                                        <button class="btn btn-sm btn-icon btn-outline-danger delete-item-btn" title="Xóa" data-id="@item.Id" data-name="@item.Title" data-delete-url="@Url.Action("Delete", "Article")"> <i class="ti ti-trash"></i> </button>
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
	@if (Model.PageCount > 1) { <div class="card-footer"><div class="d-flex align-items-center justify-content-center">@Html.PagedListPager(Model, page => Url.Action("Index", new { page, searchTerm = ViewBag.SearchTerm, categoryId = ViewBag.SelectedCategoryId, type = ViewBag.SelectedType, status = ViewBag.SelectedStatus }), PagedListRenderOptions.Bootstrap5PageNumbersOnly)</div></div> }
</div>

@* Status Badge Helper *@
@functions { string GetStatusBadgeClass(PublishStatus status) => status switch { PublishStatus.Published => "success", PublishStatus.Draft => "secondary", PublishStatus.Scheduled => "warning", PublishStatus.Archived => "dark", _ => "secondary" }; }

@section Scripts {
    <script> $(document).ready(function () { $('#clearSearch').on('click', function() { $('input[name="searchTerm"]').val(''); }); document.querySelectorAll('.tom-select-filter').forEach((el)=>{ new TomSelect(el, { create: false, allowEmptyOption: true }); }); }); </script>
 }
@* --- END OF FILE Index.txt --- *@


@* --- START OF FILE CommentController.cs --- Updated --- *@
@using web.Areas.Admin.ViewModels.Comment
@model IEnumerable<CommentListItemViewModel> @* Keep IEnumerable if not paginating comments directly here *@
@{
    ViewData["Title"] = "Quản lý Bình luận - Hệ thống quản trị";
    // Breadcrumbs set in Controller
}

@* No "Add New" button usually for comments from admin *@

<!-- Filter Form -->
<div class="card mb-3">
    <div class="card-body">
        <form method="get" asp-action="Index" id="filterForm" class="row g-3 align-items-end">
            <div class="col-md-4">
                <label class="form-label">Tìm kiếm</label>
                 <div class="input-group">
                    <input type="text" class="form-control" placeholder="Tên, email, nội dung..." name="searchTerm" value="@ViewBag.SearchTerm">
                     <button class="btn btn-icon" type="button" id="clearSearch" title="Xóa tìm kiếm"> <i class="ti ti-x"></i> </button>
                 </div>
            </div>
            <div class="col-md-3">
                <label class="form-label">Trạng thái duyệt</label>
                <select class="form-select tom-select-filter" name="isApproved" asp-items="@ViewBag.ApprovalStatusList"> @* Use ViewBag.ApprovalStatusList *@
                    <option value="">-- Tất cả --</option>
                </select>
            </div>
            <div class="col-md-3">
                <label class="form-label">Bài viết</label>
                 @* Use SelectList loaded into ViewBag *@
                <select class="form-select tom-select-filter" name="articleId" asp-items="@ViewBag.ArticleList">
                    <option value="">-- Tất cả bài viết --</option>
                </select>
            </div>
            <div class="col-md-1">
                <button type="submit" class="btn btn-primary w-100"> <i class="ti ti-filter me-1 d-none d-sm-inline-block"></i> Lọc</button>
            </div>
            <div class="col-md-1">
                <a asp-action="Index" class="btn btn-outline-secondary w-100"> <i class="ti ti-reload me-1 d-none d-sm-inline-block"></i> Đặt lại</a>
            </div>
        </form>
    </div>
</div>

<!-- Content Table -->
<div class="card">
     <div class="card-header d-flex justify-content-between align-items-center">
        <h3 class="card-title mb-0">Danh sách Bình luận</h3>
        @* No item count here if not paginated *@
    </div>
    <div class="card-body p-0">
        @if (!Model.Any()) { <div class="empty"><div class="empty-icon"><i class="ti ti-message-circle-off fa-3x text-muted"></i></div><p class="empty-title">Không có bình luận nào</p><p class="empty-subtitle text-muted">Chưa có bình luận nào được gửi.</p></div> }
        else {
            <div class="table-responsive">
                <table class="table table-vcenter card-table table-striped">
                    <thead> <tr><th>Tác giả</th><th>Bình luận</th><th>Phản hồi cho</th><th class="text-center">Trạng thái</th><th>Ngày gửi</th><th class="w-1">Thao tác</th></tr> </thead>
                    <tbody>
                        @foreach (var item in Model) {
                            <tr id="comment-row-@item.Id"> @* Add ID to row *@
                                <td> <div class="d-flex flex-column"><span class="fw-medium">@item.AuthorName</span><small class="text-muted">@item.AuthorEmail</small></div> </td>
                                <td class="text-muted">@item.ContentExcerpt</td>
                                <td> @if (item.ArticleTitle != null) { <a asp-controller="Article" asp-action="Edit" asp-route-id="@item.ArticleId" target="_blank" class="text-muted" title="@item.ArticleTitle"> <i class="ti ti-file-text me-1"></i> @(item.ArticleTitle.Length > 30 ? item.ArticleTitle.Substring(0, 30) + "..." : item.ArticleTitle) </a> } else { <span class="text-muted">N/A</span> } </td>
                                <td class="text-center status-cell"> @* Add class to target status *@
                                    @if (item.IsApproved) { <span class="badge bg-success-lt">Đã duyệt</span> }
                                    else { <span class="badge bg-warning-lt">Chờ duyệt</span> }
                                </td>
                                <td class="text-muted" title="@item.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss")">@item.CreatedAt.ToString("dd/MM/yyyy HH:mm")</td>
                                <td>
                                    <div class="btn-list flex-nowrap action-buttons"> @* Add class to target buttons *@
                                        @if (item.IsApproved) { <button class="btn btn-sm btn-icon btn-outline-warning comment-action-btn" title="Bỏ duyệt" data-action="Unapprove" data-id="@item.Id" data-url="@Url.Action("Unapprove", "Comment")"><i class="ti ti-thumb-down"></i></button> }
                                        else { <button class="btn btn-sm btn-icon btn-outline-success comment-action-btn" title="Duyệt" data-action="Approve" data-id="@item.Id" data-url="@Url.Action("Approve", "Comment")"><i class="ti ti-thumb-up"></i></button> }
                                        <a asp-action="Edit" asp-route-id="@item.Id" class="btn btn-sm btn-icon btn-outline-primary" title="Sửa"><i class="ti ti-pencil"></i></a>
                                        <button class="btn btn-sm btn-icon btn-outline-danger delete-item-btn" title="Xóa" data-id="@item.Id" data-name="Bình luận của @item.AuthorName" data-delete-url="@Url.Action("Delete", "Comment")"> <i class="ti ti-trash"></i> </button>
                                    </div>
                                </td>
                            </tr>
                        }
                    </tbody>
                </table>
            </div>
            @* Add Pagination controls here if using pagination in Controller *@
        }
    </div>
</div>

@section Scripts {
    <script>
        $(document).ready(function () {
             $('#clearSearch').on('click', function() { $('input[name="searchTerm"]').val(''); $('input[name="articleId"]').val(''); $('.tom-select-filter').each(function(){ if(this.tomselect) this.tomselect.clear(); }); /*$('#filterForm').submit();*/ });
             document.querySelectorAll('.tom-select-filter').forEach((el)=>{ new TomSelect(el, { create: false, allowEmptyOption: true }); });

            // --- AJAX for Approve/Unapprove ---
            const csrfToken = $('input[name="__RequestVerificationToken"]').val(); // Get token

             $('.comment-action-btn').on('click', function(e) {
                e.preventDefault();
                const button = $(this);
                const url = button.data('url');
                const id = button.data('id');
                const action = button.data('action'); // 'Approve' or 'Unapprove'
                const isApproving = action === 'Approve';

                button.prop('disabled', true).html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>'); // Loading indicator

                $.ajax({
                    url: url,
                    type: 'POST',
                    data: {
                        id: id,
                        __RequestVerificationToken: csrfToken
                    },
                    success: function(response) {
                        if (response.success) {
                            toastr.success(response.message);
                            // Update UI dynamically
                             const row = $(`#comment-row-${id}`);
                             const statusCell = row.find('.status-cell');
                             const actionButtons = row.find('.action-buttons');

                            // Update status badge
                            if (response.isApproved) {
                                statusCell.html('<span class="badge bg-success-lt">Đã duyệt</span>');
                            } else {
                                statusCell.html('<span class="badge bg-warning-lt">Chờ duyệt</span>');
                            }

                            // Swap approve/unapprove button
                            actionButtons.find('.comment-action-btn').remove(); // Remove existing action button
                            if (response.isApproved) {
                                actionButtons.prepend('<button class="btn btn-sm btn-icon btn-outline-warning comment-action-btn" title="Bỏ duyệt" data-action="Unapprove" data-id="' + id + '" data-url="@Url.Action("Unapprove", "Comment")"><i class="ti ti-thumb-down"></i></button>');
                            } else {
                                actionButtons.prepend('<button class="btn btn-sm btn-icon btn-outline-success comment-action-btn" title="Duyệt" data-action="Approve" data-id="' + id + '" data-url="@Url.Action("Approve", "Comment")"><i class="ti ti-thumb-up"></i></button>');
                            }

                        } else {
                            toastr.error(response.message || 'Có lỗi xảy ra.');
                             button.prop('disabled', false).html(isApproving ? '<i class="ti ti-thumb-up"></i>' : '<i class="ti ti-thumb-down"></i>'); // Restore icon
                        }
                    },
                    error: function(xhr, status, error) {
                        console.error("AJAX Error:", status, error, xhr.responseText);
                        toastr.error('Lỗi kết nối hoặc lỗi máy chủ.');
                        button.prop('disabled', false).html(isApproving ? '<i class="ti ti-thumb-up"></i>' : '<i class="ti ti-thumb-down"></i>'); // Restore icon
                    }
                    // No 'complete' needed as we handle button state in success/error
                });
            });

             // Global delete script handles .delete-item-btn
        });
    </script>
 }
@* --- END OF FILE Comment Index.txt --- *@

@* --- START OF FILE Comment Edit.txt --- Updated --- *@
@using web.Areas.Admin.ViewModels.Comment
@model CommentViewModel
@{
    ViewData["Title"] = "Chỉnh sửa Bình luận - Hệ thống quản trị";
    // Breadcrumbs set in Controller
}

<form asp-action="Edit" asp-route-id="@Model.Id" method="post"> @* Add ID route value *@
    @Html.AntiForgeryToken()
    <input type="hidden" asp-for="Id" />
    <input type="hidden" asp-for="ArticleId" />

    <div class="card">
        <div class="card-header">
            <h3 class="card-title">Chi tiết Bình luận</h3>
            <div class="card-actions">
                <a asp-action="Index" class="btn btn-link">Hủy</a>
                <button type="submit" class="btn btn-primary ms-2">
                    <i class="ti ti-device-floppy me-2"></i> Lưu thay đổi
                </button>
            </div>
        </div>
        <div class="card-body">
            <div asp-validation-summary="ModelOnly" class="text-danger alert alert-danger mb-3"></div>

            <div class="row mb-3"> @* Use row for better alignment *@
                <div class="col-md-6">
                    <label class="form-label">Bài viết:</label>
                     <p class="form-control-plaintext">
                        <a asp-controller="Article" asp-action="Edit" asp-route-id="@Model.ArticleId" target="_blank" title="Xem bài viết">@Model.ArticleTitle</a>
                    </p>
                </div>
                 <div class="col-md-6">
                    <label class="form-label">Ngày gửi:</label>
                    <p class="form-control-plaintext">@Model.CreatedAt.ToString("dd/MM/yyyy HH:mm")</p>
                </div>
            </div>

            <hr class="my-3">

            <div class="row">
                <div class="col-md-6">
                    <div class="form-group mb-3">
                        <label asp-for="AuthorName" class="form-label required"></label>
                        <input asp-for="AuthorName" class="form-control" />
                        <span asp-validation-for="AuthorName" class="text-danger"></span>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group mb-3">
                        <label asp-for="AuthorEmail" class="form-label"></label>
                        <input type="email" asp-for="AuthorEmail" class="form-control" />
                        <span asp-validation-for="AuthorEmail" class="text-danger"></span>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group mb-3">
                        <label asp-for="AuthorWebsite" class="form-label"></label>
                        <input type="url" asp-for="AuthorWebsite" class="form-control" />
                        <span asp-validation-for="AuthorWebsite" class="text-danger"></span>
                    </div>
                </div>
                 <div class="col-md-6 d-flex align-items-center"> @* Vertically align switch *@
                     <div class="form-check form-switch mb-3 mt-3"> @* Add margin for alignment *@
                         <input class="form-check-input" type="checkbox" asp-for="IsApproved" role="switch" id="commentIsApprovedSwitch">
                         <label class="form-check-label" asp-for="IsApproved"></label>
                     </div>
                 </div>
            </div>

            <div class="form-group mb-0"> @* Remove margin bottom *@
                <label asp-for="Content" class="form-label required"></label>
                <textarea asp-for="Content" class="form-control summernote-editor" rows="6"></textarea>
                <span asp-validation-for="Content" class="text-danger"></span>
            </div>

        </div>
         @* Keep footer consistent *@
        @* <div class="card-footer text-end bg-transparent border-top pt-3">
            <a asp-action="Index" class="btn btn-link">Hủy</a>
            <button type="submit" class="btn btn-primary ms-2">
                <i class="ti ti-device-floppy me-2"></i> Lưu thay đổi
            </button>
        </div> *@
    </div>
</form>

@section Scripts {
    <script>
        $(document).ready(function() {
            $('.summernote-editor').summernote({
                placeholder: 'Nội dung bình luận...',
                tabsize: 2,
                height: 150,
                toolbar: [ /* Basic toolbar */ ['font', ['bold', 'italic']], ['para', ['ul', 'ol']], ['insert', ['link']], ['view', ['codeview']] ]
            });
        });
    </script>
     @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
 }
@* --- END OF FILE Comment Edit.txt --- *@

```

**Summary of Article & Comment Refactoring:**

1.  **Entities/Configurations:** Corrected inheritance and primary keys for join tables (`ArticleCategory`, `ArticleTag`, `ArticleProduct`). Verified `Comment` relationships.
2.  **ViewModels:** Added `[Display]` attributes and `UpdatedAt` where appropriate. Implemented `ISeoPropertiesViewModel` for `ArticleViewModel`.
3.  **Mappers:** Updated list view mappings. Ensured create/edit mappings ignore non-editable/manually handled fields.
4.  **Validators:** Added `DbContext` for unique slug check in `ArticleViewModelValidator`. Used `{PropertyName}`.
5.  **Controllers:**
    *   Implemented standard patterns: pagination, logging, TempData, validation middleware, `AsNoTracking`, `ProjectTo`.
    *   Refined M-M relationship update logic in `ArticleController`.
    *   Added helper methods for loading dropdowns, calculating read time, handling publish dates, and updating junction tables.
    *   Implemented AJAX Approve/Unapprove in `CommentController`.
    *   Included logic for MinIO image deletion *after* successful DB delete in `ArticleController`.
6.  **Views:**
    *   Implemented pagination, item counts, standardized filters (using TomSelect), standardized buttons/links.
    *   Used shared `EnumExtensions.GetDisplayName()` for status/type display.
    *   Added `UpdatedAt` columns.
    *   Implemented AJAX handling for Approve/Unapprove buttons in Comment Index.
    *   Ensured necessary JS libraries (TomSelect, Summernote) are initialized.
    *   Standardized form structures and footers.
    *   Added image selection logic using the common JS pattern.

This brings both the Article and Comment management features fully in line with your established application standards and provides robust functionality. Remember to test the relationship updates and MinIO deletion logic carefully.