using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Banner;

public class BannerViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tiêu đề Banner", Prompt = "Nhập tiêu đề banner")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả ngắn gọn")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Display(Name = "URL ảnh", Prompt = "URL đến ảnh banner")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(2048, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; } = string.Empty;

    [Display(Name = "URL liên kết", Prompt = "Địa chỉ liên kết khi click vào banner")]
    [MaxLength(2048, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? LinkUrl { get; set; }

    [Display(Name = "Loại Banner")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public BannerType Type { get; set; } = BannerType.Header;

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Thứ tự hiển thị", Prompt = "Nhập số thứ tự (0 là cao nhất)")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int OrderIndex { get; set; } = 0;

    public List<SelectListItem>? TypeOptions { get; set; }
}