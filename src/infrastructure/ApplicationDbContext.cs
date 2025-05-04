using domain.Entities;
using domain.Entities.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace infrastructure;

public partial class ApplicationDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleProduct> ArticleProducts { get; set; }
    public DbSet<ArticleTag> ArticleTags { get; set; }
    public DbSet<domain.Entities.Attribute> Attributes { get; set; }
    public DbSet<AttributeValue> AttributeValues { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<FAQ> FAQs { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }
    public DbSet<Newsletter> Newsletters { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<ProductVariation> ProductVariations { get; set; }
    public DbSet<ProductVariationAttributeValue> ProductVariationAttributeValues { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<PopupModal> PopupModals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cần giữ lại để Identity hoạt động (nếu có) và các cấu hình base khác
        base.OnModelCreating(modelBuilder);

        // Độc lập hoặc ít phụ thuộc cho seeding ban đầu
        modelBuilder.ApplyConfiguration(new UserConfiguration()); // User có thể được tham chiếu bởi ProductReview
        modelBuilder.ApplyConfiguration(new BrandConfiguration()); // Brand được tham chiếu bởi Product
        modelBuilder.ApplyConfiguration(new AttributeConfiguration()); // Attribute được tham chiếu bởi AttributeValue và ProductAttribute
        modelBuilder.ApplyConfiguration(new CategoryConfiguration()); // Category được tham chiếu bởi Article, Product, FAQ
        modelBuilder.ApplyConfiguration(new TagConfiguration());     // Tag được tham chiếu bởi ArticleTag, ProductTag
        modelBuilder.ApplyConfiguration(new SettingConfiguration()); // Độc lập, chứa nhiều HasData
        modelBuilder.ApplyConfiguration(new ContactConfiguration()); // Độc lập
        modelBuilder.ApplyConfiguration(new MediaFileConfiguration()); // Độc lập
        modelBuilder.ApplyConfiguration(new NewsletterConfiguration()); // Độc lập
        modelBuilder.ApplyConfiguration(new TestimonialConfiguration()); // Độc lập

        // Các thực thể phụ thuộc vào các thực thể trên
        modelBuilder.ApplyConfiguration(new AttributeValueConfiguration()); // Phụ thuộc Attribute
        modelBuilder.ApplyConfiguration(new ProductConfiguration()); // Phụ thuộc Brand, Category
        modelBuilder.ApplyConfiguration(new ArticleConfiguration()); // Phụ thuộc Category
        modelBuilder.ApplyConfiguration(new FAQConfiguration());      // Phụ thuộc Category (ĐẶT SAU Category để fix lỗi FK)


        // Các bảng join và thực thể chi tiết (thường không có HasData phức tạp,
        // nhưng cấu hình mối quan hệ vẫn cần các bảng chính tồn tại)
        modelBuilder.ApplyConfiguration(new ProductVariationConfiguration()); // Phụ thuộc Product
        modelBuilder.ApplyConfiguration(new ProductImageConfiguration()); // Phụ thuộc Product
        modelBuilder.ApplyConfiguration(new ProductReviewConfiguration()); // Phụ thuộc Product, User

        // Các bảng Many-to-Many Join Tables
        modelBuilder.ApplyConfiguration(new ArticleProductConfiguration()); // Phụ thuộc Article, Product
        modelBuilder.ApplyConfiguration(new ArticleTagConfiguration());   // Phụ thuộc Article, Tag
        modelBuilder.ApplyConfiguration(new ProductAttributeConfiguration()); // Phụ thuộc Product, Attribute
        modelBuilder.ApplyConfiguration(new ProductTagConfiguration());     // Phụ thuộc Product, Tag
        modelBuilder.ApplyConfiguration(new ProductVariationAttributeValueConfiguration()); // Phụ thuộc ProductVariation, AttributeValue

    }
}

public partial class ApplicationDbContext
{
    public override int SaveChanges()
    {
        UpdateTrackingFields();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTrackingFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTrackingFields()
    {
        var currentUser = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity<int> &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity<int>)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedBy = currentUser;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = currentUser;
            }
            else
            {
                entry.Property("CreatedAt").IsModified = false;
                entry.Property("CreatedBy").IsModified = false;
            }

            if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = currentUser;
            }
        }
    }
}