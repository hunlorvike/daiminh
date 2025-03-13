using FluentValidation;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Slider;

public class SliderCreateRequest
{
    [Display(Name = "Tiêu đề", Prompt = "Nhập tên tiêu đề")]
    public string Title { get; set; }

    [Display(Name = "Hình ảnh", Prompt = "Nhập đường dẫn hình ảnh")]
    public string ImageUrl { get; set; }

    [Display(Name = "Đường dẫn liên kết", Prompt = "Nhập URL liên kết")]
    public string? LinkUrl { get; set; }

    [Display(Name = "Thứ tự hiển thị", Prompt = "Nhập thứ tự hiển thị")]
    public int Order { get; set; }

    [Display(Name = "Nội dung HTML overlay", Prompt = "Nhập nội dung HTML overlay")]
    public string? OverlayHtml { get; set; }

    [Display(Name = "Vị trí overlay", Prompt = "Chọn vị trí overlay")]
    public OverlayPosition? OverlayPosition { get; set; }
}

public class SliderCreateRequestValidator : AbstractValidator<SliderCreateRequest>
{
    public SliderCreateRequestValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithMessage("Tiêu đề không được để trống.")
            .MaximumLength(255).WithMessage("Tiêu đề không được quá 255 ký tự.");

        RuleFor(request => request.ImageUrl)
            .NotEmpty().WithMessage("Hình ảnh không được để trống.")
            .MaximumLength(500).WithMessage("Đường dẫn hình ảnh không được quá 500 ký tự.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("Hình ảnh phải là một URL hợp lệ.");

        RuleFor(request => request.LinkUrl)
            .MaximumLength(500).WithMessage("Đường dẫn liên kết không được quá 500 ký tự.")
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Đường dẫn liên kết phải là một URL hợp lệ.");

        RuleFor(request => request.Order)
            .GreaterThan(0).WithMessage("Thứ tự hiển thị phải lớn hơn 0.");

        RuleFor(request => request.OverlayHtml)
            .MaximumLength(2000).WithMessage("Nội dung HTML overlay không được quá 2000 ký tự.");

        RuleFor(request => request.OverlayPosition)
            .IsInEnum().WithMessage("Vị trí overlay không hợp lệ.");
    }
}