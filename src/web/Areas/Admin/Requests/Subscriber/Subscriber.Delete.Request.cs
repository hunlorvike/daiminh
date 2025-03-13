using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Subscriber;

/// <summary>
/// Represents a request to delete a subscriber.
/// </summary>
public class SubscriberDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the subscriber to delete.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }
}

/// <summary>
/// Validator for <see cref="SubscriberDeleteRequest"/>
/// </summary>
public class SubscriberDeleteRequestValidator : AbstractValidator<SubscriberDeleteRequest>
{
    /// <summary>
    /// Initializer a new instance of the <see cref="SubscriberDeleteRequestValidator"/> class.
    /// </summary>
    public SubscriberDeleteRequestValidator()
    {
        RuleFor(x => x.Id)
          .GreaterThan(0).WithMessage("ID người đăng ký phải là một số nguyên dương.");
    }
}