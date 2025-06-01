using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class PopupModalViewModelValidator : AbstractValidator<PopupModalViewModel>
{
    private readonly ApplicationDbContext _context;

    public PopupModalViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.ImageUrl)
             .MaximumLength(2048).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
             .Matches(@"^(https?://|/).*$").When(x => !string.IsNullOrWhiteSpace(x.ImageUrl)).WithMessage("{PropertyName} phải là một URL hợp lệ (http, https hoặc tương đối /).");

        RuleFor(x => x.LinkUrl)
            .MaximumLength(2048).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches(@"^(https?://|/|mailto:|tel:|#).*$").When(x => !string.IsNullOrWhiteSpace(x.LinkUrl)).WithMessage("{PropertyName} phải là một URL, đường dẫn tương đối, mailto, tel hoặc neo (#) hợp lệ.");

        RuleFor(x => x.TargetPages)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(1000).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.DisplayFrequency)
            .IsInEnum().WithMessage("{PropertyName} không hợp lệ.");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải là số không âm.");

        RuleFor(x => x.DelaySeconds)
            .InclusiveBetween(0, 300).WithMessage("{PropertyName} phải từ {FromInclusive} đến {ToInclusive} giây.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Ngày kết thúc phải lớn hơn hoặc bằng Ngày bắt đầu.");
    }
}