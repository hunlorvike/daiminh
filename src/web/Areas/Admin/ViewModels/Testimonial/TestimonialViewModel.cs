namespace web.Areas.Admin.ViewModels.Testimonial;

public class TestimonialViewModel
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? ClientTitle { get; set; }
    public string? ClientCompany { get; set; }
    public string? ClientAvatar { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public bool IsActive { get; set; } = true;
    public int OrderIndex { get; set; } = 0;
    public string? ProjectReference { get; set; }
}