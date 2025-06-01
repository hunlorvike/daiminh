using System.ComponentModel.DataAnnotations;

namespace shared.Enums;
public enum DisplayFrequency : ushort
{
    [Display(Name = "Một lần")]
    Once,
    [Display(Name = "Phiên làm việc")]
    Session,
    [Display(Name = "Hàng ngày")]
    Daily,
    [Display(Name = "Hàng tuần")]
    Weekly,
    [Display(Name = "Hàng tháng")]
    Monthly,
    [Display(Name = "Luôn luôn")]
    Always,
}
