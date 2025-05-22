using domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class RoleViewModelValidator : AbstractValidator<RoleViewModel>
{
    private readonly RoleManager<Role> _roleManager;

    public RoleViewModelValidator(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(256).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .MustAsync(BeUniqueName).WithMessage("Tên Vai trò đã tồn tại. Vui lòng chọn tên khác.");
    }

    private async Task<bool> BeUniqueName(RoleViewModel viewModel, string name, ValidationContext<RoleViewModel> context, CancellationToken cancellationToken)
    {
        string normalizedName = _roleManager.NormalizeKey(name);

        var existingRole = await _roleManager.FindByNameAsync(normalizedName);

        return existingRole == null || existingRole.Id == viewModel.Id;
    }
}