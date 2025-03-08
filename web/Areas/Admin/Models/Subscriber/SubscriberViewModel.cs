using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Models.Subscriber;

public class SubscriberViewModel
{
    public int Id { get; set; }

    [Display(Name = "Email")] public string? Email { get; set; }

    [Display(Name = "Trạng thái")] public string? Status { get; set; }

    [Display(Name = "Ngày tạo")] public DateTime? CreatedAt { get; set; }
}