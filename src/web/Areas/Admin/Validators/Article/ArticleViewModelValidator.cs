using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Article;

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

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Vui lòng chọn một {PropertyName}.");

    }

    private bool BeUniqueSlug(ArticleViewModel viewModel, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return true;
        return !_context.Set<domain.Entities.Article>()
                               .Any(a => a.Slug.ToLower() == slug.ToLower() && a.Id != viewModel.Id);
    }
}