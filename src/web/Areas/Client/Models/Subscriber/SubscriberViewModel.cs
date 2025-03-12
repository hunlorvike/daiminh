using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Client.Models.Subscriber;

public class SubscriberViewModel
{
    public int Id { get; set; }
    [DisplayName("Địa chỉ email")] public string? Email { get; set; }
    [Display(Name = "Trạng thái")] public string? Status { get; set; }
    [DisplayName("Ngày tạo")] public DateTime? CreatedAt { get; set; }
}