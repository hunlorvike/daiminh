using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Client.Requests.Subscriber;

    public class SubscriberCreateRequest
    {
        [Display(Name = "Tên email")]
        public string? Email { get; set; }
     
    }

    public class SubscriberEditRequestValidator : AbstractValidator<SubscriberCreateRequest>
    {
        public SubscriberEditRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ");
          
        }
    }


