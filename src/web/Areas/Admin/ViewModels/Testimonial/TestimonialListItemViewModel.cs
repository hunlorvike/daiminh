namespace web.Areas.Admin.ViewModels.Testimonial;

public class TestimonialListItemViewModel
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? ClientTitle { get; set; }
    public string? ClientCompany { get; set; }
    public string? ClientAvatar { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public bool IsActive { get; set; }
    public int OrderIndex { get; set; }
    public string? ProjectReference { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}