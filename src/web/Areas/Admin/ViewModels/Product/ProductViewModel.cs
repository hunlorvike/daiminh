using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using shared.Enums;
using System.ComponentModel.DataAnnotations;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.ViewModels.Product;

public class ProductViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tên sản phẩm", Prompt = "Nhập tên sản phẩm")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug (URL)", Prompt = "ten-san-pham-khong-dau")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Mô tả đầy đủ")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [DataType(DataType.Html)] // Hint for UI editors
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Mô tả ngắn", Prompt = "Nhập mô tả ngắn gọn")]
    [MaxLength(500, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.MultilineText)]
    public string? ShortDescription { get; set; }

    [Display(Name = "Nhà sản xuất", Prompt = "Nhập tên nhà sản xuất")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Manufacturer { get; set; }

    [Display(Name = "Xuất xứ", Prompt = "Ví dụ: Việt Nam, Trung Quốc")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Origin { get; set; }

    [Display(Name = "Thông số kỹ thuật")]
    [DataType(DataType.Html)]
    public string? Specifications { get; set; }

    [Display(Name = "Hướng dẫn sử dụng")]
    [DataType(DataType.Html)]
    public string? Usage { get; set; }

    [Display(Name = "Tính năng nổi bật")]
    [DataType(DataType.Html)]
    public string? Features { get; set; }

    [Display(Name = "Thông tin đóng gói")]
    [DataType(DataType.Html)]
    public string? PackagingInfo { get; set; }

    [Display(Name = "Hướng dẫn bảo quản")]
    [DataType(DataType.Html)]
    public string? StorageInstructions { get; set; }

    [Display(Name = "Thông tin an toàn")]
    [DataType(DataType.Html)]
    public string? SafetyInfo { get; set; }

    [Display(Name = "Lĩnh vực ứng dụng")]
    [DataType(DataType.Html)]
    public string? ApplicationAreas { get; set; }

    [Display(Name = "Tài liệu kỹ thuật (liên kết file)")]
    [DataType(DataType.Html)]
    public string? TechnicalDocuments { get; set; }

    [Display(Name = "Đánh dấu nổi bật")]
    public bool IsFeatured { get; set; } = false;

    [Display(Name = "Kích hoạt sản phẩm")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Trạng thái xuất bản")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    [Display(Name = "Thương hiệu")]
    public int? BrandId { get; set; }

    [Display(Name = "Danh mục")]
    public int? CategoryId { get; set; }

    [Display(Name = "Ảnh sản phẩm")]
    public List<ProductImageViewModel> Images { get; set; } = new();

    [Display(Name = "Thẻ (Tags)")]
    public List<int>? SelectedTagIds { get; set; } 

    [Display(Name = "Bài viết liên quan")]
    public List<int>? SelectedArticleIds { get; set; }

    public List<SelectListItem>? CategoryOptions { get; set; }
    public List<SelectListItem>? BrandOptions { get; set; }
    public List<SelectListItem>? StatusOptions { get; set; }
    public List<SelectListItem>? AvailableTags { get; set; }
    public List<SelectListItem>? AvailableArticles { get; set; } 


    // Embed SEO Fields - Assuming SeoViewModel exists
    public SeoViewModel Seo { get; set; } = new();
}
