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

        // We could add rules here to prevent changing status if the review was already approved/rejected,
        // or add validation for User/Email/Rating/Content if they were editable by admin (they aren't in the ViewModel)
    }
}