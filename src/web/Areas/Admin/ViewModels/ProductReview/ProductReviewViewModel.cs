using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.ProductReview;

public class ProductReviewViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Sản phẩm")]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    [Display(Name = "Người dùng")]
    public string? UserName { get; set; }

    [Display(Name = "Email")]
    public string? UserEmail { get; set; }

    [Display(Name = "Số sao")]
    public int Rating { get; set; }

    [Display(Name = "Nội dung")]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Trạng thái")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public ReviewStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    // Select lists for dropdowns
    public List<SelectListItem>? StatusOptions { get; set; }
}
