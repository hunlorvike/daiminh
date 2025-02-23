using System.ComponentModel;

namespace web.Areas.Admin.Models.Contact;

public class ContactViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Message { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}