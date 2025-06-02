using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum TagType
{
    [Display(Name = "Sản phẩm")] Product = 0,
    [Display(Name = "Bài viết")] Article = 1,
    [Display(Name = "Tổng quát")] General = 2
}