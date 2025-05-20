using FluentValidation;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class RoleViewModelValidator : AbstractValidator<RoleViewModel>
{
    private readonly IRoleService _roleService;

    public RoleViewModelValidator(IRoleService roleService)
    {
        _roleService = roleService;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên vai trò không được để trống.")
            .MaximumLength(100).WithMessage("Tên vai trò không được vượt quá 100 ký tự.");

        RuleFor(x => x)
            .MustAsync(async (role, cancellationToken) =>
                !await _roleService.RoleNameExistsAsync(role.Name, role.Id))
            .WithMessage("Tên vai trò đã tồn tại.")
            .WithName(nameof(RoleViewModel.Name));
    }
}