using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Models.Contact;

public class ContactViewModel
{
    public int Id { get; set; }

    [Display(Name = "Họ và tên")] public string? Name { get; set; }

    [Display(Name = "Email")] public string? Email { get; set; }

    [Display(Name = "Số điện thoại")] public string? Phone { get; set; }

    [Display(Name = "Trạng thái")] public string? Status { get; set; }
    [Display(Name = "Nội dung")] public string? Message { get; set; }

    [Display(Name = "Ngày tạo")] public DateTime? CreatedAt { get; set; }
}