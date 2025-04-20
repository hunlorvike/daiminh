using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum ContactStatus : ushort
{
    [Display(Name = "Mới")]
    New,

    [Display(Name = "Đã đọc")]
    Read,

    [Display(Name = "Đã trả lời")]
    Replied,

    [Display(Name = "Đang xử lý")]
    InProgress,

    [Display(Name = "Đã giải quyết")]
    Resolved,

    [Display(Name = "Spam/Rác")]
    Spam,

    [Display(Name = "Lưu trữ")]
    Archived
}
