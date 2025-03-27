using FluentValidation;
using web.Areas.Admin.ViewModels.Redirect;

namespace web.Areas.Admin.Validators;

public class RedirectViewModelValidator : AbstractValidator<RedirectViewModel>
{
    public RedirectViewModelValidator()
    {
        RuleFor(x => x.SourceUrl)
            .NotEmpty().WithMessage("Vui lòng nhập URL nguồn")
            .MaximumLength(500).WithMessage("URL nguồn không được vượt quá 500 ký tự");

        RuleFor(x => x.TargetUrl)
            .NotEmpty().WithMessage("Vui lòng nhập URL đích")
            .MaximumLength(500).WithMessage("URL đích không được vượt quá 500 ký tự");

        RuleFor(x => x.Notes)
            .MaximumLength(255).WithMessage("Ghi chú không được vượt quá 255 ký tự");

        // Validate regex pattern if IsRegex is true
        When(x => x.IsRegex, () =>
        {
            RuleFor(x => x.SourceUrl)
                .Must(BeValidRegex).WithMessage("URL nguồn không phải là biểu thức regex hợp lệ");
        });
    }

    private bool BeValidRegex(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
            return false;

        try
        {
            // Try to create a regex from the pattern
            var regex = new System.Text.RegularExpressions.Regex(pattern);
            return true;
        }
        catch
        {
            return false;
        }
    }
}