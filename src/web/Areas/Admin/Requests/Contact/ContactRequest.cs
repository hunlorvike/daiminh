using FluentValidation;
using shared.Enums;
using System.ComponentModel;

namespace web.Areas.Admin.Requests.Contact;

public class ContactUpdateRequest
{
    public int Id { get; set; }
    [DisplayName("Trạng thái")] public ContactStatus ContactStatus { get; set; }
}

public class ContactEditRequestValidator : AbstractValidator<ContactUpdateRequest>
{
    public ContactEditRequestValidator()
    {
        RuleFor(x => x.ContactStatus).IsInEnum().WithMessage("Trạng thái không hợp lệ");
    }
}