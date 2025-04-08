namespace web.Areas.Admin.ViewModels.Newsletter;

public class NewsletterListItemViewModel
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Name { get; set; }
    public bool IsActive { get; set; } // Trạng thái đăng ký
    public DateTime CreatedAt { get; set; } // Ngày đăng ký
    public DateTime? ConfirmedAt { get; set; } // Ngày xác nhận (nếu có double opt-in)
    public DateTime? UnsubscribedAt { get; set; } // Ngày hủy đăng ký
}