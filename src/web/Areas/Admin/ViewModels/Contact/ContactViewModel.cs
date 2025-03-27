using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Contact;

public class ContactViewModel
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
    public string? AdminNotes { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<SelectListItem> StatusOptions { get; set; } = new List<SelectListItem>
    {
        new SelectListItem { Value = ContactStatus.New.ToString(), Text = "Mới" },
        new SelectListItem { Value = ContactStatus.InProgress.ToString(), Text = "Đang xử lý" },
        new SelectListItem { Value = ContactStatus.Completed.ToString(), Text = "Đã xử lý" },
        new SelectListItem { Value = ContactStatus.Spam.ToString(), Text = "Spam" }
    };
}

