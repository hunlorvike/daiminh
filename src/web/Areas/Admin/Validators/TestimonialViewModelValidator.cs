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
    }
}