using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels.Newsletter;

namespace web.Areas.Admin.Validators.Newsletter;

public class NewsletterViewModelValidator : AbstractValidator<NewsletterViewModel>
{
    private readonly ApplicationDbContext _context;

    public NewsletterViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}")
            .EmailAddress().WithMessage("{PropertyName} không hợp lệ")
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự")
            .Must(BeUniqueEmail).WithMessage("{PropertyName} này đã được đăng ký.");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự");
    }

    private bool BeUniqueEmail(NewsletterViewModel viewModel, string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return true;

        return !_context.Set<domain.Entities.Newsletter>()
                               .Any(n => n.Email.ToLower() == email.ToLower() && n.Id != viewModel.Id);
    }
}