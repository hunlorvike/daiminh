using core.Common.Enums;
using core.Entities.Shared;

namespace core.Entities.Marketing;

public class ContactForm : AuditableEntity
{
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string Phone { get; set; } = null!;
    public string? Subject { get; set; }
    public string? Message { get; set; }
    public ContactFormStatus Status { get; set; } = ContactFormStatus.New;
    public string? IPAddress { get; set; }
}