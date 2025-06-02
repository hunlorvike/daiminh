using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum PublishStatus
{
    [Display(Name = "Bản nháp")] Draft = 0,
    [Display(Name = "Đã xuất bản")] Published = 1,
    [Display(Name = "Đã lên lịch")] Scheduled = 2,
    [Display(Name = "Đã lưu trữ")] Archived = 3
}