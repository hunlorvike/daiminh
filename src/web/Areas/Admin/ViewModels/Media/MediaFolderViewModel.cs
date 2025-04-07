using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Media;

public class MediaFolderViewModel
{
    public int Id { get; set; } // 0 for Create

    [Display(Name = "Tên thư mục")]
    [Required(ErrorMessage = "Vui lòng nhập tên thư mục.")]
    [StringLength(100, ErrorMessage = "Tên thư mục không được vượt quá 100 ký tự.")]
    [RegularExpression(@"^[a-zA-Z0-9_~\-.\s]+$", ErrorMessage = "Tên thư mục chứa ký tự không hợp lệ.")] // Allow spaces, adjust as needed
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Mô tả")]
    [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự.")]
    public string? Description { get; set; }

    [Display(Name = "Thư mục cha")]
    public int? ParentId { get; set; } // ID của thư mục cha (null nếu là thư mục gốc)
}