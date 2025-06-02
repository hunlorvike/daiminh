using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum FieldType
{
    [Display(Name = "Văn bản")] Text = 0,
    [Display(Name = "Đoạn văn bản")] TextArea = 1,
    [Display(Name = "HTML")] Html = 2,
    [Display(Name = "Hình ảnh")] Image = 3,
    [Display(Name = "Số điện thoại")] Phone = 4,
    [Display(Name = "Màu sắc")] Color = 5,
    [Display(Name = "Email")] Email = 6,
    [Display(Name = "URL")] Url = 7,
    [Display(Name = "Số")] Number = 8,
    [Display(Name = "Đúng/Sai")] Boolean = 9,
    [Display(Name = "Ngày tháng")] Date = 10
}