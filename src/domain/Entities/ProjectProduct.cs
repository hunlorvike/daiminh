using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ProjectProduct : BaseEntity<int>
{
    public int ProjectId { get; set; }
    public int ProductId { get; set; }
    public string? Usage { get; set; } // Cách sử dụng sản phẩm trong dự án
    public int OrderIndex { get; set; } = 0;

    // Navigation properties
    public virtual Project? Project { get; set; }
    public virtual Product? Product { get; set; }
}

public class ProjectProductConfiguration : BaseEntityConfiguration<ProjectProduct, int>
{
    public override void Configure(EntityTypeBuilder<ProjectProduct> builder)
    {
        base.Configure(builder);

        builder.ToTable("project_products");

        builder.Property(e => e.ProjectId).HasColumnName("project_id");
        builder.Property(e => e.ProductId).HasColumnName("product_id");
        builder.Property(e => e.Usage).HasColumnName("usage").HasColumnType("text");
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").HasDefaultValue(0);

        builder.HasIndex(e => new { e.ProjectId, e.ProductId })
            .HasDatabaseName("idx_project_products_project_product")
            .IsUnique();
        builder.HasIndex(e => e.ProjectId).HasDatabaseName("idx_project_products_project_id");
        builder.HasIndex(e => e.ProductId).HasDatabaseName("idx_project_products_product_id");
        builder.HasIndex(e => e.OrderIndex).HasDatabaseName("idx_project_products_order_index");

        builder.HasOne(e => e.Project)
            .WithMany(p => p.ProjectProducts)
            .HasForeignKey(e => e.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Product)
            .WithMany(p => p.ProjectProducts)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

