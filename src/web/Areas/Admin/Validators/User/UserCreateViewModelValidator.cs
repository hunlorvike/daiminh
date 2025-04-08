using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.User;

namespace web.Areas.Admin.Validators.User;

public class UserCreateViewModelValidator : AbstractValidator<UserCreateViewModel>
{
    private readonly ApplicationDbContext _context;

    public UserCreateViewModelValidator(ApplicationDbContext context)
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

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Vui lòng nhập mật khẩu")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Vui lòng xác nhận mật khẩu")
            .Equal(x => x.Password).WithMessage("Mật khẩu xác nhận không khớp");
    }

    private bool BeUniqueUsername(string username)
    {
        return ! _context.Set<domain.Entities.User>().Any(u => u.Username == username);
    }

    private bool BeUniqueEmail(string email)
    {
        return ! _context.Set<domain.Entities.User>().Any(u => u.Email == email);
    }
}