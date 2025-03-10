using shared.Enums;
using System.ComponentModel;

namespace web.Areas.Admin.Models.ProductFieldDefinition;

public class ProductFieldDefinitionViewModel
{
    public int Id { get; set; }
    [DisplayName("Id nội dung")] public int ProductTypeId { get; set; }
    [DisplayName("Tên loại nội dung")] public string? ProductTypeName { get; set; }
    [DisplayName("Tên trường")] public string? FieldName { get; set; }
    [DisplayName("Kiểu trường")] public FieldType FieldType { get; set; }
    [DisplayName("Bắt buộc")] public bool IsRequired { get; set; }
    [DisplayName("Tùy chọn trường")] public string? FieldOptions { get; set; }
    [DisplayName("Ngày tạo")] public DateTime? CreatedAt { get; set; }
}