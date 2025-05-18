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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BrandConfiguration());
        modelBuilder.ApplyConfiguration(new AttributeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new SettingConfiguration());
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
        modelBuilder.ApplyConfiguration(new MediaFileConfiguration());
        modelBuilder.ApplyConfiguration(new NewsletterConfiguration());
        modelBuilder.ApplyConfiguration(new TestimonialConfiguration());
        modelBuilder.ApplyConfiguration(new BannerConfiguration());
        modelBuilder.ApplyConfiguration(new PageConfiguration());
        modelBuilder.ApplyConfiguration(new PopupModalConfiguration());
        modelBuilder.ApplyConfiguration(new SlideConfiguration());

        modelBuilder.ApplyConfiguration(new AttributeValueConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new FAQConfiguration());

        modelBuilder.ApplyConfiguration(new ProductVariationConfiguration());
        modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
        modelBuilder.ApplyConfiguration(new ProductReviewConfiguration());

        modelBuilder.ApplyConfiguration(new ArticleProductConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleTagConfiguration());
        modelBuilder.ApplyConfiguration(new ProductAttributeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductTagConfiguration());
        modelBuilder.ApplyConfiguration(new ProductVariationAttributeValueConfiguration());
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