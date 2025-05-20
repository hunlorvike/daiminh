using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace domain.Entities;
public class UserClaim : IdentityUserClaim<int>
{
    public int ClaimDefinitionId { get; set; }

    public virtual ClaimDefinition? ClaimDefinition { get; set; }
}

public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable("UserClaims");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ClaimDefinitionId).IsRequired();
        builder.HasOne(e => e.ClaimDefinition)
            .WithMany()
            .HasForeignKey(e => e.ClaimDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}