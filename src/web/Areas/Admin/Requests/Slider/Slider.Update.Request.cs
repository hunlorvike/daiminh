using System.ComponentModel.DataAnnotations;
using FluentValidation;
using shared.Enums;

namespace web.Areas.Admin.Requests.Slider;

/// <summary>
/// Represents a request to update an existing slider item.
/// </summary>
public class SliderUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the slider item to update.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the updated title of the slider item.
    /// </summary>
    /// <example>New Slider Title</example>
    [Display(Name = "Tiêu đề", Prompt = "Nhập tên tiêu đề")]
    [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated URL of the image for the slider item.
    /// </summary>
    /// <example>https://example.com/images/new_slider.jpg</example>
    [Display(Name = "Hình ảnh", Prompt = "Nhập đường dẫn hình ảnh")]
    [Required(ErrorMessage = "Hình ảnh là bắt buộc.")]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated optional URL to link to when the slider item is clicked.
    /// </summary>
    /// <example>https://example.com/new-products</example>
    [Display(Name = "Đường dẫn liên kết", Prompt = "Nhập URL liên kết")]
    public string? LinkUrl { get; set; }

    /// <summary>
    /// Gets or sets the updated display order of the slider item.
    /// </summary>
    /// <example>2</example>
    [Display(Name = "Thứ tự hiển thị", Prompt = "Nhập thứ tự hiển thị")]
    [Required(ErrorMessage = "Thứ tự là bắt buộc.")]
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the updated optional HTML content to overlay on the slider item.
    /// </summary>
    /// <example><p>Updated content!</p></example>
    [Display(Name = "Nội dung HTML overlay", Prompt = "Nhập nội dung HTML overlay")]
    public string? OverlayHtml { get; set; }

    /// <summary>
    /// Gets or sets the updated position of the overlay content.
    /// </summary>
    /// <example>BottomRight</example>
    [Display(Name = "Vị trí overlay", Prompt = "Chọn vị trí overlay")]
    public OverlayPosition? OverlayPosition { get; set; }
}

/// <summary>
/// Validator for <see cref="SliderUpdateRequest"/>.
/// </summary>
public class SliderUpdateRequestValidator : AbstractValidator<SliderUpdateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SliderUpdateRequestValidator"/> class.
    /// </summary>
    public SliderUpdateRequestValidator()
    {
        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID slider phải là một số nguyên dương.");

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