using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Product;

public class ProductFieldValueCreateRequest
{
    [Required]
    [Display(Name = "ID trường")]
    public int FieldId { get; set; }

    [Display(Name = "Tên trường")]
    public string? FieldName { get; set; }

    [Display(Name = "Loại trường")]
    public string? FieldType { get; set; }

    [Display(Name = "Giá trị")]
    public string? Value { get; set; }
}