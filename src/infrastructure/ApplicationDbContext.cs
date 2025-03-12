using domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace infrastructure;

/// <summary>
/// The main database context for the application.  This class handles interaction with the database,
/// including querying, saving, and managing entities.
/// </summary>
/// <remarks>
/// This DbContext uses an <see cref="AuditSaveChangesInterceptor"/> to automatically manage audit fields (CreatedAt, UpdatedAt, DeletedAt) on entities.
/// </remarks>
/// <param name="options">The options for configuring the DbContext.</param>
/// <param name="auditSaveChangesInterceptor">The interceptor for automatically updating audit fields.</param>
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    AuditSaveChangesInterceptor auditSaveChangesInterceptor)
    : DbContext(options)
{
    #region Identity & Authorization

    /// <summary>
    /// Gets or sets the DbSet for Users.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Roles.
    /// </summary>
    public DbSet<Role> Roles { get; set; } = null!;

    #endregion

    #region Common

    /// <summary>
    /// Gets or sets the DbSet for Categories.
    /// </summary>
    public DbSet<Category> Categories { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Tags.
    /// </summary>
    public DbSet<Tag> Tags { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Settings.
    /// </summary>
    public DbSet<Setting> Settings { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Contacts.
    /// </summary>
    public DbSet<Contact> Contacts { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Subscribers.
    /// </summary>
    public DbSet<Subscriber> Subscribers { get; set; } = null!;
    /// <summary>
    /// Gets or sets the DbSet for Slider.
    /// </summary>
    public DbSet<Slider> Sliders { get; set; } = null!;

    #endregion

    #region Content Management

    /// <summary>
    /// Gets or sets the DbSet for ContentTypes.
    /// </summary>
    public DbSet<ContentType> ContentTypes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for ContentFieldDefinitions.
    /// </summary>
    public DbSet<ContentFieldDefinition> ContentFieldDefinitions { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Contents.
    /// </summary>
    public DbSet<Content> Contents { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for ContentFieldValues.
    /// </summary>
    public DbSet<ContentFieldValue> ContentFieldValues { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for ContentCategories (join table).
    /// </summary>
    public DbSet<ContentCategory> ContentCategories { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for ContentTags (join table).
    /// </summary>
    public DbSet<ContentTag> ContentTags { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Comments.
    /// </summary>
    public DbSet<Comment> Comments { get; set; } = null!;

    #endregion

    #region Product Management

    /// <summary>
    /// Gets or sets the DbSet for ProductTypes.
    /// </summary>
    public DbSet<ProductType> ProductTypes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for ProductFieldDefinitions.
    /// </summary>
    public DbSet<ProductFieldDefinition> ProductFieldDefinitions { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Products.
    /// </summary>
    public DbSet<Product> Products { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for ProductFieldValues.
    /// </summary>
    public DbSet<ProductFieldValue> ProductFieldValues { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for ProductImages.
    /// </summary>
    public DbSet<ProductImage> ProductImages { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for ProductCategories (join table).
    /// </summary>
    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for ProductTags (join table).
    /// </summary>
    public DbSet<ProductTag> ProductTags { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DbSet for Reviews.
    /// </summary>
    public DbSet<Review> Reviews { get; set; } = null!;

    #endregion

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types
    /// exposed in <see cref="DbSet{TEntity}"/> properties on your derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    /// <remarks>
    /// This method applies entity configurations defined in separate configuration classes.  It's organized
    /// into sections for Identity & Authorization, Common, Content Management, and Product Management.
    /// </remarks>
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
        modelBuilder.ApplyConfiguration(new SliderConfiguration());

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

    /// <summary>
    /// Configures the context, including adding the <see cref="AuditSaveChangesInterceptor"/>.
    /// </summary>
    /// <param name="optionsBuilder">The builder used to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(auditSaveChangesInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}