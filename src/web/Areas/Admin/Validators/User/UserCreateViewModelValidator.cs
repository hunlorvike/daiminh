using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels.User;

namespace web.Areas.Admin.Validators.User;

public class UserCreateViewModelValidator : AbstractValidator<UserCreateViewModel>
{
    private readonly ApplicationDbContext _context;

    public UserCreateViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(50).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches("^[a-zA-Z0-9_.-]+$").WithMessage("{PropertyName} chỉ chấp nhận chữ cái, số, dấu gạch dưới, dấu chấm, dấu gạch ngang.")
            .Must(BeUniqueUsername).WithMessage("{PropertyName} này đã được sử dụng.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .EmailAddress().WithMessage("Vui lòng nhập địa chỉ email hợp lệ.")
            .Must(BeUniqueEmail).WithMessage("{PropertyName} này đã được sử dụng.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MinimumLength(8).WithMessage("{PropertyName} phải có ít nhất {MinLength} ký tự.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .Equal(x => x.Password).WithMessage("Mật khẩu xác nhận không khớp.");

        RuleFor(x => x.FullName)
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");
    }

    private bool BeUniqueUsername(string username)
    {
        return !_context.Set<domain.Entities.User>()
                              .Any(u => u.Username == username);
    }

    private bool BeUniqueEmail(string email)
    {
        return !_context.Set<domain.Entities.User>()
                              .Any(u => u.Email == email);
    }
}