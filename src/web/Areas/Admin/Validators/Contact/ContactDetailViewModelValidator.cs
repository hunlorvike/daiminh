using FluentValidation;
using web.Areas.Admin.ViewModels.Contact;

namespace web.Areas.Admin.Validators.Contact;

public class ContactDetailViewModelValidator : AbstractValidator<ContactDetailViewModel>
{
    public ContactDetailViewModelValidator()
    {
        // Only validate fields that admin can edit: Status and AdminNotes
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Trạng thái không hợp lệ.");

        // AdminNotes is text, no strict length needed unless DB has one
    }
}