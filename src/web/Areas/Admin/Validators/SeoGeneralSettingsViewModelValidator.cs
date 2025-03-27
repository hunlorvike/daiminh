using FluentValidation;
using web.Areas.Admin.ViewModels.Seo;

namespace web.Areas.Admin.Validators;

public class SeoGeneralSettingsViewModelValidator : AbstractValidator<SeoGeneralSettingsViewModel>
{
    public SeoGeneralSettingsViewModelValidator()
    {
        RuleFor(x => x.DefaultTitle)
            .NotEmpty().WithMessage("Tiêu đề mặc định không được để trống")
            .MaximumLength(70).WithMessage("Tiêu đề mặc định không nên vượt quá 70 ký tự");

        RuleFor(x => x.DefaultDescription)
            .NotEmpty().WithMessage("Mô tả mặc định không được để trống")
            .MaximumLength(160).WithMessage("Mô tả mặc định không nên vượt quá 160 ký tự");

        RuleFor(x => x.SiteName)
            .NotEmpty().WithMessage("Tên trang web không được để trống");

        RuleFor(x => x.GoogleAnalyticsId)
            .Matches(@"^(UA|G|AW|GTM)-[A-Za-z0-9\-]+$").When(x => !string.IsNullOrEmpty(x.GoogleAnalyticsId))
            .WithMessage("Google Analytics ID không hợp lệ");

        RuleFor(x => x.GoogleTagManagerId)
            .Matches(@"^GTM-[A-Za-z0-9]+$").When(x => !string.IsNullOrEmpty(x.GoogleTagManagerId))
            .WithMessage("Google Tag Manager ID không hợp lệ");

        RuleFor(x => x.FacebookAppId)
            .Matches(@"^\d+$").When(x => !string.IsNullOrEmpty(x.FacebookAppId))
            .WithMessage("Facebook App ID không hợp lệ");

        RuleFor(x => x.TwitterUsername)
            .Matches(@"^@?[A-Za-z0-9_]{1,15}$").When(x => !string.IsNullOrEmpty(x.TwitterUsername))
            .WithMessage("Twitter Username không hợp lệ");
    }
}
