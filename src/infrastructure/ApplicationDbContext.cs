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
}

