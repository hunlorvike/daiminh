using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class BrandViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tên thương hiệu", Prompt = "Nhập tên thương hiệu")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug (URL)", Prompt = "phần-url-thân-thiện-không-dấu")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả chi tiết về thương hiệu")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Display(Name = "Logo URL", Prompt = "Nhập đường dẫn URL đến logo")]
    [MaxLength(2048, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? LogoUrl { get; set; }

    [Display(Name = "Website", Prompt = "Nhập địa chỉ website của thương hiệu")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.Url)]
    public string? Website { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;
}