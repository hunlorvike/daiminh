using FluentValidation;
using web.Areas.Admin.ViewModels.Newsletter;

namespace web.Areas.Admin.Validators.Newsletter;
public class NewsletterViewModelValidator : AbstractValidator<NewsletterViewModel>
{
    public NewsletterViewModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Vui lòng nhập email")
            .EmailAddress().WithMessage("Email không hợp lệ")
            .MaximumLength(100).WithMessage("Email không được vượt quá 100 ký tự");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Tên không được vượt quá 100 ký tự");
    }
}