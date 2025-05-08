using FluentValidation;
using web.Areas.Client.ViewModels.Contact;

namespace web.Areas.Client.Validators.Contact;

public class ContactViewModelValidator : AbstractValidator<ContactViewModel>
{
    public ContactViewModelValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("{PropertyName} không được để trống.")
            .MaximumLength(100)
            .WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("{PropertyName} không được để trống.")
            .EmailAddress()
            .WithMessage("Địa chỉ email không hợp lệ.");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches(@"^(?:\+84|0)(?:\d{9}|\d{10})$")
            .WithMessage("{PropertyName} không hợp lệ.");

        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("{PropertyName} không được để trống.");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("{PropertyName} không được để trống.");
    }
}