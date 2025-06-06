using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class FAQViewModel
{
    public int Id { get; set; }

    [Display(Name = "Câu hỏi", Prompt = "Nhập câu hỏi thường gặp")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Question { get; set; } = string.Empty;

    [Display(Name = "Câu trả lời", Prompt = "Nhập câu trả lời chi tiết")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    public string Answer { get; set; } = string.Empty;

    [Display(Name = "Thứ tự hiển thị", Prompt = "Nhập số thứ tự (số nhỏ hiển thị trước)")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Hiển thị")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Danh mục")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public int CategoryId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<SelectListItem>? Categories { get; set; }
}