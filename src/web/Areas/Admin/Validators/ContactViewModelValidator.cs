using FluentValidation;
using web.Areas.Admin.ViewModels.Contact;

namespace web.Areas.Admin.Validators;

public class ContactViewModelValidator : AbstractValidator<ContactViewModel>
{
    public ContactViewModelValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Vui lòng nhập họ tên")
            .MaximumLength(100).WithMessage("Họ tên không được vượt quá 100 ký tự");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Vui lòng nhập email")
            .EmailAddress().WithMessage("Email không hợp lệ")
            .MaximumLength(100).WithMessage("Email không được vượt quá 100 ký tự");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Số điện thoại không được vượt quá 20 ký tự");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Vui lòng nhập tiêu đề")
            .MaximumLength(255).WithMessage("Tiêu đề không được vượt quá 255 ký tự");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Vui lòng nhập nội dung");

        RuleFor(x => x.CompanyName)
            .MaximumLength(100).WithMessage("Tên công ty không được vượt quá 100 ký tự");

        RuleFor(x => x.AdminNotes)
            .MaximumLength(1000).WithMessage("Ghi chú không được vượt quá 1000 ký tự");
    }
}
