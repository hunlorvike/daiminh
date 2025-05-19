namespace web.Areas.Admin.ViewModels;

public class TestimonialListItemViewModel
{
    public int Id { get; set; }
    public string? ClientAvatar { get; set; }
    public string? ClientTitle { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? ClientCompany { get; set; }
    public int Rating { get; set; }
    public bool IsActive { get; set; }
    public int OrderIndex { get; set; }
    public DateTime? UpdatedAt { get; set; }
}