using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Article;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Validators.Article;

public class ArticleViewModelValidator : AbstractValidator<ArticleViewModel>
{
    private readonly ApplicationDbContext _context;

    public ArticleViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255)
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ chứa chữ thường, số, dấu gạch ngang.")
            .Must(BeUniqueSlug).WithMessage("{PropertyName} này đã tồn tại. Vui lòng chọn slug khác.");

        RuleFor(x => x.Content).NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");
        RuleFor(x => x.Summary).MaximumLength(500);
        RuleFor(x => x.FeaturedImage).MaximumLength(2048);
        RuleFor(x => x.ThumbnailImage).MaximumLength(2048);
        RuleFor(x => x.AuthorName).MaximumLength(100);
        RuleFor(x => x.AuthorAvatar).MaximumLength(255);
        RuleFor(x => x.Type).IsInEnum().WithMessage("{PropertyName} không hợp lệ.");
        RuleFor(x => x.Status).IsInEnum().WithMessage("{PropertyName} không hợp lệ.");

        RuleFor(x => x.SelectedCategoryIds)
            .NotEmpty().WithMessage("Vui lòng chọn ít nhất một {PropertyName}.");

        // Include base SEO validation rules
        Include(new SeoPropertiesValidator<ArticleViewModel>());
    }

    private bool BeUniqueSlug(ArticleViewModel viewModel, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return true;
        return !_context.Set<domain.Entities.Article>()
                               .Any(a => a.Slug.ToLower() == slug.ToLower() && a.Id != viewModel.Id);
    }
}