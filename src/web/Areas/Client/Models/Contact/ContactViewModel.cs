using System.ComponentModel.DataAnnotations;

namespace web.Areas.Client.Models.Contact;

public class ContactViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập nội dung liên hệ")]
    public string? Message { get; set; }
}