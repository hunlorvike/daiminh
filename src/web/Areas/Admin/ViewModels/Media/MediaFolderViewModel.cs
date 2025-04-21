using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Media;

public class MediaFolderViewModel
{
    public int Id { get; set; }
    [Display(Name = "Tên thư mục", Prompt = "Nhập tên thư mục")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; } // Null for root folders
    public string? ParentName { get; set; } // For display
    public int FileCount { get; set; } // Optional: count files directly inside
    public int FolderCount { get; set; } // Optional: count child folders
}