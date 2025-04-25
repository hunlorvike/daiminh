using System.ComponentModel.DataAnnotations;

namespace shared.Enums;
public enum ReviewStatus
{
    [Display(Name = "Chờ duyệt")]
    Pending,
    [Display(Name = "Đã duyệt")]
    Approved,
    [Display(Name = "Bị từ chối")]
    Rejected
}
