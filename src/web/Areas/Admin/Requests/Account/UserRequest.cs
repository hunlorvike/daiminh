using System.ComponentModel;
using FluentValidation;

namespace web.Areas.Admin.Requests.Account;

public class UserRequest
{
    public int Id { get; set; }
    [DisplayName("Vai trò")] public int RoleId { get; set; }
}

public class UserRequestValidator : AbstractValidator<UserRequest>
{
    public UserRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Vai trò không được để trống");
    }
}