using System.Globalization;
using FluentValidation;
using shared.Enums;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class SettingViewModelValidator : AbstractValidator<SettingViewModel>
{
    public SettingViewModelValidator()
    {
        RuleFor(x => x.Value)
            .Must((model, value) => BeValidBasedOnType(model.Type, value))
            .WithMessage((model, value) => $"Giá trị nhập không hợp lệ cho loại '{model.Type}'. {GetExpectedFormatMessage(model.Type)}");

        // Có thể thêm các rule khác như NotEmpty nếu cần cho một số Key cụ thể
        // Ví dụ thêm rule cho một Key cụ thể nếu cần
        // When(x => x.Key == "SiteName", () => {
        //     RuleFor(x => x.Value).NotEmpty().WithMessage("Tên website không được để trống.");
        // });
    }

    private bool BeValidBasedOnType(FieldType type, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return true;
        }

        switch (type)
        {
            case FieldType.Text:
            case FieldType.TextArea:
            case FieldType.Html:
            case FieldType.Image:
            case FieldType.Phone:
            case FieldType.Color:
                if (type == FieldType.Color)
                    return System.Text.RegularExpressions.Regex.IsMatch(value, @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
                return true;

            case FieldType.Email:
                return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            case FieldType.Url:
                return Uri.TryCreate(value, UriKind.Absolute, out _);

            case FieldType.Number:
                return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);

            case FieldType.Boolean:
                string lowerValue = value.ToLowerInvariant();
                return lowerValue == "true" || lowerValue == "false" || lowerValue == "1" || lowerValue == "0";

            case FieldType.Date:
                return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

            default:
                return false;
        }
    }

    private string GetExpectedFormatMessage(FieldType type)
    {
        return type switch
        {
            FieldType.Email => "Định dạng email không đúng (ví dụ: user@example.com).",
            FieldType.Url => "Phải là một URL hợp lệ (ví dụ: http://example.com).",
            FieldType.Number => "Phải là một số (có thể có dấu thập phân).",
            FieldType.Boolean => "Phải là 'true', 'false', '1', hoặc '0'.",
            FieldType.Color => "Phải là mã màu hex (ví dụ: #FF0000 hoặc #F00).",
            FieldType.Phone => "Định dạng số điện thoại không đúng.",
            FieldType.Date => $"Phải là ngày hợp lệ (định dạng mong muốn: yyyy-MM-dd).",
            _ => string.Empty,
        };
    }
}