using core.Entities;
using core.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    AuditSaveChangesInterceptor auditSaveChangesInterceptor)
    : DbContext(options)
{
    #region Identity & Authorization

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;

    #endregion

    #region Common

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<Subscriber> Subscribers { get; set; } = null!;

    #endregion

    #region Content Management

    public DbSet<ContentType> ContentTypes { get; set; } = null!;
    public DbSet<ContentFieldDefinition> ContentFieldDefinitions { get; set; } = null!;
    public DbSet<Content> Contents { get; set; } = null!;
    public DbSet<ContentFieldValue> ContentFieldValues { get; set; } = null!;
    public DbSet<ContentCategory> ContentCategories { get; set; } = null!;
    public DbSet<ContentTag> ContentTags { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    #endregion

    #region Product Management

    public DbSet<ProductType> ProductTypes { get; set; } = null!;
    public DbSet<ProductFieldDefinition> ProductFieldDefinitions { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductFieldValue> ProductFieldValues { get; set; } = null!;
    public DbSet<ProductImage> ProductImages { get; set; } = null!;
    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
    public DbSet<ProductTag> ProductTags { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Identity & Authorization Configurations

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());

        #endregion

        #region Common Configurations

        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new SettingConfiguration());
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriberConfiguration());

        #endregion

        #region Content Management Configurations

        modelBuilder.ApplyConfiguration(new ContentTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ContentFieldDefinitionConfiguration());
        modelBuilder.ApplyConfiguration(new ContentConfiguration());
        modelBuilder.ApplyConfiguration(new ContentFieldValueConfiguration());
        modelBuilder.ApplyConfiguration(new ContentCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ContentTagConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());

        #endregion

        #region Product Management Configurations

        modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductFieldDefinitionConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductFieldValueConfiguration());
        modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductTagConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());

        #endregion
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(auditSaveChangesInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}