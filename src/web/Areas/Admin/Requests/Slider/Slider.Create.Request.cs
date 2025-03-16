using System.ComponentModel.DataAnnotations;
using FluentValidation;
using shared.Enums;

namespace web.Areas.Admin.Requests.Slider;

/// <summary>
/// Represents a request to create a new slider item.
/// </summary>
public class SliderCreateRequest
{
    /// <summary>
    /// Gets or sets the title of the slider item.
    /// </summary>
    /// <example>Welcome to Our Site!</example>
    [Display(Name = "Tiêu đề", Prompt = "Nhập tiêu đề")]
    [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL of the image for the slider item.
    /// </summary>
    /// <example>https://example.com/images/slider1.jpg</example>
    [Display(Name = "Hình ảnh", Prompt = "Nhập URL hình ảnh")]
    [Required(ErrorMessage = "Hình ảnh là bắt buộc.")]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional URL to link to when the slider item is clicked.
    /// </summary>
    /// <example>https://example.com/products</example>
    [Display(Name = "Liên kết", Prompt = "Nhập URL liên kết")]
    public string? LinkUrl { get; set; }

    /// <summary>
    /// Gets or sets the display order of the slider item.
    /// </summary>
    /// <example>1</example>
    [Display(Name = "Thứ tự", Prompt = "Nhập thứ tự")]
    [Required(ErrorMessage = "Thứ tự là bắt buộc.")]
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets optional HTML content to overlay on the slider item.
    /// </summary>
    /// <example><h2>Special Offer!</h2></example>
    [Display(Name = "HTML Overlay", Prompt = "Nhập HTML (tùy chọn)")]
    public string? OverlayHtml { get; set; }

    /// <summary>
    /// Gets or sets the position of the overlay content.
    /// </summary>
    /// <example>TopLeft</example>
    [Display(Name = "Vị trí", Prompt = "Chọn vị trí")]
    public OverlayPosition? OverlayPosition { get; set; }
}

/// <summary>
/// Validator for <see cref="SliderCreateRequest"/>.
/// </summary>
public class SliderCreateRequestValidator : AbstractValidator<SliderCreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SliderCreateRequestValidator"/> class.
    /// </summary>
    public SliderCreateRequestValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithMessage("Tiêu đề slider không được bỏ trống.") // Improved message
            .MaximumLength(255).WithMessage("Tiêu đề slider không được vượt quá 255 ký tự.");

        RuleFor(request => request.ImageUrl)
            .NotEmpty().WithMessage("Đường dẫn hình ảnh không được bỏ trống.") // Improved message
            .MaximumLength(500).WithMessage("Đường dẫn hình ảnh không được vượt quá 500 ký tự.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("Đường dẫn hình ảnh phải là một URL tuyệt đối và hợp lệ."); // More specific

        RuleFor(request => request.LinkUrl)
            .MaximumLength(500).WithMessage("Đường dẫn liên kết không được vượt quá 500 ký tự.")
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Đường dẫn liên kết (nếu có) phải là một URL tuyệt đối và hợp lệ."); // More specific

        RuleFor(request => request.Order)
            .GreaterThanOrEqualTo(0).WithMessage("Thứ tự hiển thị phải là một số nguyên không âm."); // Allow 0

        RuleFor(request => request.OverlayHtml)
            .MaximumLength(2000).WithMessage("Nội dung HTML overlay không được vượt quá 2000 ký tự.");

        RuleFor(request => request.OverlayPosition)
            .IsInEnum().WithMessage("Vị trí overlay không hợp lệ. Vui lòng chọn một vị trí từ danh sách."); // More specific
    }
}