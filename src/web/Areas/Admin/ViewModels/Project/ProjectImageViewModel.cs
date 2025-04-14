using System.ComponentModel.DataAnnotations;
namespace web.Areas.Admin.ViewModels.Project;
public class ProjectImageViewModel
{
    public int Id { get; set; } // 0 for new images

    [Display(Name = "Đường dẫn ảnh (MinIO)")]
    [Required(ErrorMessage = "Đường dẫn ảnh không được rỗng")]
    [MaxLength(2048)]
    public string ImageUrl { get; set; } = string.Empty; // MinIO Path

    [Display(Name = "Đường dẫn Thumbnail (MinIO)")]
    [MaxLength(2048)]
    public string? ThumbnailUrl { get; set; } // MinIO Path

    [Display(Name = "Văn bản thay thế (Alt)")]
    [MaxLength(255)]
    public string? AltText { get; set; }

    [Display(Name = "Tiêu đề (Title)")]
    [MaxLength(255)]
    public string? Title { get; set; }

    [Display(Name = "Mô tả ảnh")]
    public string? Description { get; set; }

    [Display(Name = "Thứ tự")]
    [Range(0, int.MaxValue)]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Ảnh chính")]
    public bool IsMain { get; set; } = false;

    // Internal flag
    public bool IsDeleted { get; set; } = false;
}