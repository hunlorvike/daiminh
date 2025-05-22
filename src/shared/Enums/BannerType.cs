using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum BannerType : ushort
{
    [Display(Name = "Đầu trang")]
    Header = 1,

    [Display(Name = "Chân trang")]
    Footer = 2,

    [Display(Name = "Thanh bên")]
    Sidebar = 3,
}