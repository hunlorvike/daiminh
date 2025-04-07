using domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace infrastructure;

public class ApplicationDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    // Người dùng
    public DbSet<User> Users { get; set; }

    // Sản phẩm
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }

    // Bài viết
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleCategory> ArticleCategories { get; set; }
    public DbSet<ArticleTag> ArticleTags { get; set; }
    public DbSet<ArticleProduct> ArticleProducts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    // Dự án
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectImage> ProjectImages { get; set; }
    public DbSet<ProjectCategory> ProjectCategories { get; set; }
    public DbSet<ProjectTag> ProjectTags { get; set; }
    public DbSet<ProjectProduct> ProjectProducts { get; set; }

    // Thư viện ảnh
    public DbSet<Gallery> Galleries { get; set; }
    public DbSet<GalleryImage> GalleryImages { get; set; }
    public DbSet<GalleryCategory> GalleryCategories { get; set; }
    public DbSet<GalleryTag> GalleryTags { get; set; }

    // Phân loại
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }

    // Khác
    public DbSet<FAQ> FAQs { get; set; }
    public DbSet<FAQCategory> FAQCategories { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Newsletter> Newsletters { get; set; }
    public DbSet<Setting> Settings { get; set; }

    // Thêm vào ApplicationDbContext
    public DbSet<MediaFolder> MediaFolders { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Product).Assembly);
    }

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
