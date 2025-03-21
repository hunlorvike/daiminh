using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;

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
    [Required(ErrorMessage = "ID không được bỏ trống")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the new status of the contact.
    /// </summary>
    /// <example>Responded</example>
    [DisplayName("Trạng thái")]
    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public ContactStatus ContactStatus { get; set; }
}

/// <summary>
/// Validator for <see cref="ContactUpdateRequest"/>.
/// </summary>
public class ContactUpdateRequestValidator : AbstractValidator<ContactUpdateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactUpdateRequestValidator"/> class.
    /// </summary>
    public ContactUpdateRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID liên hệ phải là một số nguyên dương.")
            .MustAsync(BeExistingContact).WithMessage("Thông tin liên hệ không tồn tại hoặc đã bị xóa.");

        RuleFor(x => x.ContactStatus)
            .IsInEnum().WithMessage("Trạng thái liên hệ không hợp lệ. Vui lòng chọn một trạng thái hợp lệ từ danh sách.");
    }

    /// <summary>
    /// Checks if the Contact exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingContact(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Contacts
            .AnyAsync(c => c.Id == id && c.DeletedAt == null, cancellationToken);
    }
}