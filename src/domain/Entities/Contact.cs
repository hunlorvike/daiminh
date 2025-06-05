using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Contact : BaseEntity<int>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public ContactStatus Status { get; set; } = ContactStatus.New;
    public string? AdminNotes { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}

public class ContactConfiguration : BaseEntityConfiguration<Contact, int>
{
    public override void Configure(EntityTypeBuilder<Contact> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.FullName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(e => e.Email);

        builder.Property(e => e.Phone).HasMaxLength(20);
        builder.Property(e => e.Subject).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Message).IsRequired().HasColumnType("TEXT");
        builder.Property(e => e.Status)
            .IsRequired()
            .HasDefaultValue(ContactStatus.New);

        builder.Property(e => e.AdminNotes).HasColumnType("TEXT");
        builder.Property(e => e.IpAddress).HasMaxLength(50);
        builder.Property(e => e.UserAgent).HasMaxLength(255);
    }
}