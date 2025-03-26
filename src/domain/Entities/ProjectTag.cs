using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Models;

namespace domain.Entities;

public class ProjectTag : BaseEntity<int>
{
    public int ProjectId { get; set; }
    public int TagId { get; set; }

    // Navigation properties
    public virtual Project? Project { get; set; }
    public virtual Tag? Tag { get; set; }
}

public class ProjectTagConfiguration : BaseEntityConfiguration<ProjectTag, int>
{
    public override void Configure(EntityTypeBuilder<ProjectTag> builder)
    {
        base.Configure(builder);

        builder.ToTable("project_tags");

        builder.Property(e => e.ProjectId).HasColumnName("project_id");
        builder.Property(e => e.TagId).HasColumnName("tag_id");

        builder.HasIndex(e => new { e.ProjectId, e.TagId })
            .HasDatabaseName("idx_project_tags_project_tag")
            .IsUnique();
        builder.HasIndex(e => e.ProjectId).HasDatabaseName("idx_project_tags_project_id");
        builder.HasIndex(e => e.TagId).HasDatabaseName("idx_project_tags_tag_id");

        builder.HasOne(e => e.Project)
            .WithMany(p => p.ProjectTags)
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Tag)
            .WithMany(t => t.ProjectTags)
            .HasForeignKey(e => e.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

