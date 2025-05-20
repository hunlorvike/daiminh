using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class ClaimDefinition : BaseEntity<int>
{
    public string Type { get; set; } = string.Empty; // e.g., "permission" ... 
    public string Value { get; set; } = string.Empty; // e.g., "Product.View" ...
    public string? Description { get; set; }
}

public class ClaimDefinitionConfiguration : BaseEntityConfiguration<ClaimDefinition, int>
{
    public override void Configure(EntityTypeBuilder<ClaimDefinition> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Type).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Value).IsRequired().HasMaxLength(50);
        builder.HasIndex(e => e.Value).IsUnique();
        builder.Property(e => e.Description).HasMaxLength(255);
    }
}