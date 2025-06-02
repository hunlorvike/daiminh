using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum ReviewStatus
{
    [Display(Name = "Chờ duyệt")] Pending = 0,
    [Display(Name = "Đã duyệt")] Approved = 1,
    [Display(Name = "Bị từ chối")] Rejected = 2
}