using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class AttributeViewModelValidator : AbstractValidator<AttributeViewModel>
{
    private readonly ApplicationDbContext _context;

    public AttributeViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .Must(BeUniqueSlug).WithMessage("Slug này đã tồn tại, vui lòng chọn slug khác.");

        RuleFor(x => x.Description)
            .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.Description));
    }

    private bool BeUniqueSlug(AttributeViewModel viewModel, string slug)
    {
        return !_context.Set<domain.Entities.Attribute>()
                              .Any(a => a.Slug == slug && a.Id != viewModel.Id);
    }
}