using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Models.Product;

public class ProductFieldDefinitionViewModel
{
    public int Id { get; set; }

    [Display(Name = "Loại sản phẩm")]
    public int ProductTypeId { get; set; }

    [Display(Name = "Tên trường")]
    public string? FieldName { get; set; }

    [Display(Name = "Loại trường")]
    public FieldType FieldType { get; set; }

    [Display(Name = "Bắt buộc")]
    public bool IsRequired { get; set; }

    [Display(Name = "Tùy chọn")]
    public string? FieldOptions { get; set; }

    // Helper property to get options as a list
    public List<string> Options => string.IsNullOrEmpty(FieldOptions)
        ? new List<string>()
        : FieldOptions.Split(',').Select(o => o.Trim()).ToList();
}