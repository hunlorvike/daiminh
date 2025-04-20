using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum FieldType : ushort
{
    [Display(Name = "Văn bản")]
    Text,

    [Display(Name = "Đoạn văn bản")]
    TextArea,

    [Display(Name = "HTML")]
    Html,

    [Display(Name = "Hình ảnh")]
    Image,

    [Display(Name = "Số điện thoại")]
    Phone,

    [Display(Name = "Màu sắc")]
    Color,

    [Display(Name = "Email")]
    Email,

    [Display(Name = "URL")]
    Url,

    [Display(Name = "Số")]
    Number,

    [Display(Name = "Đúng/Sai")]
    Boolean,

    [Display(Name = "Ngày tháng")]
    Date,
}
