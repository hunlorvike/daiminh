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

        builder.ToTable("contacts");

        builder.Property(e => e.FullName).HasColumnName("full_name").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
        builder.Property(e => e.Subject).HasColumnName("subject").IsRequired().HasMaxLength(255);
        builder.Property(e => e.Message).HasColumnName("message").IsRequired().HasColumnType("text");
        builder.Property(e => e.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(ContactStatus.New);
        builder.Property(e => e.AdminNotes).HasColumnName("admin_notes").HasColumnType("text");
        builder.Property(e => e.IpAddress).HasColumnName("ip_address").HasMaxLength(50);
        builder.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(255);

        builder.HasIndex(e => e.Status).HasDatabaseName("idx_contacts_status");
        builder.HasIndex(e => e.CreatedAt).HasDatabaseName("idx_contacts_created_at");
        builder.HasIndex(e => e.Email).HasDatabaseName("idx_contacts_email");
    }
}

