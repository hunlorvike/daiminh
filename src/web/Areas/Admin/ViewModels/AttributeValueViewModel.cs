using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Admin.ViewModels;

public class AttributeValueViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Thuộc tính cha")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public int AttributeId { get; set; }

    [Display(Name = "Giá trị", Prompt = "Nhập giá trị (vd: Đỏ, 5 Lít)")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Value { get; set; } = string.Empty;

    [Display(Name = "Slug (URL)", Prompt = "slug-gia-tri")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")]
    public string? Slug { get; set; }

    [Display(Name = "Thứ tự")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int OrderIndex { get; set; } = 0;
    public List<SelectListItem>? AttributeOptions { get; set; }
}
