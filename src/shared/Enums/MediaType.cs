using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum MediaType : ushort
{
    [Display(Name = "Hình ảnh")]
    Image,

    [Display(Name = "Video")]
    Video,

    [Display(Name = "Tài liệu")]
    Document,

    [Display(Name = "Khác")]
    Other
}
