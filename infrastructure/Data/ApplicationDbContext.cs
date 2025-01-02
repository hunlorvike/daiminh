using core.Configuration.CMS;
using core.Configuration.Ecommerce;
using core.Configuration.Marketing;
using core.Configuration.Settings;
using core.Configuration.Users;
using core.Entities.CMS;
using core.Entities.Ecommerce;
using core.Entities.Marketing;
using core.Entities.Settings;
using core.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<EmailTemplate> EmailTemplates { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Page> Pages { get; set; } = null!;
    public DbSet<Manufacturer> Manufacturers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductMedia> ProductMedias { get; set; } = null!;
    public DbSet<SeoMetadata> SeoMetadata { get; set; } = null!;
    public DbSet<ContactForm> ContactForms { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new SettingConfiguration());
        modelBuilder.ApplyConfiguration(new EmailTemplateConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new PageConfiguration());
        modelBuilder.ApplyConfiguration(new ManufacturerConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductMediaConfiguration());
        modelBuilder.ApplyConfiguration(new SeoMetadataConfiguration());
        modelBuilder.ApplyConfiguration(new ContactFormConfiguration());

        // Global query filters
        modelBuilder.Entity<User>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<Category>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<Tag>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<Post>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<Page>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<Manufacturer>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<Product>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<ContactForm>().HasQueryFilter(x => x.DeletedAt == null);
    }
}