using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Brand;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Validators.Brand;

public class BrandViewModelValidator : AbstractValidator<BrandViewModel>
{
    private readonly ApplicationDbContext _context;

    public BrandViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        Include(new SeoPropertiesValidator<BrandViewModel>());

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên thương hiệu.")
            .MaximumLength(255).WithMessage("Tên thương hiệu không được vượt quá 255 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug.")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .Must(BeUniqueSlug).WithMessage("Slug này đã tồn tại. Vui lòng chọn slug khác.");

        RuleFor(x => x.Description);

        RuleFor(x => x.LogoUrl)
            .MaximumLength(2048).WithMessage("Đường dẫn logo không được vượt quá 2048 ký tự.");

        RuleFor(x => x.Website)
            .MaximumLength(255).WithMessage("Website không được vượt quá 255 ký tự.")
            .Matches(@"^(https?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$").WithMessage("Địa chỉ website không hợp lệ.")
            .When(x => !string.IsNullOrWhiteSpace(x.Website));
    }

    private bool BeUniqueSlug(BrandViewModel viewModel, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return true;
        }
        return !_context.Set<domain.Entities.Brand>()
                        .Any(b => b.Slug == slug.ToLowerInvariant() && b.Id != viewModel.Id);
    }
}