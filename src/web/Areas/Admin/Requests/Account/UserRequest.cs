using System.ComponentModel;
using FluentValidation;

namespace web.Areas.Admin.Requests.Account;

/// <summary>
/// Represents a request to update a user's role.
/// </summary>
public class UserRequest
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
    [DisplayName("Vai trò")] public int RoleId { get; set; }
}

/// <summary>
/// Validator for <see cref="UserRequest"/>.
/// </summary>
public class UserRequestValidator : AbstractValidator<UserRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRequestValidator"/> class.
    /// </summary>
    public UserRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Vai trò không được để trống");
    }
}