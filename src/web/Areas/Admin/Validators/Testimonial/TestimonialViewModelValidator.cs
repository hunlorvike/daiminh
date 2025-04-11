using FluentValidation;
using web.Areas.Admin.ViewModels.Testimonial;

namespace web.Areas.Admin.Validators.Testimonial;

public class TestimonialViewModelValidator : AbstractValidator<TestimonialViewModel>
{
    public TestimonialViewModelValidator()
    {
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}")
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự");

        RuleFor(x => x.ClientTitle)
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự");

        RuleFor(x => x.ClientCompany)
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("{PropertyName} phải từ {From} đến {To} sao");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải là số không âm");

        RuleFor(x => x.ProjectReference)
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự");
    }
}
