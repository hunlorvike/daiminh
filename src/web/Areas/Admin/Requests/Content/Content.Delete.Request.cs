using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Content;

public class ContentDeleteRequest
{
    [Required]
    public int Id { get; set; }
}
public class ContentDeleteRequestValidator : AbstractValidator<ContentDeleteRequest>
{
    public ContentDeleteRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID nội dung phải là một số nguyên dương.");
    }
}