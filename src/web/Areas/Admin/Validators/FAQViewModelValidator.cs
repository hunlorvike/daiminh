using FluentValidation;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class FAQViewModelValidator : AbstractValidator<FAQViewModel>
{
    public FAQViewModelValidator()
    {
        RuleFor(x => x.Question)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Answer)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Vui lòng chọn {PropertyName}.");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải là số không âm.");
    }
}