using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;
using shared.Models;

namespace domain.Entities;

public class Project : SeoEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Client { get; set; }
    public string? Location { get; set; }
    public decimal? Area { get; set; } // Diện tích (m²)
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string? FeaturedImage { get; set; }
    public string? ThumbnailImage { get; set; }
    public int ViewCount { get; set; } = 0;
    public bool IsFeatured { get; set; } = false;
    public ProjectStatus Status { get; set; } = ProjectStatus.InProgress;
    public PublishStatus PublishStatus { get; set; } = PublishStatus.Draft;

    // Navigation properties
    public virtual ICollection<ProjectImage>? Images { get; set; }
    public virtual ICollection<ProjectCategory>? ProjectCategories { get; set; }
    public virtual ICollection<ProjectTag>? ProjectTags { get; set; }
    public virtual ICollection<ProjectProduct>? ProjectProducts { get; set; }
}

public class ProjectConfiguration : SeoEntityConfiguration<Project, int>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        base.Configure(builder);

        builder.ToTable("projects");

        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Slug).HasColumnName("slug").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
        builder.Property(e => e.ShortDescription).HasColumnName("short_description").HasMaxLength(500);
        builder.Property(e => e.Client).HasColumnName("client").HasMaxLength(255);
        builder.Property(e => e.Location).HasColumnName("location").HasMaxLength(255);
        builder.Property(e => e.Area).HasColumnName("area").HasPrecision(10, 2);
        builder.Property(e => e.StartDate).HasColumnName("start_date");
        builder.Property(e => e.CompletionDate).HasColumnName("completion_date");
        builder.Property(e => e.FeaturedImage).HasColumnName("featured_image").HasMaxLength(255);
        builder.Property(e => e.ThumbnailImage).HasColumnName("thumbnail_image").HasMaxLength(255);
        builder.Property(e => e.ViewCount).HasColumnName("view_count").HasDefaultValue(0);
        builder.Property(e => e.IsFeatured).HasColumnName("is_featured").HasDefaultValue(false);
        builder.Property(e => e.Status)
            .HasColumnName("status")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<ProjectStatus>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(ProjectStatus.InProgress);
        builder.Property(e => e.PublishStatus)
            .HasColumnName("publish_status")
            .HasConversion(
                v => v.ToString().ToLowerInvariant(),
                v => Enum.Parse<PublishStatus>(v, true))
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(PublishStatus.Draft);

        builder.HasIndex(e => e.Slug).HasDatabaseName("idx_projects_slug").IsUnique();
        builder.HasIndex(e => e.Status).HasDatabaseName("idx_projects_status");
        builder.HasIndex(e => e.PublishStatus).HasDatabaseName("idx_projects_publish_status");
        builder.HasIndex(e => e.ViewCount).HasDatabaseName("idx_projects_view_count");
        builder.HasIndex(e => e.IsFeatured).HasDatabaseName("idx_projects_is_featured");
        builder.HasIndex(e => e.StartDate).HasDatabaseName("idx_projects_start_date");
        builder.HasIndex(e => e.CompletionDate).HasDatabaseName("idx_projects_completion_date");
    }
}

