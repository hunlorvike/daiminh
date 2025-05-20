using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;

public class RoleClaim : IdentityRoleClaim<int>
{
    public int ClaimDefinitionId { get; set; }

    public virtual ClaimDefinition? ClaimDefinition { get; set; }
}

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("RoleClaims");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ClaimDefinitionId).IsRequired();
        builder.HasOne(e => e.ClaimDefinition)
            .WithMany()
            .HasForeignKey(e => e.ClaimDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}