using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Tag;

public class TagViewModelValidator : AbstractValidator<TagViewModel>
{
    private readonly ApplicationDbContext _context;

    public TagViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên thẻ")
            .MaximumLength(50).WithMessage("Tên thẻ không được vượt quá 50 ký tự");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug")
            .MaximumLength(50).WithMessage("Slug không được vượt quá 50 ký tự")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang")
            .Must(BeUniqueSlug).WithMessage("Slug đã tồn tại cho loại thẻ này, vui lòng chọn slug khác");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Vui lòng chọn loại thẻ")
            .IsInEnum().WithMessage("Loại thẻ không hợp lệ");
    }

    private bool BeUniqueSlug(TagViewModel viewModel, string slug)
    {
        return !_context.Set<Tag>()
                .Any(t => t.Slug == slug
                    && t.Type == viewModel.Type
                    && t.Id != viewModel.Id);
    }
}
