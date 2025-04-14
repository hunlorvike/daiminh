using FluentValidation;
using web.Areas.Admin.ViewModels.Comment;

namespace web.Areas.Admin.Validators.Comment;

public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
{
    public CommentViewModelValidator()
    {
        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("{PropertyName} không được rỗng.")
            .MaximumLength(100);

        RuleFor(x => x.AuthorEmail)
            .EmailAddress().WithMessage("{PropertyName} không hợp lệ.").When(x => !string.IsNullOrWhiteSpace(x.AuthorEmail))
            .MaximumLength(100);

        RuleFor(x => x.AuthorWebsite)
            .MaximumLength(255)
            .Matches(@"^(https?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$").WithMessage("{PropertyName} không hợp lệ.").When(x => !string.IsNullOrWhiteSpace(x.AuthorWebsite));

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("{PropertyName} không được rỗng.");
    }
}