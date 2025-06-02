using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum MediaType
{
    [Display(Name = "Hình ảnh")] Image = 0,
    [Display(Name = "Video")] Video = 1,
    [Display(Name = "Tài liệu")] Document = 2,
    [Display(Name = "Khác")] Other = 3
}