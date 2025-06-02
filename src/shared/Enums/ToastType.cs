using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum ToastType
{
    [Display(Name = "Thành công")] Success = 0,
    [Display(Name = "Lỗi")] Error = 1,
    [Display(Name = "Cảnh báo")] Warning = 2,
    [Display(Name = "Thông tin")] Info = 3
}