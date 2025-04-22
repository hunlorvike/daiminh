using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.ViewModels.Article;

public class ArticleViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tiêu đề bài viết", Prompt = "Nhập tiêu đề bài viết")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Slug (URL)", Prompt = "phan-url-than-thien-bai-viet")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Nội dung", Prompt = "Nhập nội dung chi tiết bài viết")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    // MaxLength for Content is usually not needed at ViewModel level if DB column is text/ntext/nvarchar(max)
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Tóm tắt", Prompt = "Nhập tóm tắt ngắn gọn")]
    [MaxLength(500, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.MultilineText)]
    public string? Summary { get; set; }

    [Display(Name = "Ảnh đại diện (Banner)", Prompt = "URL ảnh đại diện")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.ImageUrl)]
    public string? FeaturedImage { get; set; }

    [Display(Name = "Ảnh Thumbnail", Prompt = "URL ảnh thumbnail")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.ImageUrl)]
    public string? ThumbnailImage { get; set; }

    [Display(Name = "Nổi bật")]
    public bool IsFeatured { get; set; } = false;

    [Display(Name = "Ngày xuất bản")]
    [DataType(DataType.DateTime)]
    public DateTime? PublishedAt { get; set; }

    [Display(Name = "Tên tác giả", Prompt = "Tên hiển thị của tác giả")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? AuthorName { get; set; }

    [Display(Name = "Avatar tác giả", Prompt = "URL avatar tác giả")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [DataType(DataType.ImageUrl)]
    public string? AuthorAvatar { get; set; }

    [Display(Name = "Thời gian đọc (phút)", Prompt = "Ước tính thời gian đọc")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int EstimatedReadingMinutes { get; set; } = 0;

    [Display(Name = "Trạng thái")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    [Display(Name = "Danh mục")]
    public int? CategoryId { get; set; }

    [Display(Name = "Thẻ (Tags)")]
    public List<int>? SelectedTagIds { get; set; }

    [Display(Name = "Sản phẩm liên quan")]
    public List<int>? SelectedProductIds { get; set; }

    // SelectLists for dropdowns - populated in Controller
    public List<SelectListItem>? CategoryOptions { get; set; }
    public List<SelectListItem>? StatusOptions { get; set; }
    public List<SelectListItem>? TagOptions { get; set; }
    public List<SelectListItem>? ProductOptions { get; set; }

    // Embed SEO Fields
    public SeoViewModel Seo { get; set; } = new();
}