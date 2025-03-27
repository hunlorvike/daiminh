using shared.Enums;

namespace web.Areas.Admin.ViewModels.Contact;

public class ContactListItemViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public ContactStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public string StatusBadgeClass
    {
        get
        {
            return Status switch
            {
                ContactStatus.New => "bg-blue",
                ContactStatus.InProgress => "bg-yellow",
                ContactStatus.Completed => "bg-green",
                ContactStatus.Spam => "bg-red",
                _ => "bg-secondary"
            };
        }
    }

    public string StatusDisplayName
    {
        get
        {
            return Status switch
            {
                ContactStatus.New => "Mới",
                ContactStatus.InProgress => "Đang xử lý",
                ContactStatus.Completed => "Đã xử lý",
                ContactStatus.Spam => "Spam",
                _ => Status.ToString()
            };
        }
    }
}

