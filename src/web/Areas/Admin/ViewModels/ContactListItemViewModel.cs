using System.ComponentModel.DataAnnotations;
using shared.Enums;

namespace web.Areas.Admin.ViewModels;

public class ContactListItemViewModel
{
    public int Id { get; set; }

    [Display(Name = "Người gửi")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Chủ đề")]
    public string Subject { get; set; } = string.Empty;

    [Display(Name = "Trạng thái")]
    public ContactStatus Status { get; set; }

    [Display(Name = "Ngày gửi")]
    public DateTime CreatedAt { get; set; }
}