using FluentValidation;
using web.Areas.Admin.ViewModels.Comment;

namespace web.Areas.Admin.Validators.Comment;

public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
{
    public CommentViewModelValidator()
    {
        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("Tên tác giả không được rỗng.")
            .MaximumLength(100);

        RuleFor(x => x.AuthorEmail)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.AuthorEmail))
            .MaximumLength(100);

        RuleFor(x => x.AuthorWebsite)
            .MaximumLength(255)
            .Matches(@"^(https?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$").When(x => !string.IsNullOrWhiteSpace(x.AuthorWebsite));

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Nội dung bình luận không được rỗng.");
    }
}