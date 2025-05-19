using FluentValidation;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class ContactViewModelValidator : AbstractValidator<ContactViewModel>
{
    public ContactViewModelValidator()
    {
        RuleFor(x => x.AdminNotes)
            .MaximumLength(1000).WithMessage("Ghi chú nội bộ không được vượt quá 1000 ký tự.");
    }
}