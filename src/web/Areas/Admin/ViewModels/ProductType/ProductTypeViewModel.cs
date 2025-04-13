using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.ViewModels.ProductType;

public class ProductTypeViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tên loại sản phẩm", Prompt = "Nhập tên loại sản phẩm")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug", Prompt = "Ví dụ: loai-san-pham")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả ngắn (không bắt buộc)")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? Description { get; set; }

    [Display(Name = "Icon (Tabler)", Prompt = "Ví dụ: ti ti-package")]
    [MaxLength(50, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    [RegularExpression(@"^ti ti-[a-z0-9\-]+$", ErrorMessage = "Định dạng icon không hợp lệ (ví dụ: ti ti-device-laptop)")]
    public string? Icon { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;
}