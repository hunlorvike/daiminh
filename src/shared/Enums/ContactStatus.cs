using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum ContactStatus
{
    [Display(Name = "Mới")] New = 0,
    [Display(Name = "Đã đọc")] Read = 1,
    [Display(Name = "Đã trả lời")] Replied = 2,
    [Display(Name = "Đang xử lý")] InProgress = 3,
    [Display(Name = "Đã giải quyết")] Resolved = 4,
    [Display(Name = "Spam/Rác")] Spam = 5,
    [Display(Name = "Lưu trữ")] Archived = 6
}