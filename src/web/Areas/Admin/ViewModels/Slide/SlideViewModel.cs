using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.ViewModels.Slide;

public class SlideViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tiêu đề chính", Prompt = "Nhập tiêu đề chính của slide")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(150, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Tiêu đề phụ", Prompt = "Nhập tiêu đề phụ (nếu có)")]
    [MaxLength(150, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Subtitle { get; set; }

    [Display(Name = "Mô tả ngắn gọn", Prompt = "Nhập mô tả ngắn")]
    [MaxLength(500, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Display(Name = "URL ảnh nền", Prompt = "URL đến ảnh nền của slide")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; } = string.Empty;

    [Display(Name = "Text nút CTA", Prompt = "Ví dụ: Xem ngay, Mua sắm")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? CtaText { get; set; }

    [Display(Name = "URL nút CTA", Prompt = "Địa chỉ liên kết khi click nút CTA")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.Url)]
    public string? CtaLink { get; set; }

    [Display(Name = "Target liên kết", Prompt = "Ví dụ: _self, _blank")]
    [MaxLength(10, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Target { get; set; } = "_self";

    [Display(Name = "Thứ tự hiển thị", Prompt = "Nhập số thứ tự (0 là cao nhất)")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Thời gian bắt đầu hiển thị")]
    public DateTime? StartAt { get; set; }

    [Display(Name = "Thời gian kết thúc hiển thị")]
    public DateTime? EndAt { get; set; }
}