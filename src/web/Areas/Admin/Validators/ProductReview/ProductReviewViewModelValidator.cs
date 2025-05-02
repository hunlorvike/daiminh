using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels.ProductReview;

namespace web.Areas.Admin.Validators.ProductReview;

public class ProductReviewViewModelValidator : AbstractValidator<ProductReviewViewModel>
{
    private readonly ApplicationDbContext _context;

    public ProductReviewViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Không tìm thấy ID đánh giá để cập nhật.");

        RuleFor(x => x.Status)
             .IsInEnum().WithMessage("{PropertyName} không hợp lệ.");
    }
}