using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class SlideViewModelValidator : AbstractValidator<SlideViewModel>
{
    private readonly ApplicationDbContext _context;

    public SlideViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(150).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Subtitle)
            .MaximumLength(150).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches(@"^(https?://|/).*$").When(x => !string.IsNullOrWhiteSpace(x.ImageUrl)).WithMessage("{PropertyName} phải là một URL hợp lệ (http, https hoặc tương đối /).");

        RuleFor(x => x.CtaText)
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.CtaLink)
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches(@"^(https?://|/|mailto:|tel:|#).*$").When(x => !string.IsNullOrWhiteSpace(x.CtaLink)).WithMessage("{PropertyName} phải là một URL, đường dẫn tương đối, mailto, tel hoặc neo (#) hợp lệ.");

        RuleFor(x => x.Target)
           .MaximumLength(10).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải là số không âm.");

        RuleFor(x => x.EndAt)
            .GreaterThanOrEqualTo(x => x.StartAt)
            .When(x => x.StartAt.HasValue && x.EndAt.HasValue)
            .WithMessage("Thời gian kết thúc phải lớn hơn hoặc bằng Thời gian bắt đầu.");
    }
}