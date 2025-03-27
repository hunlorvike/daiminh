using FluentValidation;
using web.Areas.Admin.ViewModels.FAQ;

namespace web.Areas.Admin.Validators;

public class FAQViewModelValidator : AbstractValidator<FAQViewModel>
{
    public FAQViewModelValidator()
    {
        RuleFor(x => x.Question)
            .NotEmpty().WithMessage("Vui lòng nhập câu hỏi")
            .MaximumLength(255).WithMessage("Câu hỏi không được vượt quá 255 ký tự");

        RuleFor(x => x.Answer)
            .NotEmpty().WithMessage("Vui lòng nhập câu trả lời");

        RuleFor(x => x.CategoryId)
            .NotEqual(0).WithMessage("Vui lòng chọn danh mục");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Thứ tự hiển thị phải là số không âm");
    }
}
