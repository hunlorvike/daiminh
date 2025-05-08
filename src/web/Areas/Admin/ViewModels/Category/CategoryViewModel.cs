using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Category;

public class CategoryViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tên danh mục", Prompt = "Nhập tên danh mục")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug (URL)", Prompt = "phan-url-than-thien")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả ngắn (không bắt buộc)")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Display(Name = "Icon Class (Tabler)", Prompt = "Ví dụ: ti ti-home")]
    [MaxLength(50, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Icon { get; set; }

    [Display(Name = "Danh mục cha")]
    public int? ParentId { get; set; }

    [Display(Name = "Thứ tự hiển thị", Prompt = "Nhập số thứ tự (số nhỏ hiển thị trước)")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Loại danh mục")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public CategoryType Type { get; set; } = CategoryType.Product;

    [Display(Name = "Số mục")]
    public int ItemCount { get; set; }

    [Display(Name = "Có danh mục con")]
    public bool HasChildren { get; set; }

    // SelectLists for dropdowns - populated in Controller
    public List<SelectListItem>? ParentCategories { get; set; }
    public List<SelectListItem>? CategoryTypes { get; set; }
}