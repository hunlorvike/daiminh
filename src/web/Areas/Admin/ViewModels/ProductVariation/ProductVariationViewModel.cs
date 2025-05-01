using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Admin.ViewModels.ProductVariation;

public class ProductVariationViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [HiddenInput]
    [Required(ErrorMessage = "Sản phẩm gốc không được để trống.")]
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    [Display(Name = "Giá bán", Prompt = "Nhập giá bán")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "{0} phải lớn hơn 0.")]
    public decimal Price { get; set; }

    [Display(Name = "Giá khuyến mãi", Prompt = "Nhập giá khuyến mãi (nếu có)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "{0} phải lớn hơn 0.")]
    public decimal? SalePrice { get; set; }

    [Display(Name = "Tồn kho")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int StockQuantity { get; set; }

    [Display(Name = "Ảnh biến thể", Prompt = "URL ảnh riêng của biến thể")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.ImageUrl)]
    public string? ImageUrl { get; set; }

    [Display(Name = "Là biến thể mặc định")]
    public bool IsDefault { get; set; } = false;

    [Display(Name = "Hoạt động")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Giá trị thuộc tính")]
    public List<int>? SelectedAttributeValueIds { get; set; }

    public Dictionary<int, List<SelectListItem>> AttributeValueOptionsByAttribute { get; set; } = new();
    public List<domain.Entities.Attribute>? ParentProductAttributes { get; set; }
}