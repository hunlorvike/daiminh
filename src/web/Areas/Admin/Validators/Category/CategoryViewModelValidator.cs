using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels.Category;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.Validators.Shared;
using shared.Enums;
using System.Linq;

namespace web.Areas.Admin.Validators.Category;

public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
{
    private readonly ApplicationDbContext _context;

    public CategoryViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .Must(BeUniqueSlugAndType).WithMessage("Slug này đã tồn tại cho loại danh mục này, vui lòng chọn slug khác.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Icon)
            .MaximumLength(50).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải là số không âm.");

        RuleFor(x => x.ParentId)
             .NotEqual(x => x.Id).When(x => x.ParentId.HasValue && x.Id > 0).WithMessage("Danh mục cha không thể là chính nó.")
             .Must(NotBeAChildOfItself).WithMessage("Không thể chọn danh mục con của chính nó làm danh mục cha.");

        RuleFor(x => x.Seo).SetValidator(new SeoViewModelValidator());
    }

    private bool BeUniqueSlugAndType(CategoryViewModel viewModel, string slug)
    {
        return !_context.Set<domain.Entities.Category>()
                              .Any(c => c.Slug == slug && c.Type == viewModel.Type && c.Id != viewModel.Id);
    }

    private bool NotBeAChildOfItself(CategoryViewModel viewModel, int? parentId)
    {
        if (!parentId.HasValue || viewModel.Id <= 0)
        {
            return true;
        }

        var descendantIds = GetDescendantIdsAsync(viewModel.Id);

        return !descendantIds.Contains(parentId.Value);
    }

    private List<int> GetDescendantIdsAsync(int categoryId)
    {
        var childrenIds = _context.Set<domain.Entities.Category>()
                                       .Where(c => c.ParentId == categoryId)
                                       .Select(c => c.Id)
                                       .ToList();

        var descendantIds = new List<int>(childrenIds);
        foreach (var childId in childrenIds)
        {
            descendantIds.AddRange(GetDescendantIdsAsync(childId));
        }
        return descendantIds;
    }
}