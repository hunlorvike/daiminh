using domain.Entities;
using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels.ProductType;

namespace web.Areas.Admin.Validators.Product;

public class ProductTypeViewModelValidator : AbstractValidator<ProductTypeViewModel>
{
    private readonly ApplicationDbContext _context;

    public ProductTypeViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .Must(BeUniqueSlug).WithMessage("{PropertyName} này đã tồn tại. Vui lòng chọn slug khác.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Icon)
            .MaximumLength(50).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches(@"^ti ti-[a-z0-9\-]+$").WithMessage("Định dạng {PropertyName} không hợp lệ (ví dụ: ti ti-package)")
            .When(x => !string.IsNullOrWhiteSpace(x.Icon));
    }

    private bool BeUniqueSlug(ProductTypeViewModel viewModel, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return true;

        return !_context.Set<ProductType>()
                               .Any(pt => pt.Slug.ToLower() == slug.ToLower() && pt.Id != viewModel.Id);
    }
}