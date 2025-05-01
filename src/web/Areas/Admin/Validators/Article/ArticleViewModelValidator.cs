using FluentValidation;
using infrastructure;
using shared.Enums;
using web.Areas.Admin.Validators.Shared;
using web.Areas.Admin.ViewModels.Article;

namespace web.Areas.Admin.Validators.Article;

public class ArticleViewModelValidator : AbstractValidator<ArticleViewModel>
{
    private readonly ApplicationDbContext _context;

    public ArticleViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .Must(BeUniqueSlug).WithMessage("Slug này đã tồn tại, vui lòng chọn slug khác.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");

        RuleFor(x => x.Summary)
            .MaximumLength(500).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.FeaturedImage)
            .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.FeaturedImage));
        RuleFor(x => x.ThumbnailImage)
            .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.ThumbnailImage));

        RuleFor(x => x.AuthorName)
             .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.AuthorAvatar)
              .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.AuthorAvatar));

        RuleFor(x => x.EstimatedReadingMinutes)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải là số không âm.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Vui lòng chọn {PropertyName}.")
            .Must((model, categoryId) => CategoryExists(categoryId)).WithMessage("Danh mục được chọn không tồn tại.");

        RuleFor(x => x.Status)
             .IsInEnum().WithMessage("{PropertyName} không hợp lệ.");

        RuleFor(x => x.PublishedAt)
             .NotNull().When(x => x.Status == PublishStatus.Published || x.Status == PublishStatus.Scheduled)
             .WithMessage("Ngày xuất bản không được để trống khi trạng thái là Đã xuất bản hoặc Đã lên lịch.");

        Include(new SeoViewModelValidator());
    }

    private bool BeUniqueSlug(ArticleViewModel viewModel, string slug)
    {
        return !_context.Set<domain.Entities.Article>()
                              .Any(a => a.Slug == slug && a.Id != viewModel.Id);
    }

    private bool CategoryExists(int? categoryId)
    {
        if (categoryId == null) return false;

        return _context.Set<domain.Entities.Category>()
                       .Any(c => c.Id == categoryId && c.Type == shared.Enums.CategoryType.Article);
    }
}