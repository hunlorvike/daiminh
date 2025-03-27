using FluentValidation;
using web.Areas.Admin.ViewModels.Testimonial;

namespace web.Areas.Admin.Validators;

public class TestimonialViewModelValidator : AbstractValidator<TestimonialViewModel>
{
    public TestimonialViewModelValidator()
    {
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("Vui lòng nhập tên khách hàng")
            .MaximumLength(100).WithMessage("Tên khách hàng không được vượt quá 100 ký tự");

        RuleFor(x => x.ClientTitle)
            .MaximumLength(100).WithMessage("Chức danh không được vượt quá 100 ký tự");

        RuleFor(x => x.ClientCompany)
            .MaximumLength(100).WithMessage("Tên công ty không được vượt quá 100 ký tự");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Vui lòng nhập nội dung đánh giá");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Đánh giá phải từ 1 đến 5 sao");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Thứ tự hiển thị phải là số không âm");

        RuleFor(x => x.ProjectReference)
            .MaximumLength(255).WithMessage("Tham chiếu dự án không được vượt quá 255 ký tự");

        RuleFor(x => x.AvatarFile)
            .Must(x => x == null || IsValidImage(x))
            .WithMessage("Chỉ chấp nhận file ảnh (jpg, jpeg, png, gif) và kích thước không quá 2MB");
    }

    private bool IsValidImage(Microsoft.AspNetCore.Http.IFormFile file)
    {
        if (file == null)
            return true;

        // Check file size (max 2MB)
        if (file.Length > 2 * 1024 * 1024)
            return false;

        // Check file extension
        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
        return validExtensions.Contains(fileExtension);
    }
}