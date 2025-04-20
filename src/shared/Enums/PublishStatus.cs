using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum PublishStatus
{
    [Display(Name = "Bản nháp")]
    Draft,

    [Display(Name = "Đã xuất bản")]
    Published,

    [Display(Name = "Đã lên lịch")]
    Scheduled,

    [Display(Name = "Đã lưu trữ")]
    Archived
}
