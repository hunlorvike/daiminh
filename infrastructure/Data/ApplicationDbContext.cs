using core.Entities;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    #region Identity & Authorization

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    #endregion

    #region Common

    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Subscriber> Subscribers { get; set; }

    #endregion

    #region Content Management

    public DbSet<ContentType> ContentTypes { get; set; }
    public DbSet<ContentFieldDefinition> ContentFieldDefinitions { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<ContentFieldValue> ContentFieldValues { get; set; }
    public DbSet<ContentCategory> ContentCategories { get; set; }
    public DbSet<ContentTag> ContentTags { get; set; }
    public DbSet<ContentComment> ContentComments { get; set; }

    #endregion

    #region Product Management

    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductFieldDefinition> ProductFieldDefinitions { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductFieldValue> ProductFieldValues { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<ProductComment> ProductComments { get; set; }
    public DbSet<ProductReview> ProductReviews { get; set; }

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
        modelBuilder.ApplyConfiguration(new ContentCommentConfiguration());

        #endregion

        #region Product Management Configurations

        modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductFieldDefinitionConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductFieldValueConfiguration());
        modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductTagConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCommentConfiguration());
        modelBuilder.ApplyConfiguration(new ProductReviewConfiguration());

        #endregion
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
    }
}