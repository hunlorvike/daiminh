using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.ProductReview;

public class ProductReviewFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Sản phẩm")]
    public int? ProductId { get; set; }

    [Display(Name = "Trạng thái")]
    public ReviewStatus? Status { get; set; }

    [Display(Name = "Số sao từ")]
    [Range(1, 5, ErrorMessage = "{0} phải từ {1} đến {2}.")]
    public int? MinRating { get; set; }

    [Display(Name = "Số sao đến")]
    [Range(1, 5, ErrorMessage = "{0} phải từ {1} đến {2}.")]
    public int? MaxRating { get; set; }

    public List<SelectListItem> ProductOptions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> RatingOptions { get; set; } = new();
}
