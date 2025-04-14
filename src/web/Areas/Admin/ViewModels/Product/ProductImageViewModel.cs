using System.ComponentModel.DataAnnotations;
namespace web.Areas.Admin.ViewModels.Product;
public class ProductImageViewModel
{
    public int Id { get; set; } // 0 for new images

    [Display(Name = "Đường dẫn ảnh (MinIO)", Prompt = "Đường dẫn file ảnh trên MinIO")]
    [Required(ErrorMessage = "Đường dẫn ảnh không được để trống")]
    public string ImageUrl { get; set; } = string.Empty; // MinIO Path

    [Display(Name = "Đường dẫn ảnh thu nhỏ (MinIO)", Prompt = "Đường dẫn thumbnail (nếu có)")]
    public string? ThumbnailUrl { get; set; } // MinIO Path

    [Display(Name = "Văn bản thay thế (Alt Text)", Prompt = "Mô tả ngắn gọn cho ảnh (SEO)")]
    [MaxLength(255)]
    public string? AltText { get; set; }

    [Display(Name = "Tiêu đề ảnh (Title)", Prompt = "Tiêu đề hiển thị khi hover (không bắt buộc)")]
    [MaxLength(255)]
    public string? Title { get; set; }

    [Display(Name = "Thứ tự")]
    [Range(0, int.MaxValue)]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Ảnh chính")]
    public bool IsMain { get; set; } = false;

    // Not displayed, used internally for Edit logic
    public bool IsDeleted { get; set; } = false;
}