using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ProjectImage : BaseEntity<int>
{
    public int ProjectId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? AltText { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int OrderIndex { get; set; } = 0;
    public bool IsMain { get; set; } = false;
    
    // Navigation properties
    public virtual Project? Project { get; set; }
}

public class ProjectImageConfiguration : BaseEntityConfiguration<ProjectImage, int>
{
    public override void Configure(EntityTypeBuilder<ProjectImage> builder)
    {
        base.Configure(builder);

        builder.ToTable("project_images");

        builder.Property(e => e.ProjectId).HasColumnName("project_id");
        builder.Property(e => e.ImageUrl).HasColumnName("image_url").IsRequired().HasMaxLength(255);
        builder.Property(e => e.ThumbnailUrl).HasColumnName("thumbnail_url").HasMaxLength(255);
        builder.Property(e => e.AltText).HasColumnName("alt_text").HasMaxLength(255);
        builder.Property(e => e.Title).HasColumnName("title").HasMaxLength(255);
        builder.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);
        builder.Property(e => e.IsMain).HasColumnName("is_main").HasDefaultValue(false);

        builder.HasIndex(e => e.ProjectId).HasDatabaseName("idx_project_images_project_id");
        builder.HasIndex(e => new { e.ProjectId, e.IsMain }).HasDatabaseName("idx_project_images_project_main");
        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_project_images_order_index");

        builder.HasOne(e => e.Project)
            .WithMany(p => p.Images)
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

