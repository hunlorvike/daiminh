using FluentValidation;
using shared.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Contact;

/// <summary>
/// Represents a request to update the status of a contact.
/// </summary>
public class ContactUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the contact to update.
    /// </summary>
    /// <example>1</example>
    [Required] // Add DataAnnotations Required attribute
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the new status of the contact.
    /// </summary>
    /// <example>Responded</example>
    [DisplayName("Trạng thái")]
    [Required] // Add DataAnnotations Required attribute
    public ContactStatus ContactStatus { get; set; }
}

/// <summary>
/// Validator for <see cref="ContactUpdateRequest"/>.
/// </summary>
public class ContactEditRequestValidator : AbstractValidator<ContactUpdateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContactEditRequestValidator"/> class.
    /// </summary>
    public ContactEditRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID liên hệ phải là một số nguyên dương.");

        RuleFor(x => x.ContactStatus)
            .IsInEnum().WithMessage("Trạng thái liên hệ không hợp lệ. Vui lòng chọn một trạng thái hợp lệ từ danh sách."); // Improved message
    }
}