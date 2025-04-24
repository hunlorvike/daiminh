using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Product;

public class ProductImageViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "URL ảnh")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; } = string.Empty;

    [Display(Name = "URL ảnh thumbnail")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.ImageUrl)]
    public string? ThumbnailUrl { get; set; }

    [Display(Name = "Alt Text", Prompt = "Mô tả ảnh cho SEO và hỗ trợ tiếp cận")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? AltText { get; set; }

    [Display(Name = "Tiêu đề ảnh", Prompt = "Tiêu đề hiển thị khi rê chuột")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Title { get; set; }

    [Display(Name = "Thứ tự")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Ảnh chính")]
    public bool IsMain { get; set; } = false;

    [HiddenInput]
    public bool _Delete { get; set; } = false;

}
