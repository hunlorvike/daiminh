using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Slide;

public class SlideListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;    
    public string ImageUrl { get; set; } = string.Empty;
    public string? CtaLink { get; set; }
    public bool IsActive { get; set; }
    public int OrderIndex { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}