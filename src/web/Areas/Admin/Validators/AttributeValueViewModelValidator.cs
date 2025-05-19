using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class AttributeValueViewModelValidator : AbstractValidator<AttributeValueViewModel>
{
    private readonly ApplicationDbContext _context;

    public AttributeValueViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.AttributeId)
            .NotEmpty().WithMessage("Vui lòng chọn {PropertyName}.")
            .Must(AttributeExists).WithMessage("Thuộc tính cha được chọn không tồn tại.");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Slug)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Slug))
            .Matches("^[a-z0-9-]+$").When(x => !string.IsNullOrEmpty(x.Slug))
            .WithMessage("{PropertyName} chỉ được chứa chữ cái thường, số và dấu gạch ngang.");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải là số không âm.");
    }

    private bool AttributeExists(int attributeId)
    {
        return _context.Set<domain.Entities.Attribute>()
                      .Any(a => a.Id == attributeId);
    }
}