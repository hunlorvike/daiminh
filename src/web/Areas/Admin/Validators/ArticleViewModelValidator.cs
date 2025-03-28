// ArticleViewModelValidator.cs
using System.Text.RegularExpressions;
using FluentValidation;
using shared.Enums;
using web.Areas.Admin.ViewModels.Article;

namespace web.Areas.Admin.Validators;

public class ArticleViewModelValidator : AbstractValidator<ArticleViewModel>
{
    public ArticleViewModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Vui lòng nhập tiêu đề bài viết")
            .MaximumLength(255).WithMessage("Tiêu đề không được vượt quá 255 ký tự");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự")
            .Matches(new Regex("^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            .WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Vui lòng nhập nội dung bài viết");

        RuleFor(x => x.Summary)
            .MaximumLength(500).WithMessage("Tóm tắt không được vượt quá 500 ký tự")
            .When(x => !string.IsNullOrEmpty(x.Summary));

        RuleFor(x => x.FeaturedImage)
            .MaximumLength(255).WithMessage("Đường dẫn ảnh đại diện không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.FeaturedImage));

        RuleFor(x => x.ThumbnailImage)
            .MaximumLength(255).WithMessage("Đường dẫn ảnh thumbnail không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.ThumbnailImage));

        RuleFor(x => x.AuthorName)
            .MaximumLength(100).WithMessage("Tên tác giả không được vượt quá 100 ký tự")
            .When(x => !string.IsNullOrEmpty(x.AuthorName));

        RuleFor(x => x.AuthorAvatar)
            .MaximumLength(255).WithMessage("Đường dẫn avatar tác giả không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.AuthorAvatar));

        RuleFor(x => x.MetaTitle)
            .MaximumLength(255).WithMessage("Meta title không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.MetaTitle));

        RuleFor(x => x.MetaDescription)
            .MaximumLength(500).WithMessage("Meta description không được vượt quá 500 ký tự")
            .When(x => !string.IsNullOrEmpty(x.MetaDescription));

        RuleFor(x => x.MetaKeywords)
            .MaximumLength(255).WithMessage("Meta keywords không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.MetaKeywords));

        RuleFor(x => x.PublishedAt)
            .NotEmpty().WithMessage("Vui lòng chọn ngày xuất bản")
            .When(x => x.Status == PublishStatus.Published);

        RuleFor(x => x.CategoryIds)
            .Must(x => x.Count > 0)
            .WithMessage("Vui lòng chọn ít nhất một danh mục");
    }
}