using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.ViewModels;

public class ProductViewModel : SeoViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tên sản phẩm", Prompt = "Nhập tên sản phẩm")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug (URL)", Prompt = "phan-url-than-thien-san-pham")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả chi tiết", Prompt = "Nhập mô tả chi tiết sản phẩm")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [DataType(DataType.Html)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Mô tả ngắn", Prompt = "Nhập mô tả ngắn gọn")]
    [MaxLength(500, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.MultilineText)]
    public string? ShortDescription { get; set; }

    [Display(Name = "Nhà sản xuất", Prompt = "Tên nhà sản xuất")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Manufacturer { get; set; }

    [Display(Name = "Xuất xứ", Prompt = "Quốc gia sản xuất")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Origin { get; set; }

    [Display(Name = "Thông số kỹ thuật", Prompt = "Nhập thông số kỹ thuật chi tiết")]
    [DataType(DataType.Html)]
    public string? Specifications { get; set; }

    [Display(Name = "Hướng dẫn sử dụng", Prompt = "Nhập cách sử dụng, thi công")]
    [DataType(DataType.Html)]
    public string? Usage { get; set; }

    [Display(Name = "Nổi bật")]
    public bool IsFeatured { get; set; } = false;

    [Display(Name = "Hoạt động")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Trạng thái")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    [Display(Name = "Thương hiệu")]
    public int? BrandId { get; set; }

    [Display(Name = "Danh mục")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public int? CategoryId { get; set; }

    [Display(Name = "Thẻ (Tags)")]
    public List<int>? SelectedTagIds { get; set; }

    [Display(Name = "Bài viết liên quan")]
    public List<int>? SelectedArticleIds { get; set; }

    [Display(Name = "Hình ảnh sản phẩm")]
    public List<ProductImageViewModel> Images { get; set; } = new();

    // Select lists for dropdowns
    public List<SelectListItem>? CategoryOptions { get; set; }
    public List<SelectListItem>? BrandOptions { get; set; }
    public List<SelectListItem>? StatusOptions { get; set; }
    public List<SelectListItem>? TagOptions { get; set; }
}