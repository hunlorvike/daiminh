using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels.User;

namespace web.Areas.Admin.Validators.User;

public class UserEditViewModelValidator : AbstractValidator<UserEditViewModel>
{
    private readonly ApplicationDbContext _context;

    public UserEditViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Vui lòng nhập tên đăng nhập")
            .MaximumLength(50).WithMessage("Tên đăng nhập không được vượt quá 50 ký tự")
            .Matches("^[a-zA-Z0-9_.-]+$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số, dấu gạch dưới, dấu chấm, dấu gạch ngang")
            .Must(BeUniqueUsername).WithMessage("Tên đăng nhập này đã tồn tại");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Vui lòng nhập email")
            .MaximumLength(255).WithMessage("Email không được vượt quá 255 ký tự")
            .EmailAddress().WithMessage("Vui lòng nhập địa chỉ email hợp lệ")
            .Must(BeUniqueEmail).WithMessage("Địa chỉ email này đã được sử dụng");
    }

    private bool BeUniqueUsername(UserEditViewModel viewModel, string username)
    {
        return !_context.Set<domain.Entities.User>().Any(u => u.Username == username && u.Id != viewModel.Id);
    }

    private bool BeUniqueEmail(UserEditViewModel viewModel, string email)
    {
        return !_context.Set<domain.Entities.User>().Any(u => u.Email == email && u.Id != viewModel.Id);
    }
}