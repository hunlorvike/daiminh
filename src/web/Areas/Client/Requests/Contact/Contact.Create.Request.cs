using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Client.Requests.Contact;

public class ContactCreateRequest
{
    [Display(Name = "Họ và tên", Prompt = "Nhập họ và tên của bạn")]
    public string? Name { get; set; }

    [Display(Name = "Email", Prompt = "Nhập email của bạn")]
    public string? Email { get; set; }

    [Display(Name = "Số điện thoại", Prompt = "Nhập số điện thoại của bạn")]
    public string? Phone { get; set; }

    [Display(Name = "Tin nhắn", Prompt = "Nhập nội dung tin nhắn")]
    public string? Message { get; set; }
}

public class ContactCreateRequestValidator : AbstractValidator<ContactCreateRequest>
{
    public ContactCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Họ và tên không được để trống")
            .MaximumLength(100).WithMessage("Họ và tên không được quá 100 ký tự");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống")
            .EmailAddress().WithMessage("Email không hợp lệ");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Số điện thoại không được để trống")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Số điện thoại không hợp lệ");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Tin nhắn không được để trống")
            .MaximumLength(500).WithMessage("Tin nhắn không được quá 500 ký tự");
    }
}