using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.ViewModels;

public class AttributeViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tên thuộc tính", Prompt = "Nhập tên thuộc tính (vd: Màu sắc, Dung tích)")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug (URL)", Prompt = "slug-thuoc-tinh")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Mô tả ngắn về thuộc tính")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Description { get; set; }
}
