using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum BannerType : ushort
{
    [Display(Name = "Đầu trang")]
    Header = 1,

    [Display(Name = "Trang chủ")]
    Homepage = 2,

    [Display(Name = "Danh mục")]
    Category = 3,

    [Display(Name = "Bài viết")]
    Article = 4,

    [Display(Name = "Chân trang")]
    Footer = 5
}