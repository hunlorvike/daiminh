using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Client.Requests.Subscriber;

public class SubscriberCreateRequest
{
    [Display(Name = "Tên email", Prompt = "Nhập email của bạn")]
    public string? Email { get; set; }
}

public class SubscriberCreateRequestValidator : AbstractValidator<SubscriberCreateRequest>
{
    public SubscriberCreateRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống")
            .EmailAddress().WithMessage("Email không hợp lệ");
    }
}