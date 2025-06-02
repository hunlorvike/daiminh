using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum DisplayFrequency
{
    [Display(Name = "Một lần")] Once = 0,
    [Display(Name = "Phiên làm việc")] Session = 1,
    [Display(Name = "Hàng ngày")] Daily = 2,
    [Display(Name = "Hàng tuần")] Weekly = 3,
    [Display(Name = "Hàng tháng")] Monthly = 4,
    [Display(Name = "Luôn luôn")] Always = 5
}