using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.ProductType;

public class ProductTypeViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Loại sản phẩm không được để trống")]
    [Display(Name = "Loại sản phẩm", Prompt = "Nhập tên loại sản phẩm")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Slug không được để trống")]
    [Display(Name = "Slug", Prompt = "Nhập slug URL (không dấu, cách nhau bằng dấu gạch ngang)")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả ngắn cho loại sản phẩm")]
    public string? Description { get; set; }

    [Display(Name = "Biểu tượng", Prompt = "Nhập tên biểu tượng hoặc mã Tabler icon")]
    public string? Icon { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;
}
