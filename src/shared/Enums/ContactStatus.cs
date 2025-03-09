namespace shared.Enums;

using System.ComponentModel.DataAnnotations;

public enum ContactStatus
{
    [Display(Name = "Đang chờ xử lý")] Pending = 0,

    [Display(Name = "Đang xử lý")] InProgress = 1,

    [Display(Name = "Hoàn thành")] Completed = 2,

    [Display(Name = "Spam")] Spam = 3
}