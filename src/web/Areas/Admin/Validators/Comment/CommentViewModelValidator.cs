using FluentValidation;
using web.Areas.Admin.ViewModels.Comment;

namespace web.Areas.Admin.Validators.Comment;

public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
{
    public CommentViewModelValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Vui lòng nhập nội dung bình luận")
            .MaximumLength(5000).WithMessage("Nội dung bình luận không được vượt quá 5000 ký tự");

        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("Vui lòng nhập tên tác giả")
            .MaximumLength(100).WithMessage("Tên tác giả không được vượt quá 100 ký tự");

        RuleFor(x => x.AuthorEmail)
            .EmailAddress().WithMessage("Email không hợp lệ")
            .MaximumLength(100).WithMessage("Email không được vượt quá 100 ký tự")
            .When(x => !string.IsNullOrEmpty(x.AuthorEmail));

        RuleFor(x => x.AuthorWebsite)
            .MaximumLength(255).WithMessage("Website không được vượt quá 255 ký tự")
            .Matches(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$")
            .WithMessage("Website không hợp lệ")
            .When(x => !string.IsNullOrEmpty(x.AuthorWebsite));

        RuleFor(x => x.AuthorAvatar)
            .MaximumLength(255).WithMessage("Avatar URL không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.AuthorAvatar));

        RuleFor(x => x.ArticleId)
            .NotEmpty().WithMessage("Bài viết không được để trống");
    }
}