using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels.Brand;

namespace web.Areas.Admin.Validators.Brand;

public class BrandViewModelValidator : AbstractValidator<BrandViewModel>
{
    private readonly ApplicationDbContext _context;

    public BrandViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .Must(BeUniqueSlug).WithMessage("{PropertyName} này đã tồn tại, vui lòng chọn slug khác.");

        RuleFor(x => x.Description);

        RuleFor(x => x.LogoUrl)
             .MaximumLength(2048).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
             .Matches(@"^(https?://|/).*$").When(x => !string.IsNullOrWhiteSpace(x.LogoUrl)).WithMessage("{PropertyName} phải là một URL hợp lệ (http, https hoặc tương đối /).");


        RuleFor(x => x.Website)
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches(@"^https?://.*$").When(x => !string.IsNullOrWhiteSpace(x.Website)).WithMessage("{PropertyName} phải là một URL hợp lệ bắt đầu bằng http hoặc https.");
    }

    private bool BeUniqueSlug(BrandViewModel viewModel, string slug)
    {
        return !_context.Set<domain.Entities.Brand>()
                              .Any(b => b.Slug == slug && b.Id != viewModel.Id);
    }
}