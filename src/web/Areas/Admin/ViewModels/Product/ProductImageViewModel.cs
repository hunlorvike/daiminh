using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Product;

public class ProductImageViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    [Display(Name = "URL Ảnh")]
    public string ImageUrl { get; set; } = string.Empty;
    [Display(Name = "URL Thumbnail")]
    public string? ThumbnailUrl { get; set; }
    [Display(Name = "Alt Text")]
    [MaxLength(255)]
    public string? AltText { get; set; }
    [Display(Name = "Tiêu đề")]
    [MaxLength(255)]
    public string? Title { get; set; }
    [Display(Name = "Thứ tự")]
    public int OrderIndex { get; set; } = 0;
    [Display(Name = "Ảnh chính")]
    public bool IsMain { get; set; } = false;
    public bool IsDeleted { get; set; }
}