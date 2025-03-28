using domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    // Người dùng
    public DbSet<User> Users { get; set; }

    // Sản phẩm
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }

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

    // Thêm vào ApplicationDbContext
    public DbSet<SeoSettings> SeoSettings { get; set; }
    public DbSet<Redirect> Redirects { get; set; }
    public DbSet<SeoAnalytics> SeoAnalytics { get; set; }
    public DbSet<MediaFolder> MediaFolders { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Người dùng           
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        // Sản phẩm
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductTagConfiguration());

        // Bài viết
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleTagConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleProductConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());

        // Dự án
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectImageConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectTagConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectProductConfiguration());

        // Thư viện ảnh
        modelBuilder.ApplyConfiguration(new GalleryConfiguration());
        modelBuilder.ApplyConfiguration(new GalleryImageConfiguration());
        modelBuilder.ApplyConfiguration(new GalleryCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new GalleryTagConfiguration());

        // Phân loại
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());

        // Khác
        modelBuilder.ApplyConfiguration(new FAQConfiguration());
        modelBuilder.ApplyConfiguration(new FAQCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TestimonialConfiguration());
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
        modelBuilder.ApplyConfiguration(new NewsletterConfiguration());

        // Thêm vào OnModelCreating
        modelBuilder.ApplyConfiguration(new SeoSettingsConfiguration());
        modelBuilder.ApplyConfiguration(new RedirectConfiguration());
        modelBuilder.ApplyConfiguration(new SeoAnalyticsConfiguration());
    }
}

