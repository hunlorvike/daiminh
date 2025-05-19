using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class TagViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} không được để trống")]
    [Display(Name = "Tên thẻ", Prompt = "Nhập tên thẻ (tag)")]
    [MaxLength(50, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} không được để trống")]
    [Display(Name = "Slug", Prompt = "Nhập slug URL (không dấu, chữ thường, gạch ngang)")]
    [MaxLength(50, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả cho thẻ (không bắt buộc)")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? Description { get; set; }

    [Display(Name = "Loại thẻ")]
    public TagType? Type { get; set; }

    public List<SelectListItem>? TagTypes { get; set; }

}