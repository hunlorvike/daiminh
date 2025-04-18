using System.Text.RegularExpressions;
using FluentValidation;
using shared.Enums;
using web.Areas.Admin.ViewModels.Setting;

namespace web.Areas.Admin.Validators.Setting;

public class SettingValidator : AbstractValidator<SettingViewModel>
{
    private static readonly Regex PhoneRegex = new Regex(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", RegexOptions.Compiled);

    public SettingValidator()
    {
        // Validate Value based on Type enum
        RuleFor(x => x.Value)
            .EmailAddress().WithMessage("Giá trị phải là một địa chỉ email hợp lệ.")
            .When(x => x.Type == FieldType.Email && !string.IsNullOrWhiteSpace(x.Value)); // Check type and non-empty

        RuleFor(x => x.Value)
            .Must(BeValidNumber!).WithMessage("Giá trị phải là một số hợp lệ.") // Use ! to satisfy nullable check
            .When(x => x.Type == FieldType.Number && !string.IsNullOrWhiteSpace(x.Value));

        RuleFor(x => x.Value)
            .Must(BeValidUrl!).WithMessage("Giá trị phải là một URL hợp lệ.") // Use ! to satisfy nullable check
            .When(x => x.Type == FieldType.Url && !string.IsNullOrWhiteSpace(x.Value));

        RuleFor(x => x.Value)
            .Matches(PhoneRegex).WithMessage("Giá trị phải là một số điện thoại hợp lệ.")
            .When(x => x.Type == FieldType.Phone && !string.IsNullOrWhiteSpace(x.Value));

        RuleFor(x => x.Value)
           .Must(BeValidBooleanString!).WithMessage("Giá trị phải là 'true' hoặc 'false'.")
           .When(x => x.Type == FieldType.Boolean && !string.IsNullOrWhiteSpace(x.Value)); // Validate boolean string representation

        // Add validation for Color (e.g., hex format) if needed
        // RuleFor(x => x.Value).Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$").WithMessage("Mã màu không hợp lệ.")
        //   .When(x => x.Type == FieldType.Color && !string.IsNullOrWhiteSpace(x.Value));
    }

    private bool BeValidNumber(string value)
    {
        return double.TryParse(value, out _);
    }

    private bool BeValidUrl(string value)
    {
        return Uri.TryCreate(value, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
    private bool BeValidBooleanString(string value)
    {
        return bool.TryParse(value, out _);
    }
}

