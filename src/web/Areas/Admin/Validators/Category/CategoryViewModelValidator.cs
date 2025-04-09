using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Category;
using web.Areas.Admin.ViewModels.Shared;

public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
{
    private readonly ApplicationDbContext _context;

    public CategoryViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        Include(new SeoPropertiesValidator<CategoryViewModel>());

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên danh mục.")
            .MaximumLength(100).WithMessage("Tên danh mục không được vượt quá 100 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug.")
            .MaximumLength(100).WithMessage("Slug không được vượt quá 100 ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .Must(BeUniqueSlug).WithMessage("Slug này đã tồn tại cho loại danh mục này. Vui lòng chọn slug khác.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự.");

        RuleFor(x => x.Icon)
            .MaximumLength(50).WithMessage("Icon không được vượt quá 50 ký tự.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(255).WithMessage("Đường dẫn ảnh không được vượt quá 255 ký tự.");

        RuleFor(x => x.ParentId)
            .Must((viewModel, parentId) =>
            {
                if (!parentId.HasValue) return true;
                if (parentId.Value == viewModel.Id && viewModel.Id != 0) return false;

                return _context.Set<Category>()
                                     .Any(c => c.Id == parentId.Value && c.Type == viewModel.Type);
            }).WithMessage("Danh mục cha không hợp lệ hoặc không cùng loại.");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Thứ tự phải là số không âm.");
    }

    private bool BeUniqueSlug(CategoryViewModel viewModel, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return true;

        return !_context.Set<Category>()
                .Any(c => c.Slug == slug
                            && c.Type == viewModel.Type
                            && c.Id != viewModel.Id);
    }
}