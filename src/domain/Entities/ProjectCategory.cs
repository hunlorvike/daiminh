using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ProjectCategory : BaseEntity<int>
{
    public int ProjectId { get; set; }
    public int CategoryId { get; set; }

    // Navigation properties
    public virtual Project? Project { get; set; }
    public virtual Category? Category { get; set; }
}

public class ProjectCategoryConfiguration : BaseEntityConfiguration<ProjectCategory, int>
{
    public override void Configure(EntityTypeBuilder<ProjectCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("project_categories");

        builder.Property(e => e.ProjectId).HasColumnName("project_id");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");

        builder.HasIndex(e => new { e.ProjectId, e.CategoryId })
            .HasDatabaseName("idx_project_categories_project_category")
            .IsUnique();
        builder.HasIndex(e => e.ProjectId).HasDatabaseName("idx_project_categories_project_id");
        builder.HasIndex(e => e.CategoryId).HasDatabaseName("idx_project_categories_category_id");

        builder.HasOne(e => e.Project)
            .WithMany(p => p.ProjectCategories)
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.ProjectCategories)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

