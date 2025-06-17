using domain.Entities;
using domain.Entities.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace infrastructure;

public partial class ApplicationDbContext : IdentityDbContext<User, Role, int,
    UserClaim, UserRole, UserLogin,
    RoleClaim, UserToken>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor
    ) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Identity tables (explicitly hiding base class members)
    public new DbSet<User> Users { get; set; }
    public new DbSet<Role> Roles { get; set; }
    public new DbSet<UserRole> UserRoles { get; set; }
    public new DbSet<RoleClaim> RoleClaims { get; set; }
    public new DbSet<UserClaim> UserClaims { get; set; }
    public new DbSet<UserLogin> UserLogins { get; set; }
    public new DbSet<UserToken> UserTokens { get; set; }

    // Domain tables
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleTag> ArticleTags { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ClaimDefinition> ClaimDefinitions { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<FAQ> FAQs { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }
    public DbSet<Newsletter> Newsletters { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<PopupModal> PopupModals { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(User).Assembly);
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
                entity.CreatedAt = DateTime.Now;
                entity.CreatedBy = currentUser;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = currentUser;
            }
            else
            {
                entry.Property(nameof(BaseEntity<int>.CreatedAt)).IsModified = false;
                entry.Property(nameof(BaseEntity<int>.CreatedBy)).IsModified = false;
            }

            if (entry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = currentUser;
            }
        }
    }
}
