using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Contact;

public class ContactDetailViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string? ProjectDetails { get; set; }
    public ContactStatus Status { get; set; } = ContactStatus.New;
    public string? AdminNotes { get; set; } // Editable notes
    public DateTime CreatedAt { get; set; } // Display only
    public string? IpAddress { get; set; } // Display only
    public string? UserAgent { get; set; } // Display only

    // Dropdown for status update
    public SelectList? StatusList { get; set; }
}