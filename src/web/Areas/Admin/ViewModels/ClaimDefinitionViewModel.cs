using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class ClaimDefinitionViewModel
{
    public int Id { get; set; }

    [Display(Name = "Loại Claim", Prompt = "Nhập loại claim (ví dụ: permission)")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(50, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Type { get; set; } = string.Empty;

    [Display(Name = "Giá trị Claim", Prompt = "Nhập giá trị claim (ví dụ: Product.View)")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(50, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Value { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Mô tả ngắn gọn về claim này")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
