using FluentValidation;
using web.Areas.Admin.ViewModels.Contact;

namespace web.Areas.Admin.Validators.Contact;

public class ContactDetailViewModelValidator : AbstractValidator<ContactDetailViewModel>
{
    public ContactDetailViewModelValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("{PropertyName} không hợp lệ.");

        RuleFor(x => x.AdminNotes).MaximumLength(2000).WithMessage("{PropertyName} quá dài.");
    }
}
