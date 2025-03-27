using FluentValidation;
using web.Areas.Admin.ViewModels.Seo;

namespace web.Areas.Admin.Validators;

public class GoogleSearchConsoleImportViewModelValidator : AbstractValidator<GoogleSearchConsoleImportViewModel>
{
    public GoogleSearchConsoleImportViewModelValidator()
    {
        RuleFor(x => x.JsonFile)
            .NotNull().WithMessage("Vui lòng chọn file JSON từ Google Search Console");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Vui lòng chọn ngày bắt đầu")
            .LessThanOrEqualTo(x => x.EndDate).WithMessage("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Vui lòng chọn ngày kết thúc")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Ngày kết thúc không thể trong tương lai");
    }
}
