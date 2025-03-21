using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

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
    private readonly ApplicationDbContext _dbContext;
    /// <summary>
    /// Initializer a new instance of the <see cref="SubscriberDeleteRequestValidator"/> class.
    /// </summary>
    public SubscriberDeleteRequestValidator(ApplicationDbContext context)
    {
        _dbContext = context;

        RuleFor(x => x.Id)
          .GreaterThan(0).WithMessage("ID người đăng ký phải là một số nguyên dương.")
          .MustAsync(BeExistingSubscriber).WithMessage("Người đăng ký không tồn tại hoặc đã bị xoá");
    }

    private async Task<bool> BeExistingSubscriber(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Subscribers
            .AnyAsync(s => s.Id == id && s.DeletedAt == null, cancellationToken);
    }
}