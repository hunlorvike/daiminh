using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.ContentType;

public class ContentTypeCreateRequest
{
    [Display(Name = "Tên loại bài viết", Prompt = "Nhập tên của loại bài viết")]
    public string? Name { get; set; }

    [Display(Name = "Đường dẫn bài viết", Prompt = "Nhập đường dẫn của loại bài viết")]
    public string? Slug { get; set; }
}

public class ContentTypeUpdateRequest
{
    public int Id { get; set; }

    [Display(Name = "Tên loại bài viết", Prompt = "Nhập tên của loại bài viết")]
    public string? Name { get; set; }

    [Display(Name = "Đường dẫn bài viết", Prompt = "Nhập đường dẫn của loại bài viết")]
    public string? Slug { get; set; }
}

public class ContentTypeDeleteRequest
{
    public int Id { get; set; }
}

public class ContentTypeCreateRequestValidator : AbstractValidator<ContentTypeCreateRequest>
{
    public ContentTypeCreateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(255).WithMessage("Tên không được vượt quá 255 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Slug không được để trống.")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            // .MustAsync(BeUniqueSlug).WithMessage("Slug đã tồn tại.");
            ;
    }

    // private async Task<bool> BeUniqueSlug(string slug, CancellationToken cancellationToken)
    // {
    //     return !await _contentTypeService.IsSlugExistAsync(slug);
    // }
}

public class ContentTypeUpdateRequestValidator : AbstractValidator<ContentTypeUpdateRequest>
{
    public ContentTypeUpdateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(255).WithMessage("Tên không được vượt quá 255 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Slug không được để trống.")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            // .MustAsync(BeUniqueSlug).WithMessage("Slug đã tồn tại.");
            ;
    }


    // private async Task<bool> BeUniqueSlug(string slug, CancellationToken cancellationToken)
    // {
    //     return !await _contentTypeService.IsSlugExistAsync(slug);
    // }
}