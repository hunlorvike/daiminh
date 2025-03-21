using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;

namespace web.Areas.Admin.Requests.Account;

/// <summary>
/// Represents a request to update a user's role.
/// </summary>
public class AccountUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the role to assign to the user.
    /// </summary>
    /// <example>2</example>
    [Display(Name = "Vai trò", Prompt = "Lựa chọn vai trò")] public int RoleId { get; set; }
}

/// <summary>
/// Validator for <see cref="AccountUpdateRequest"/>.
/// </summary>
public class AccountUpdateRequestValidator : AbstractValidator<AccountUpdateRequest>
{
    private readonly ApplicationDbContext _context;
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountUpdateRequestValidator"/> class.
    /// </summary>
    public AccountUpdateRequestValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotNull().WithMessage("ID không được để trống")
            .MustAsync(async (id, cancellation) =>
            {
                var user = await _context.Users.FindAsync(id);
                return user != null;
            }).WithMessage("Người dùng không tồn tại hoặc đã bị xóa");

        RuleFor(x => x.RoleId)
            .NotNull().WithMessage("Vai trò không được để trống")
            .MustAsync(async (roleId, cancellation) =>
            {
                var role = await _context.Roles.FindAsync(roleId);
                return role != null;
            }).WithMessage("Vai trò không tồn tại hoặc đã bị xóa");
    }
}