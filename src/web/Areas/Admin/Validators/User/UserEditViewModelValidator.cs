using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.User;

namespace web.Areas.Admin.Validators.User;

public class UserEditViewModelValidator : AbstractValidator<UserEditViewModel>
{
    private readonly ApplicationDbContext _context;

    public UserEditViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}")
            .MaximumLength(50).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự")
            .Matches("^[a-zA-Z0-9_.-]+$").WithMessage("{PropertyName} chỉ được chứa chữ cái, số, dấu gạch dưới, dấu chấm, dấu gạch ngang")
            .Must(BeUniqueUsername).WithMessage("{PropertyName} này đã tồn tại");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự")
            .EmailAddress().WithMessage("Vui lòng nhập địa chỉ email hợp lệ")
            .Must(BeUniqueEmail).WithMessage("Địa chỉ email này đã được sử dụng");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}")
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự");
    }

    private bool BeUniqueUsername(UserEditViewModel viewModel, string username)
    {
        return ! _context.Set<domain.Entities.User>()
                               .Any(u => u.Username.ToLower() == username.ToLower() && u.Id != viewModel.Id);
    }

    private bool BeUniqueEmail(UserEditViewModel viewModel, string email)
    {
        return ! _context.Set<domain.Entities.User>()
                               .Any(u => u.Email.ToLower() == email.ToLower() && u.Id != viewModel.Id);
    }
}